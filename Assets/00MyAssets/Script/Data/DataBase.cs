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
    public GameObject PlayerHitEffect;
    public GameObject EnemyHitEffect;

    public Data_Chara[] Charas;
    public Data_Atk[] N_Atks;
    public Data_Atk[] S_Atks;
    public Data_Atk[] E_Atks;
    public List<GameObject> Wepons;
    public Data_Stage[] Stages;
}
