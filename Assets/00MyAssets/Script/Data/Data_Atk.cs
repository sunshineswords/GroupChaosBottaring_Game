using UnityEngine;
[CreateAssetMenu(menuName ="DataCre/Atk")]
public class Data_Atk : ScriptableObject
{
    public string Name;
    public string Info;
    public Texture Icon;
    public int EndTime;
    public float CT;
    public int SPUse;
    public bool Aiming;

    public Move_FixedC[] Move_Fixeds;
    public ShotC_Base[] Shots;
    public AtkBranchC[] Branch;
    public AnimC[] Anims;
    [System.Serializable]
    public class Move_FixedC
    {
        public Vector2Int Times;
        public float SpeedRem;
        public bool NoJump;
        public bool NoDash;
    }
    [System.Serializable]
    public class ShotC_Base
    {
        public GameObject Obj;
        public ShotC_Fire[] Fires;
        public ShotC_Hits Hits;
    }
    [System.Serializable]
    public class ShotC_Fire
    {
        public Vector3Int Times;
        public int Count;
        public Vector2 Speed;
        public ShotC_Trans Trans;
    }
    public enum PosBaseE
    {
        使用者位置,
        ターゲット位置,
    }
    public enum RotBaseE
    {
        固定=-1,
        使用者向き,
        ターゲット方向,
        使用者カメラ方向,
    }
    [System.Serializable]
    public class ShotC_Trans
    {
        public PosBaseE PosBase;
        public Vector3 PosChange;
        public Vector3 PosRand;
        public Vector3 PosWay;
        public Vector3 PosTime;
        public RotBaseE RotBase;
        public Vector3 RotChange;
        public Vector3 RotRand;
        public Vector3 RotWay;
        public Vector3 RotTime;
    }
    [System.Serializable]
    public class ShotC_Hits
    {
        public int BaseDam;
        public float AtkDamPer;
        public float DefDamPer;
        public float DefRemPer;
        public int HitCT;
    }
    [System.Serializable]
    public class AtkBranchC
    {
        public Vector2Int Times;
        public AtkIfE[] Ifs;
        public Data_Atk FutureAtk;
    }
    public enum AtkIfE
    {
        攻撃単入力,
        攻撃長入力,
        攻撃未入力,
    }
    [System.Serializable]
    public class AnimC
    {
        public Vector2Int Times;
        public int ID;
    }
}
