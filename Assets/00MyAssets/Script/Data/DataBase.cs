using UnityEngine;
using System.Collections.Generic;
using static Manifesto;
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

    [EnumIndex(typeof(Enum_Passive))]
    public Data_Passive[] Passives;
    [EnumIndex(typeof(Enum_PassiveAtk))]
    public Data_AddShot[] PassiveAtks;

    public List<GameObject> Wepons;
    public List<AudioClip> SEs;
    public Data_Stage[] Stages;
    public List<Data_Buf> Bufs;

}
