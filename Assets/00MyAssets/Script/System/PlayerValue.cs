using UnityEngine;

public class PlayerValue
{
    static public PSavesC PSaves;

    public class PSavesC
    {
        public int CharaID;
        public int N_AtkID;
        public int S1_AtkID;
        public int E_AtkID;
        public PSavesC()
        {
            CharaID = 0;
            N_AtkID = 0;
            S1_AtkID = 0;
            E_AtkID = 0;
        }
    }

    static public void Save()
    {
        var PSaves_Json = JsonUtility.ToJson(PSaves);
        PlayerPrefs.SetString("PSaves", PSaves_Json);
    }
    static public void Load()
    {
        var PSaves_Json = PlayerPrefs.GetString("PSaves", "");
        PSaves = new PSavesC();
        if (PSaves_Json != "")
        {
            var PSaves_C = JsonUtility.FromJson<PSavesC>(PSaves_Json);
            PSaves = PSaves_C;
        }
    }
}
