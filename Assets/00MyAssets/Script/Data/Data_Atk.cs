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

    public FixedC[] Fixeds;
    public ShotC_Base[] Shots;
    public AtkBranchC[] Branchs;
    public AnimC[] Anims;
    [System.Serializable]
    public class FixedC
    {
        [HideInInspector] public string EditDisp;
        public int BranchNum;
        public Vector2Int Times;
        public float SpeedRem;
        public bool NoJump;
        public bool NoDash;
        public bool Aiming;
    }
    [System.Serializable]
    public class ShotC_Base
    {
        public GameObject Obj;
        public ShotC_Fire[] Fires;
        public ShotC_Hit[] Hits;
        public int HitCT;
        public void EditDispSet()
        {
            for(int i = 0; i < Fires.Length; i++)
            {
                var Fire = Fires[i];
                Fire.EditDisp = "[" + i + "]";
                Fire.EditDisp += "BNum:" + Fire.BranchNum;
                Fire.EditDisp += ",Time:" + Fire.Times;
                Fire.EditDisp += ",Count:" + Fire.Count;
                Fire.EditDisp += ",Speed:" + Fire.Speed;
            }
            for (int i = 0; i < Hits.Length; i++)
            {
                var Hit = Hits[i];
                Hit.EditDisp = "[" + i + "]";
                Hit.EditDisp += "BNum:" + Hit.BranchNum;
            }

        }
    }
    [System.Serializable]
    public class ShotC_Fire
    {
        [HideInInspector] public string EditDisp;
        public int BranchNum;
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
    public class ShotC_Hit
    {
        [HideInInspector] public string EditDisp;
        public int BranchNum;
        public int BaseDam;
        public float AtkDamPer;
        public float DefDamPer;
        public float DefRemPer;
        public int SPAdd;
    }
    [System.Serializable]
    public class AtkBranchC
    {
        [HideInInspector] public string EditDisp;
        public int[] BranchNums;
        public Vector2Int Times;
        public AtkIfE[] Ifs;
        public int FutureNum;
        public int FutureTime;
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
        [HideInInspector] public string EditDisp;
        public int BranchNum;
        public Vector2Int Times;
        public int ID;
    }


    private void OnValidate()
    {
        if(Fixeds!=null)
        for (int i = 0; i < Fixeds.Length; i++)
        {
            var Fixed = Fixeds[i];
            Fixed.EditDisp = "[" + i + "]";
            Fixed.EditDisp += "BNum:" + Fixed.BranchNum;
            Fixed.EditDisp += "Time:" + Fixed.Times;
        }
        if (Shots != null)
        for (int i = 0; i < Shots.Length; i++) Shots[i].EditDispSet();
        if (Branchs != null)
            for (int i = 0; i < Branchs.Length; i++)
            {
                var Branch = Branchs[i];
                Branch.EditDisp = "[" + i + "]";
                Branch.EditDisp += "BNum:";
                for (int j = 0; j < Branch.BranchNums.Length; j++)
                {
                    if (j > 0) Branch.EditDisp += ",";
                    Branch.EditDisp += Branch.BranchNums[j];
                }
                Branch.EditDisp += "Time:" + Branch.Times;
                Branch.EditDisp += "Future{Num:" + Branch.FutureNum;
                Branch.EditDisp += "Time:" + Branch.FutureTime + "}";
            }
        if (Anims != null)
        for (int i = 0; i < Anims.Length; i++)
        {
            var Anim = Anims[i];
            Anim.EditDisp = "[" + i + "]";
            Anim.EditDisp += "BNum:" + Anim.BranchNum;
            Anim.EditDisp += "Time:" + Anim.Times;
            Anim.EditDisp += "ID:" + Anim.ID;
        }
    }
}
