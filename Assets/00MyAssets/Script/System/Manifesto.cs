using UnityEngine;
using static Manifesto;
using static Calculation;
using NaughtyAttributes;
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
        [HideInInspector] public string EditDisp;
        public Enum_Bufs Buf;
        [Tooltip("状態番号")] public int Index;
        [Tooltip("付与処理")] public Enum_BufSet Set;
        [Tooltip("時間付与フレーム\n0以下だと永続")] public int TimeVal;
        [Tooltip("段階付与式\n0以下だと段階表示なし\n" + TooltipStr), TextArea(1, 3)] public string PowVal;
        [Tooltip("時間上限フレーム\n0以下は上限無し")] public int TimeMax;
        [Tooltip("段階上限上限式\n0以下は上限無し\n" + TooltipStr), TextArea(1, 3)] public string PowMax;
        public string InfoStr()
        {
            var OStr = Buf.ToString() + Set.ToString();
            bool Adds = (Set == Enum_BufSet.付与増加 || Set == Enum_BufSet.不付与増加);
            OStr += "時間:" + (TimeVal / 60f).ToString("F1");
            if (Adds && TimeMax > 0) OStr += "Max" + (TimeMax / 60f).ToString("F1");
            OStr += "秒";
            var PowStr = CalStr(PowVal);
            var PowVald = double.TryParse(PowStr, out var oPVal) ? oPVal : 1;
            if (PowStr != "" && PowVald > 0)
            {
                OStr += ",段階:" + CalStr(PowVal);
                if (Adds)
                {
                    var MaxStr = CalStr(PowMax);
                    var MaxVal = double.TryParse(MaxStr, out var oVal) ? oVal : 1;
                    if (MaxStr != "" && MaxVal > 0) OStr += "Max" + MaxStr;
                }
            }
            return OStr;
        }
        public void EditDispSet()
        {
            EditDisp = Buf.ToString();
            EditDisp += "[" + Index;
            EditDisp += "," + Set.ToString() +"]";
            var Adds = Set == Enum_BufSet.付与増加 || Set == Enum_BufSet.不付与増加;
            if (TimeVal > 0)
            {
                EditDisp += "(時間:付与" + TimeVal;
                if(Adds && TimeMax > 0) EditDisp += "Max" + TimeMax;
                EditDisp += ")";
            }
            else
            {
                EditDisp += "(時間無限)";
            }
            var PowStr = CalStr(PowVal);
            var PowVald = double.TryParse(PowStr, out var oPVal) ? oPVal : 1;
            if (PowStr != "" && PowVald > 0)
            {
                EditDisp += "(段階:付与" + CalStr(PowVal);
                if (Adds)
                {
                    var MaxStr = CalStr(PowMax);
                    var MaxVal = double.TryParse(MaxStr, out var oVal) ? oVal : 1;
                    if (MaxStr != "" && MaxVal > 0) EditDisp += "Max" + MaxStr;
                }
                EditDisp += ")";
            }
        }
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
                if (Hit.DamCalc != "") Hit.EditDisp += CalStr(Hit.DamCalc);
                if(Hit.BufSets!=null)
                for (int j = 0; j < Hit.BufSets.Length; j++) Hit.BufSets[j].EditDispSet();
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
                Str += "(";
                if (Hit.EHit) Str += "敵";
                if (Hit.FHit) Str += "味方";
                if (Hit.MHit) Str += "自身";
                Str += ",";
                Str += Hit.Heals ? "回復" : "攻撃";
                Str += ")";
                if (Hit.DamCalc != "") Str += CalStr(Hit.DamCalc);
                if (ShotCounts != 1) Str += "×" + ShotCounts;
                for (int j = 0; j < Hit.BufSets.Length; j++) Str += Hit.BufSets[j].InfoStr();
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
        [Tooltip("発射形状")] public Class_Atk_Shot_TransBase Trans;
    }
    [System.Serializable]
    public class Class_Atk_Shot_TransBase
    {
        [Tooltip("位置基準")] public Enum_PosBase PosBase;
        [Tooltip("位置変化")] public Class_Atk_Shot_TransPos[] TransPoss;
        [Tooltip("角度基準")] public Enum_RotBase RotBase;
        [Tooltip("角度変化")] public Class_Atk_Shot_TransRot[] TransRots;
    }
    [System.Serializable]
    public class Class_Atk_Shot_TransPos
    {
        [Tooltip("変化方法")] public Enum_TransChange Change;
        [Tooltip("変化補正")] public float Mlt;
        [Tooltip("変化値")] public Vector3 Val;
    }
    [System.Serializable]
    public class Class_Atk_Shot_TransRot
    {
        [Tooltip("変化方法")] public Enum_TransChange Change;
        [Tooltip("変化補正")] public float Mlt;
        [Tooltip("変化値")] public Vector3 Val;
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
        [Tooltip("ダメージ式\n"+TooltipStr),TextArea(1,3)] public string DamCalc;
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
        [Tooltip("増値式\n"+TooltipStr), TextArea(1, 3)] public string Adds;
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
        public float MasterVol = 1;
        public float BGMVol = 1;
        public float SEVol = 1;
        public float SystemVol = 1;
        public int QualityLV = 4;
        public int PriSetID;
        public Class_Save_PSaves()
        {
            MasterVol = 1;
            BGMVol = 1;
            SEVol = 1;
            SystemVol = 1;
            QualityLV = 4;
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
        バリア = 2011,
        根性 =2100,
        根性CT=2101,
        与ダメージ増加 =2200,
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
    public enum Enum_TransChange
    {
        ズレ,
        ブレ,
        拡散_掛け,
        拡散_Sin,
        拡散_Cos,
        時間_掛け,
        時間_Sin,
        時間_Cos,
    }
    public enum Enum_State
    {
        回復,
        ダメージ,
        MP増加,
        MP減少,
        SP増加,
        SP減少,
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
    public enum Enum_SetSlot
    {
        キャラ,
        表通常,
        表スキル1,
        表スキル2,
        表必殺,
        裏通常,
        裏スキル1,
        裏スキル2,
        裏必殺,
        パッシブ1,
        パッシブ2,
        パッシブ3,
        パッシブ4,
    }
    #endregion
}
