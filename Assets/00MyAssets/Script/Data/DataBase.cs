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
    public Data_Passive[] Passives;
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
    public enum PassiveE
    {
        HP増加,
        自然再生,
        MP増加,
        気力増幅,
        SPブースト,
        攻撃力増加,
        防御力増加,
        速度増加,
        CTカット,
        必殺再生,
        必殺返還,
        タルタル,
        根性,
        死に力,
    }
    public enum BufsE
    {
        HP増加 = 0,
        攻撃増加 = 10,
        防御増加 = 20,

        毒 = 1000,
        HP再生 = 2000,
    }
    public enum BufSetE
    {
        付与,
        付与増加,
        不付与増加,
        切り替え,
        消去,
    }
    [System.Serializable]
    public class BufSetC
    {
        public BufsE Buf;
        public int Index;
        public BufSetE Set;
        public Vector2Int Time;
        public Vector2Int Pow;
    }
}
