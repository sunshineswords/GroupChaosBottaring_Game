using System.IO;
using UnityEngine;


public class PlayerValue
{
    static string Paths { get { return Application.persistentDataPath+"/"; } }
    static public int StageID = 0;
    static public PSavesC PSaves;
    static public PriSetC[] PriSets = new PriSetC[10];
    static public PriSetC PriSetGet => PriSets[PSaves.PriSetID];
    [System.Serializable]
    public class PSavesC
    {
        public int PriSetID;
        public PSavesC()
        {
            PriSetID = 0;
        }
    }
    [System.Serializable]
    public class PriSetC
    {
        public int CharaID;
        public AtksC AtkF;
        public AtksC AtkB;
        public PriSetC()
        {
            CharaID = 0;
            AtkF = new AtksC();
            AtkB = new AtksC();
        }
        public AtksC AtkGet(bool Back)
        {
            return !Back ? AtkF : AtkB;
        }
    }
    [System.Serializable]
    public class AtksC
    {
        public int N_AtkID;
        public int S1_AtkID;
        public int S2_AtkID;
        public int E_AtkID;
        public AtksC()
        {
            N_AtkID = 0;
            S1_AtkID = 0;
            S2_AtkID = 1;
            E_AtkID = 0;
        }
    }

    static public void Save()
    {
        var PSaves_Json = JsonUtility.ToJson(PSaves);
        SaveFile("PSaves.data",PSaves_Json);
        Debug.Log(PSaves_Json);
        for (int i = 0; i < PriSets.Length; i++)
        {
            var PriSet_Json = JsonUtility.ToJson(PriSets[i]);
            SaveFile("Priset_"+(i+1)+".data", PriSet_Json);
            Debug.Log("PriSet_" + i + ":" + PriSet_Json);
        }
    }
    static public void Load()
    {
        var PSaves_Json = LoadFile("PSaves.data");
        PSaves = new PSavesC();
        if (PSaves_Json != "")
        {
            var PSaves_C = JsonUtility.FromJson<PSavesC>(PSaves_Json);
            PSaves = PSaves_C;
        }
        for (int i = 0; i < PriSets.Length; i++)
        {
            var PriSet_Json = LoadFile("Priset_" + (i + 1) + ".data");
            PriSets[i] = new PriSetC();
            if (PriSet_Json != "")
            {
                var PriSet_C = JsonUtility.FromJson<PriSetC>(PriSet_Json);
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
