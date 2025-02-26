using System.IO;
using UnityEngine;
using static Manifesto;
using static DataBase;
public class PlayerValue
{
    static string Paths { get { return Application.persistentDataPath+"/"; } }
    static public int StageID = 0;
    static public Class_Save_PSaves PSaves;
    static public Class_Save_PriSet[] PriSets = new Class_Save_PriSet[10];
    static public Class_Save_PriSet PriSetGet => PriSets[PSaves.PriSetID];

    static public void Save()
    {
        var PSaves_Json = JsonUtility.ToJson(PSaves);
        SaveFile("PSaves.data",PSaves_Json);
        var DebugStr = "SaveJson\n(PSaves)\n" + PSaves_Json;
        for (int i = 0; i < PriSets.Length; i++)
        {
            var PriSet_Json = JsonUtility.ToJson(PriSets[i]);
            SaveFile("Priset_"+(i+1)+".data", PriSet_Json);
            DebugStr += "\n(PriSet_" + (i + 1) + ")\n" + PriSet_Json;
        }
        Debug.Log(DebugStr);
    }
    static public void Load()
    {
        var PSaves_Json = LoadFile("PSaves.data");
        PSaves = new Class_Save_PSaves();
        if (PSaves_Json != "")
        {
            var PSaves_C = JsonUtility.FromJson<Class_Save_PSaves>(PSaves_Json);
            PSaves = PSaves_C;
        }
        for (int i = PSaves.StageSoloStars.Count - 1; i < DB.Stages.Length; i++)
        {
            PSaves.StageSoloStars.Add(0);
        }
        for (int i = PSaves.StageMultStars.Count - 1; i < DB.Stages.Length; i++)
        {
            PSaves.StageMultStars.Add(0);
        }
        for (int i = 0; i < PriSets.Length; i++)
        {
            var PriSet_Json = LoadFile("Priset_" + (i + 1) + ".data");
            PriSets[i] = new Class_Save_PriSet();
            if (PriSet_Json != "")
            {
                var PriSet_C = JsonUtility.FromJson<Class_Save_PriSet>(PriSet_Json);
                PriSets[i] = PriSet_C;
            }
        }

    }

    static void SaveFile(string FileName,string Str)
    {
        var Writer = new StreamWriter(Paths + FileName);
        Writer.Write(Str);
        Writer.Flush();
        Writer.Close();
    }
    static string LoadFile(string FileName)
    {
        try
        {
            var reader = new StreamReader(Paths + FileName);
            string Str = reader.ReadToEnd();
            reader.Close();
            return Str;
        }
        catch
        {
            return "";
        }
    }
}
