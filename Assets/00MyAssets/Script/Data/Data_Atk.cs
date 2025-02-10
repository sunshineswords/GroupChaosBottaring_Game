using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(menuName ="DataCre/Atk")]
public class Data_Atk : ScriptableObject
{
    const string Ttp_BID = "分岐ID条件";
    const string Ttp_Times = "時間条件\n攻撃時間がx～yフレームの間\nzフレーム間隔";
    public string Name;
    [TextArea]public string Info;
    public Texture Icon;
    [Tooltip("終了時間(フレーム)")] public int EndTime;
    [Tooltip("CT(秒)")] public float CT;
    [Tooltip("SP消費")] public int SPUse;

    [Header("分岐情報")]public List<BranchInfoC> BranchInfos;
    [Header("分岐先")] public AtkBranchC[] Branchs;
    [Header("制限")] public FixedC[] Fixeds;
    [Header("弾発射")] public ShotC_Base[] Shots;
    [Header("武器表示")] public WeponSetC[] WeponSets;
    [Header("アニメーション")] public AnimC[] Anims;
    [System.Serializable]
    public class BranchInfoC
    {
        [Tooltip(Ttp_BID)] public int BID;
        [Tooltip("分岐名")] public string Name;
    }
    [System.Serializable]
    public class FixedC
    {
        [HideInInspector] public string EditDisp;
        [Tooltip(Ttp_BID)] public int BranchNum;
        [Tooltip(Ttp_Times)] public Vector2Int Times;
        [Tooltip("移動減速")] public float SpeedRem;
        [Tooltip("ジャンプ不可")] public bool NoJump;
        [Tooltip("ダッシュ不可")] public bool NoDash;
        [Tooltip("照準モード")] public bool Aiming;
    }
    [System.Serializable]
    public class WeponSetC
    {
        [Tooltip(Ttp_BID)] public int BranchNum;
        [Tooltip(Ttp_Times)] public Vector2Int Times;
        [Tooltip("武器オブジェクト")] public GameObject Obj;
        [Tooltip("表示部位")] public WeponSetE Set;
        [Tooltip("位置ズレ")] public Vector3 PosChange;
        [Tooltip("角度ズレ")] public Vector3 RotChange;
    }
    public enum WeponSetE
    {
        右手 = 0,
        左手 = 1,
        右足 = 10,
        左足 = 11,
        胴体 = 20,
        頭 = 30,
    }
    [System.Serializable]
    public class ShotC_Base
    {
        [Tooltip("弾オブジェクト")] public GameObject Obj;
        [Tooltip("発射")] public ShotC_Fire[] Fires;
        [Tooltip("命中効果")] public ShotC_Hit[] Hits;
        [Tooltip("多段ヒットCT(0以下は単発ヒット)")] public int HitCT;
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
        [Tooltip(Ttp_BID)] public int BranchNum;
        [Tooltip(Ttp_Times)] public Vector3Int Times;
        [Tooltip("弾数")] public int Count;
        [Tooltip("弾速度x～y")] public Vector2 Speed;
        [Tooltip("発射形状")] public ShotC_Trans Trans;
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
        [Tooltip("座標基準")] public PosBaseE PosBase;
        [Tooltip("座標ズレ")] public Vector3 PosChange;
        [Tooltip("座標ブレ")] public Vector3 PosRand;
        [Tooltip("座標拡散")] public Vector3 PosWay;
        [Tooltip("座標時間変化")] public Vector3 PosTime;
        [Tooltip("角度基準")] public RotBaseE RotBase;
        [Tooltip("角度ズレ")] public Vector3 RotChange;
        [Tooltip("角度ブレ")] public Vector3 RotRand;
        [Tooltip("角度拡散")] public Vector3 RotWay;
        [Tooltip("角度時間変化")] public Vector3 RotTime;
    }
    [System.Serializable]
    public class ShotC_Hit
    {
        [HideInInspector] public string EditDisp;
        [Tooltip(Ttp_BID)] public int BranchNum;
        [Tooltip("基礎ダメージ")] public int BaseDam;
        [Tooltip("使用者攻撃力依存%")] public float AtkDamPer;
        [Tooltip("使用者防御力依存%")] public float DefDamPer;
        [Tooltip("対象防御軽減%")] public float DefRemPer;
        [Tooltip("命中時SP増加量")] public int SPAdd;
    }
    [System.Serializable]
    public class AtkBranchC
    {
        [HideInInspector] public string EditDisp;
        [Tooltip(Ttp_BID+"いずれか")] public int[] BranchNums;
        [Tooltip(Ttp_Times)] public Vector2Int Times;
        [Tooltip("追加条件")] public AtkIfE[] Ifs;
        [Tooltip("分岐先ID")] public int FutureNum;
        [Tooltip("分岐後攻撃時間(フレーム)")] public int FutureTime;
    }
    public enum AtkIfE
    {
        攻撃単入力,
        攻撃長入力,
        攻撃未入力,
        攻撃未長入力,
    }
    [System.Serializable]
    public class AnimC
    {
        [HideInInspector] public string EditDisp;
        [Tooltip(Ttp_BID)] public int BranchNum;
        [Tooltip(Ttp_Times)] public Vector2Int Times;
        [Tooltip("アニメーションID")] public int ID;
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
