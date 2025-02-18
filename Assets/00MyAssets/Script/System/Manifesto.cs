using UnityEngine;
using static Manifesto;
static public class Manifesto
{
    #region Const
    public const string Const_Ttp_BID = "分岐ID条件\n-だと無条件";
    public const string Const_Ttp_Times = "時間条件\n攻撃時間がx～yフレームの間\nzフレーム間隔";
    #endregion

    #region Class
    [System.Serializable]
    public class Class_Base_SEPlay
    {
        [Tooltip("SEファイル")] public AudioClip Clip;
        [Tooltip("音量")] public float Volume = 100f;
        [Tooltip("音程-300～300"), Range(-300f, 300f)] public float Pitch = 100f;
    }
    [System.Serializable]
    public class Class_Base_BufSet
    {
        public Enum_Bufs Buf;
        [Tooltip("状態番号")] public int Index;
        [Tooltip("付与処理")]public Enum_BufSet Set;
        [Tooltip("付与時間フレーム\nx=0以下だと永続\ny=上限,0以下は上限無し")] public Vector2Int Time;
        [Tooltip("付与段階\nx=0以下だと段階表示なし\ny=上限,0以下は上限無し")] public Vector2Int Pow;
    }

    [System.Serializable]
    public class Class_Atk_BranchInfo
    {
        [Tooltip(Const_Ttp_BID)] public int BID;
        [Tooltip("分岐名")] public string Name;
    }
    [System.Serializable]
    public class Class_Atk_Branch
    {
        [HideInInspector] public string EditDisp;
        [Tooltip("分岐ID条件いずれか")] public int[] BranchNums;
        [Tooltip(Const_Ttp_Times)] public Vector2Int Times;
        [Tooltip("追加条件")] public Enum_AtkIf[] Ifs;
        [Tooltip("MP消費")] public float UseMP;
        [Tooltip("分岐先ID")] public int FutureNum;
        [Tooltip("分岐後攻撃時間(フレーム)")] public int FutureTime;
    }
    [System.Serializable]
    public class Class_Atk_Fixed
    {
        [HideInInspector] public string EditDisp;
        [Tooltip(Const_Ttp_BID)] public int BranchNum;
        [Tooltip(Const_Ttp_Times)] public Vector2Int Times;
        [Tooltip("移動減速")] public float SpeedRem;
        [Tooltip("ジャンプ不可")] public bool NoJump;
        [Tooltip("ダッシュ不可")] public bool NoDash;
        [Tooltip("照準モード")] public bool Aiming;
        [Tooltip("無重力")] public bool NGravity;
    }
    [System.Serializable]
    public class Class_Atk_Shot_Base
    {
        [Tooltip("弾オブジェクト")] public GameObject Obj;
        [Tooltip("発射")] public Class_Atk_Shot_Fire[] Fires;
        [Tooltip("命中効果")] public Class_Atk_Shot_Hit[] Hits;
        [Tooltip("多段ヒットCT(0以下は単発ヒット)")] public int HitCT;
        public void EditDispSet()
        {
            for (int i = 0; i < Fires.Length; i++)
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
                Hit.EditDisp += "BNum:" + Hit.BranchNum + "|(";
                Hit.EditDisp += Hit.DamageType+",";
                Hit.EditDisp += Hit.ShortAtk ? "近距離" : "遠距離";
                Hit.EditDisp += ")";
                if (Hit.BaseDam != 0) Hit.EditDisp += Hit.BaseDam;
                if (Hit.AtkDamPer != 0) Hit.EditDisp += "攻撃力" + Hit.AtkDamPer + "%";
                if (Hit.DefDamPer != 0) Hit.EditDisp += "防御力" + Hit.DefDamPer + "%";
                if (Hit.DefRemPer != 0) Hit.EditDisp += "防御影響" + Hit.DefRemPer + "%";
            }

        }
        public string OtherStrGet(int BNum)
        {
            string Str = "";
            int ShotCounts = 0;
            for (int i = 0; i < Fires.Length; i++)
            {
                var Fire = Fires[i];
                if (Fire.BranchNum != BNum) continue;
                int TCounts = (Fire.Times.y - Fire.Times.x) / Mathf.Max(1, Fire.Times.z) + 1;
                ShotCounts += Fire.Count * TCounts;
            }
            for (int i = 0; i < Hits.Length; i++)
            {
                var Hit = Hits[i];
                if (Hit.BranchNum != BNum) continue;
                if (Str != "") Str += "\n";
                if (Hit.BaseDam != 0) Str += Hit.BaseDam;
                if (Hit.AtkDamPer != 0) Str += "攻撃力" + Hit.AtkDamPer + "%";
                if (Hit.DefDamPer != 0) Str += "防御力" + Hit.DefDamPer + "%";
                if (Hit.DefRemPer != 0) Str += "防御影響" + Hit.DefRemPer + "%";
                if (ShotCounts != 1) Str += "×" + ShotCounts;
            }
            return Str;
        }
    }
    [System.Serializable]
    public class Class_Atk_Shot_Fire
    {
        [HideInInspector] public string EditDisp;
        [Tooltip(Const_Ttp_BID)] public int BranchNum;
        [Tooltip(Const_Ttp_Times)] public Vector3Int Times;
        [Tooltip("弾数")] public int Count;
        [Tooltip("弾速度x～y")] public Vector2 Speed;
        [Tooltip("発射形状")] public Class_Atk_Shot_Trans Trans;
    }
    [System.Serializable]
    public class Class_Atk_Shot_Trans
    {
        [Tooltip("座標基準")] public Enum_PosBase PosBase;
        [Tooltip("座標ズレ")] public Vector3 PosChange;
        [Tooltip("座標ブレ")] public Vector3 PosRand;
        [Tooltip("座標拡散")] public Vector3 PosWay;
        [Tooltip("座標時間変化")] public Vector3 PosTime;
        [Tooltip("角度基準")] public Enum_RotBase RotBase;
        [Tooltip("角度ズレ")] public Vector3 RotChange;
        [Tooltip("角度ブレ")] public Vector3 RotRand;
        [Tooltip("角度拡散")] public Vector3 RotWay;
        [Tooltip("角度時間変化")] public Vector3 RotTime;
    }
    [System.Serializable]
    public class Class_Atk_Shot_Hit
    {
        [HideInInspector] public string EditDisp;
        [Tooltip(Const_Ttp_BID)] public int BranchNum;
        [Tooltip("敵命中")] public bool EHit = true;
        [Tooltip("味方命中")] public bool FHit = false;
        [Tooltip("自己命中")] public bool MHit = false;
        [Tooltip("回復")] public bool Heals;
        [Tooltip("ダメージタイプ")]public Enum_DamageType DamageType;
        [Tooltip("近距離攻撃")] public bool ShortAtk;
        [Tooltip("基礎ダメージ")] public int BaseDam;
        [Tooltip("使用者最大HP依存%")] public float MHPDamPer;
        [Tooltip("使用者HP依存%")] public float HPDamPer;
        [Tooltip("使用者攻撃力依存%")] public float AtkDamPer;
        [Tooltip("使用者防御力依存%")] public float DefDamPer;
        [Tooltip("対象防御軽減%")] public float DefRemPer;
        [Tooltip("命中時SP増加量")] public int SPAdd;
        [Tooltip("状態付与")] public Class_Base_BufSet[] BufSets;
    }
    [System.Serializable]
    public class Class_Atk_Move
    {
        [Tooltip(Const_Ttp_BID)] public int BranchNum;
        [Tooltip(Const_Ttp_Times)] public Vector3Int Times;
        public Enum_RotBase Base;
        public Vector3 Vect;
        public bool SetSpeed;
    }
    [System.Serializable]
    public class Class_Atk_State
    {
        [Tooltip(Const_Ttp_BID)] public int BranchNum;
        [Tooltip(Const_Ttp_Times)] public Vector3Int Times;
        public Enum_State State;
        public float Adds;
    }
    [System.Serializable]
    public class Class_Atk_Buf
    {
        [Tooltip(Const_Ttp_BID)] public int BranchNum;
        [Tooltip(Const_Ttp_Times)] public Vector3Int Times;
        public Class_Base_BufSet[] BufSets;
    }
    [System.Serializable]
    public class Class_Atk_WeponSet
    {
        [Tooltip(Const_Ttp_BID)] public int BranchNum;
        [Tooltip(Const_Ttp_Times)] public Vector2Int Times;
        [Tooltip("武器オブジェクト")] public GameObject Obj;
        [Tooltip("表示部位")] public Enum_WeponSet Set;
        [Tooltip("位置ズレ")] public Vector3 PosChange;
        [Tooltip("角度ズレ")] public Vector3 RotChange;
    }
    [System.Serializable]
    public class Class_Atk_Anim
    {
        [HideInInspector] public string EditDisp;
        [Tooltip(Const_Ttp_BID)] public int BranchNum;
        [Tooltip(Const_Ttp_Times)] public Vector2Int Times;
        [Tooltip("アニメーションID")] public int ID;
        [Tooltip("アニメーション速度加算%")] public float Speed;
    }
    [System.Serializable]
    public class Class_Atk_SEPlay
    {
        [Tooltip(Const_Ttp_BID)] public int BranchNum;
        [Tooltip(Const_Ttp_Times)] public Vector3Int Times;
        [Tooltip("SEファイル")] public AudioClip Clip;
        [Tooltip("音量")] public float Volume = 100f;
        [Tooltip("音程-300～300"), Range(-300f, 300f)] public float Pitch = 100f;
    }

    [System.Serializable]
    public class Class_Sta_AtkCT
    {
        public int CT;
        public int CTMax;
    }
    [System.Serializable]
    public class Class_Sta_BufInfo
    {
        public int ID;
        public int Index;
        public int Time;
        public int TimeMax;
        public int Pow;
    }

    [System.Serializable]
    public class Class_Enemy_AtkAI
    {
        public Vector2Int TimeIf;
        public Class_Enemy_OtherIfs[] OtherIfs;
        public int AtkSlot;
        public Data_Atk AtkD;
    }
    [System.Serializable]
    public class Class_Enemy_OtherIfs
    {
        public Enum_OtherIfs Ifs;
        public Vector2 Val;
    }
    [System.Serializable]
    public class Class_Wave
    {
        public GameObject[] Enemys;
        public Vector3[] Pos;
    }
    [System.Serializable]
    public class Class_Shot_Move
    {
        public Vector3Int Times;
        public Enum_MoveMode MoveMode;
        public float Pow;
        public Enum_TargetMode TargetMode;
        public int TargetCT;
        [HideInInspector] public State_Base Target;
        [HideInInspector] public State_Hit TargetHit;
        [HideInInspector] public int TCT;
    }
    [System.Serializable]
    public class Class_Save_PSaves
    {
        public int PriSetID;
        public Class_Save_PSaves()
        {
            PriSetID = 0;
        }
    }
    [System.Serializable]
    public class Class_Save_PriSet
    {
        public int CharaID;
        public Class_Save_Atks AtkF;
        public Class_Save_Atks AtkB;
        public Class_Save_Passive Passive;

        public Class_Save_PriSet()
        {
            CharaID = 0;
            AtkF = new Class_Save_Atks();
            AtkB = new Class_Save_Atks();
            Passive = new Class_Save_Passive();
        }
        public Class_Save_Atks AtkGet(bool Back)
        {
            return !Back ? AtkF : AtkB;
        }
        public int PassiveLVGet(Enum_Passive Pass)
        {
            int LV = 0;
            if (Passive.P1_ID == (int)Pass) LV++;
            if (Passive.P2_ID == (int)Pass) LV++;
            if (Passive.P3_ID == (int)Pass) LV++;
            if (Passive.P4_ID == (int)Pass) LV++;
            return LV;
        }
    }
    [System.Serializable]
    public class Class_Save_Atks
    {
        public int N_AtkID;
        public int S1_AtkID;
        public int S2_AtkID;
        public int E_AtkID;
        public Class_Save_Atks()
        {
            N_AtkID = 0;
            S1_AtkID = 0;
            S2_AtkID = 1;
            E_AtkID = 0;
        }
    }
    [System.Serializable]
    public class Class_Save_Passive
    {
        public int P1_ID;
        public int P2_ID;
        public int P3_ID;
        public int P4_ID;
        public Class_Save_Passive()
        {
            P1_ID = 0;
            P2_ID = 0;
            P3_ID = 5;
            P4_ID = 6;
        }
    }
    #endregion

    #region Enum
    public enum Enum_Passive
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
        追斬,
        メイン強化,
        通常強化,
        重落強化,
        スキル強化,
        必殺強化,
        近距離強化,
        遠距離強化,
        Wシステム,
        生命の振動,
    }
    public enum Enum_Bufs
    {
        HP増加 = 0,
        攻撃増加 = 10,
        防御増加 = 20,

        毒 = 1000,
        HP再生 = 2000,
        シールド=2010,
        根性=2100,
        根性CT=2101,

        与ダメージ増加=2200,
        近距離強化=2201,
        遠距離強化=2202,
    }
    public enum Enum_BufSet
    {
        付与,
        付与増加,
        不付与増加,
        切り替え,
        消去,
    }
    public enum Enum_OtherIfs
    {
        無 = -1,
        HP割合_x以下 = 0,
        HP割合_x以上,
        ターゲット距離_x以下 = 10,
        ターゲット距離_x以上,
    }
    public enum Enum_MoveMode
    {
        重力落下 = -2,
        速度向き = -1,
        加速 = 0,
        速度変化 = 1,
        直線補間ホーミング = 10,
        曲線補間ホーミング = 11,
        瞬間移動 = 12,
    }
    public enum Enum_TargetMode
    {
        ターゲット,
        近敵ターゲット優先,
        近敵,
        自身 = 10,
        味方 = 20,
    }
    public enum Enum_AtkIf
    {
        攻撃単入力 = 0,
        攻撃長入力,
        攻撃未入力,
        攻撃未長入力,
        地上 = 10,
        空中 = 11,
        MP有り = 20,
        MP無し = 21,
    }
    public enum Enum_PosBase
    {
        使用者位置,
        ターゲット位置,
    }
    public enum Enum_RotBase
    {
        固定 = -1,
        使用者向き,
        ターゲット方向,
        使用者カメラ方向,
    }
    public enum Enum_State
    {
        HP,
        MP,
        SP,
    }
    public enum Enum_WeponSet
    {
        基点 = -1,
        右手 = 0,
        左手 = 1,
        右足 = 10,
        左足 = 11,
        胴体 = 20,
        頭 = 30,
    }
    public enum Enum_PassiveAtk
    {
        タルタル,
        追斬,
        Wシステム,
        生命の振動,
    }
    public enum Enum_AtkType
    {
        通常,
        スキル,
        必殺,
    }
    public enum Enum_DamageType
    {
        通常,
        重撃,
        落下,
        スキル,
        必殺,
        パッシブ,
    }
    public enum Enum_AtkFilter
    {
        攻撃,
        移動,
        バフ,
        デバフ,
        回復,
        召喚,
        特殊,
        近距離=100,
        遠距離,
        照準,
        自己 =110,
        味方=111,
        複数=200,
        多段=201,
        高頻度=202,
        追加攻撃=203,
        攻撃強化=300,
        防御強化=301,
    }
    public enum Enum_PassiveFilter
    {
        基礎ステータス,
        攻撃強化,
        防御強化,
        回復,
        追撃=10,
        条件,
        メイン=20,
        スキル,
        必殺,
    }
    #endregion
}
