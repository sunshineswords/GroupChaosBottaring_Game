using UnityEngine;
using System.Collections.Generic;
[CreateAssetMenu(menuName ="DataCre/DataBase")]
public class DataBase : ScriptableObject
{
    static DataBase DBs;
    static public DataBase DB
    {
        get
        {
            if (DBs == null) DBs = (DataBase)Resources.Load("DataBase");
            return DBs;
        }
    }
    public DamageObj DamageObjs;
    public AudioSource SEObj;
    public GameObject[] HitEffects;
    public GameObject[] HealEffects;

    public Data_Chara[] Charas;
    public Data_Atk[] N_Atks;
    public Data_Atk[] S_Atks;
    public Data_Atk[] E_Atks;
    public List<GameObject> Wepons;
    public List<AudioClip> SEs;
    public Data_Stage[] Stages;

    [System.Serializable]
    public class SEPlayC
    {
        [Tooltip("SEファイル")] public AudioClip Clip;
        [Tooltip("音量")] public float Volume = 100f;
        [Tooltip("音程-300～300"), Range(-300f, 300f)] public float Pitch = 100f;
    }
}
