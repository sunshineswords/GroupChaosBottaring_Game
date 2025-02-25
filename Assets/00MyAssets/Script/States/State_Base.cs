using Photon.Pun;
using UnityEngine;
using static DataBase;
using static Statics;
using static BattleManager;
using System.Collections.Generic;
using System.Linq;
using static PlayerValue;
using static Manifesto;
using static Calculation;
using NaughtyAttributes;
public class State_Base : MonoBehaviourPun,IPunObservable
{
    #region インスペクター変数
    [Foldout("設定")]public string Name;
    [Foldout("設定")] public Rigidbody Rig;
    [Foldout("設定")] public Transform CamTrans;
    [Foldout("設定")] public float Hight;
    [Foldout("設定")] public float SizeAdd;
    [Foldout("設定"),Tooltip("チーム")] public int Team;
    [Foldout("設定"), Tooltip("プレイヤー")] public bool Player;
    [Foldout("設定"), Tooltip("ボス")] public bool Boss;
    [Foldout("設定"), Tooltip("不死")] public bool Undet;
    [Foldout("設定")] public Class_Base_SEPlay DamageSE;
    [Foldout("設定")] public Class_Base_SEPlay DeathSE;
    [Foldout("設定"), Tooltip("死亡エフェクト")] public GameObject DeathEffect;
    //
    [Foldout("ステータス"), Tooltip("最大HP")]public int MHP;
    [Foldout("ステータス"), Tooltip("秒間HP回復速度")] public float HPRegene;
    [Foldout("ステータス"), Tooltip("最大MP(移動力)")] public int MMP;
    [Foldout("ステータス"), Tooltip("秒間MP回復速度")] public float MPRegene;
    [Foldout("ステータス"), Tooltip("秒間SP回復速度")] public float SPRegene;
    [Foldout("ステータス"), Tooltip("攻撃力")] public int Atk;
    [Foldout("ステータス"), Tooltip("防御力")] public int Def;
    //
    [Foldout("数値")]public float HP;
    [Foldout("数値")] public float MP;
    [Foldout("数値")] public float SP;
    [Foldout("数値")] public bool Ground;
    [Foldout("数値")] public int DeathTime;
    [Foldout("数値")] public State_Base Target;
    [Foldout("数値")] public State_Hit TargetHit;
    [Foldout("数値")] public List<Class_Sta_BufInfo> Bufs = new List<Class_Sta_BufInfo>();
    //

    [Foldout("変数")] public bool LowMP;
    [Foldout("変数")] public int DashTime;
    [Foldout("変数")] public bool GroundB;

    [Foldout("変数")] public Data_Atk AtkD;
    [Foldout("変数")] public int AtkSlot;
    [Foldout("変数")] public int AtkTime;
    [Foldout("変数")] public int AtkBranch;

    [Foldout("変数")] public float SpeedRem;
    [Foldout("変数")] public bool NoJump;
    [Foldout("変数")] public bool NoDash;
    [Foldout("変数")] public bool Aiming;
    [Foldout("変数")] public bool NGravity;

    [Foldout("変数")] public int Anim_MoveID;
    [Foldout("変数")] public int Anim_AtkID;
    [Foldout("変数")] public float Anim_AtkSpeed;
    [Foldout("変数")] public int Anim_OtherID;

    #endregion
    #region 内部変数
    public Dictionary<int,int> WeponSets = new Dictionary<int, int>();
    public Dictionary<int, Vector3> WeponPoss = new Dictionary<int, Vector3>();
    public Dictionary<int, Vector3> WeponRots = new Dictionary<int, Vector3>();
    public Dictionary<int,Class_Sta_AtkCT> AtkCTs = new Dictionary<int,Class_Sta_AtkCT>();
    public Dictionary<int,GameObject> BufEffects = new Dictionary<int,GameObject>();
    public Dictionary<int, int> LocalCTs = new Dictionary<int, int>();
    [System.NonSerialized] public int AddTimer = 0;
    [System.NonSerialized] public int[] AddDams = new int[10];
    [System.NonSerialized] public float AddDamTotal = 0;
    [System.NonSerialized] public int[] AddHits = new int[10];
    [System.NonSerialized] public float AddHitTotal = 0;

    public int FMHP
    {
        get
        {
            float FVal = MHP;
            FVal *= 1f + BufPowGet(Enum_Bufs.HP増加) * 0.01f;
            return Mathf.RoundToInt(FVal);
        }
    }
    public int FMMP
    {
        get
        {
            float FVal = MMP;
            return Mathf.RoundToInt(FVal);
        }
    }
    public int FAtk
    {
        get
        {
            float FVal = Atk;
            FVal *= 1f + BufPowGet(Enum_Bufs.攻撃増加) * 0.01f;
            return Mathf.RoundToInt(FVal);
        }
    }
    public int FDef
    {
        get
        {
            float FVal = Def;
            FVal *= 1f + BufPowGet(Enum_Bufs.防御増加) * 0.01f;
            return Mathf.RoundToInt(FVal);
        }
    }
    #endregion
    private void Start()
    {
        if (!photonView.IsMine) return;
        if (Player)
        {
            MHP = Mathf.RoundToInt(MHP * (1f + PriSetGet.PassiveLVGet(Enum_Passive.HP増加) * 0.25f));
            HPRegene = Mathf.RoundToInt(HPRegene * (1f + PriSetGet.PassiveLVGet(Enum_Passive.自然再生) * 0.5f));
            MMP = Mathf.RoundToInt(MMP * (1f + PriSetGet.PassiveLVGet(Enum_Passive.MP増加) * 0.1f));
            MPRegene = Mathf.RoundToInt(MPRegene * (1f + PriSetGet.PassiveLVGet(Enum_Passive.気力増幅) * 0.05f));
            SPRegene = Mathf.RoundToInt(SPRegene * (1f + PriSetGet.PassiveLVGet(Enum_Passive.SPブースト) * 0.2f));
            Atk = Mathf.RoundToInt(Atk * (1f + PriSetGet.PassiveLVGet(Enum_Passive.攻撃力増加) * 0.1f));
            Def = Mathf.RoundToInt(Def * (1f + PriSetGet.PassiveLVGet(Enum_Passive.防御力増加) * 0.1f));
        }
        if (Team != 0)
        {
            MHP = Mathf.RoundToInt(MHP * BTManager.EStaMults);
            HPRegene = Mathf.RoundToInt(HPRegene * BTManager.EStaMults);
        }
        HP = FMHP;
        MP = FMMP;
        if (!PhotonNetwork.OfflineMode)
        {
            if (Player) Name = photonView.Owner.NickName;
        }
        if (CamTrans == null) CamTrans = transform;
    }
    private void FixedUpdate()
    {
        Anim_OtherID = 0;
        if (HP <= 0) Anim_OtherID = 2;
        BufEffectSet();
        if (!photonView.IsMine) return;

        Ground = GroundB;
        GroundB = false;
        if (MP <= 0)
        {
            MP = 0;
            LowMP = true;
        }
        if(Ground)MP += MPRegene / 60f;
        MP = Mathf.Min(MP, FMMP);
        if (LowMP && MP >= FMMP * 0.2f) LowMP = false;
        if (HP <= 0)
        {
            StateDeaths();
        }
        else
        {
            LivesTimes();
        }
        if (!Player && Team != 0 && BTManager.End) Deletes();
        DashTime--;
        BufTimeRems();
        LocalCTRems();
        AtkPlays(CamTrans.eulerAngles);
        if (Rig) Rig.useGravity = !NGravity;
        if (Player) AddInfoChange();
    }
    #region 内部メソッド
    void Deletes()
    {
        photonView.RPC(nameof(RPC_DeathEffect), RpcTarget.All);
        PhotonNetwork.Destroy(gameObject);
    }
    void AtkPlays(Vector3 CamRot)
    {
        #region CT減少
        var CTKeys = AtkCTs.Keys.ToArray();
        for (int i = 0; i < CTKeys.Length; i++)
        {
            AtkCTs[CTKeys[i]].CT--;
            if (AtkCTs[CTKeys[i]].CT <= 0) AtkCTs.Remove(CTKeys[i]);
        }
        #endregion
        var WeponKeys = WeponSets.Keys.ToArray();
        for (int i = 0; i < WeponKeys.Length; i++)
        {
            WeponSets[WeponKeys[i]] = -1;
        }
        SpeedRem = 0;
        NoJump = false;
        NoDash = false;
        Aiming = false;
        NGravity = false;
        #region スキル処理
        if (HP <= 0) AtkD = null;
        Anim_AtkID = 0;
        Anim_AtkSpeed = 1;
        if (AtkD == null)
        {
            AtkTime = 0;
            return;
        }
        State_Atk.Fixed(this);
        State_Atk.Shot(this, PosGet(), RotGet(), CamRot);
        State_Atk.Move(this, CamRot);
        State_Atk.State(this);
        State_Atk.Buf(this);
        State_Atk.WeponSet(this);
        State_Atk.Anim(this);
        State_Atk.SEPlay(this);
        AtkTime++;
        if (AtkTime > AtkD.EndTime) AtkD = null;
        #endregion
    }
    void StateDeaths()
    {
        var GutsLV = BufPowGet(Enum_Bufs.根性);
        if (GutsLV <= 0)
        {
            if (Player)
            {
                if (DeathTime == 0)
                {
                    BTManager.SEPlay(DeathSE.Clip, PosGet(), DeathSE.Volume, DeathSE.Pitch);
                    BTManager.DeathAdd();
                    BTManager.MessageAdd("<color=#FF0000>" + Name + "</color>\\<color=#FF0000>は倒れた!!!</color>");
                    var DeathPowLV = PriSetGet.PassiveLVGet(Enum_Passive.死に力);
                    if (DeathPowLV > 0)
                    {
                        BufSets(Enum_Bufs.攻撃増加, -1000, Enum_BufSet.付与増加, 0, DeathPowLV * 5);
                    }
                }
                if (DeathTime >= 300) HP = FMHP;
            }
            else if (Undet)
            {
                if (DeathTime >= 300) HP = FMHP;
            }
            else if (!Boss)
            {
                if (DeathTime >= 60)
                {
                    BTManager.SEPlay(DeathSE.Clip, PosGet(), DeathSE.Volume, DeathSE.Pitch);
                    Deletes();
                }
            }
            else
            {
                if (DeathTime == 0) BTManager.SEPlay(DeathSE.Clip, PosGet(), DeathSE.Volume, DeathSE.Pitch);
            }
        }
        else
        {
            BufPowRem(Enum_Bufs.根性, 1);
            HP = 1;
        }
        DeathTime++;
        DashTime = 0;
    }
    void LivesTimes()
    {
        HP += HPRegene / 60f;
        HP -= BufPowGet(Enum_Bufs.毒) / 60f;
        HP = Mathf.Clamp(HP, 1, FMHP);
        SP += SPRegene / 60f;
        DeathTime = 0;
        if (Player)
        {
            var GutLV = PriSetGet.PassiveLVGet(Enum_Passive.根性);
            if (GutLV > 0 && !BufCheck(Enum_Bufs.根性CT))
            {
                BufSets(Enum_Bufs.根性, -1000, Enum_BufSet.付与, 0, GutLV);
                BufSets(Enum_Bufs.根性CT, -1000, Enum_BufSet.付与, 60 * 15, 0);
            }
        }
    }
    void BufTimeRems()
    {
        for (int i = Bufs.Count - 1; i >= 0; i--)
        {
            var Bufi = Bufs[i];
            if (Bufi.TimeMax <= 0) continue;
            Bufi.Time--;
            if (Bufi.Time <= 0) Bufs.RemoveAt(i);
        }
    }
    void LocalCTRems()
    {
        var LocalKeys = LocalCTs.Keys.ToArray();
        for (int i = 0; i < LocalKeys.Length; i++)
        {
            LocalCTs[LocalKeys[i]]--;
            if (LocalCTs[LocalKeys[i]] <= 0) LocalCTs.Remove(LocalKeys[i]);
        }
    }
    void BufEffectSet()
    {
        var BufKeys = BufEffects.Keys.ToArray();
        for (int i = 0; i < BufKeys.Length; i++)
        {
            var BufGet = Bufs.Find(x => x.ID == BufKeys[i]);
            if (BufGet == null)
            {
                Destroy(BufEffects[BufKeys[i]]);
                BufEffects.Remove(BufKeys[i]);
            }
        }
        for (int i = 0; i < Bufs.Count; i++)
        {
            var Bufi = Bufs[i];
            var BufD = DB.Bufs.Find(x => (int)x.Buf == Bufi.ID);
            if (BufD != null && !BufEffects.ContainsKey(Bufi.ID))
            {
                var EffectIns = Instantiate(BufD.EffectObj, PosGet(), Quaternion.identity);
                EffectIns.transform.localScale = Vector3.one * (1f + SizeAdd * 0.01f);
                EffectIns.transform.parent = Rig != null ? Rig.transform : transform;
                BufEffects.Add(Bufi.ID, EffectIns);
            }
        }
    }
    void BufPowRem(Enum_Bufs BufID, int Val)
    {
        for (int i = Bufs.Count - 1; i >= 0; i--)
        {
            var Bufi = Bufs[i];
            if (Bufi.ID == (int)BufID)
            {
                var Vald = Val;
                Val -= Bufi.Pow;
                Bufi.Pow -= Vald;
                if (Bufi.Pow <= 0) Bufs.Remove(Bufi);
            }
            if (Val <= 0) return;
        }
    }

    void AddInfoChange()
    {
        AddTimer++;
        if(AddTimer >= 60)
        {
            AddTimer = 0;
            for (int i = AddDams.Length - 1; i > 0; i--)
            {
                AddDams[i] = AddDams[i - 1];
                AddHits[i] = AddHits[i - 1];
            }
            AddDams[0] = 0;
            AddHits[0] = 0;
        }
    }
    #endregion
    #region 呼び出しメソッド
    public Vector3 PosGet()
    {
        if (Rig != null) return Rig.position + Vector3.up * Hight;
        else return transform.position + Vector3.up * Hight;
    }
    public Vector3 RotGet()
    {
        if (Rig != null) return Rig.transform.eulerAngles;
        else return transform.eulerAngles;
    }
    public void Damage(Vector3 HitPos, int Val)
    {
        photonView.RPC(nameof(RPC_Damage), RpcTarget.All, HitPos, Val);
    }
    public void TargetSet()
    {
        float NearDis = -1;
        Target = null;
        foreach (var Sta in BTManager.StateList)
        {
            if (!TeamCheck(this, Sta)) continue;
            if (Sta.HP <= 0) continue;
            float Dis = Vector3.Distance(PosGet(), Sta.PosGet());
            if (NearDis < 0 || NearDis > Dis)
            {
                NearDis = Dis;
                Target = Sta;
            }
        }
    }
    public void AtkInput(int UseAtkSlot, Data_Atk UseAtkD, bool Enter, bool Stay)
    {
        if (HP <= 0) return;
        if (AtkD == null && Enter)
        {
            if (AtkCTs.ContainsKey(UseAtkSlot)) return;
            if (SP < UseAtkD.SPUse) return;
            if (UseAtkD.SPUse > 0) SP = 0;
            int CTs = Mathf.RoundToInt(UseAtkD.CT * 60);
            if (Player)
            {
                CTs = Mathf.RoundToInt(CTs * (1f - PriSetGet.PassiveLVGet(Enum_Passive.CTカット) * 0.10f));
                if(UseAtkD.AtkType == Enum_AtkType.必殺)
                {
                    var SpHealLV = PriSetGet.PassiveLVGet(Enum_Passive.必殺再生);
                    if (SpHealLV > 0) Damage(PosGet(), Mathf.RoundToInt(MHP * SpHealLV * 0.15f));
                    var SpReturnLV = PriSetGet.PassiveLVGet(Enum_Passive.必殺返還);
                    if (SpReturnLV > 0) SP = SpReturnLV * 15;
                }
            }
            if (UseAtkD.AtkType == Enum_AtkType.必殺) BTManager.MessageAdd(Name + "\\の必殺「" + UseAtkD.Name + "」");
            AtkCTs.Add(UseAtkSlot, new Class_Sta_AtkCT { CT = CTs, CTMax = CTs });

            AtkD = UseAtkD;
            AtkSlot = UseAtkSlot;
            AtkTime = 0;
            AtkBranch = 0;
            return;
        }
        if (AtkD != null && UseAtkSlot == AtkSlot)
        {
            State_Atk.Branch(this, Enter, Stay);
        }

    }
    public void BufSets(Class_Base_BufSet BufSet,State_Base AddSta)
    {
        var PowVal = Mathf.RoundToInt((float)Cal(BufSet.PowVal, AddSta, this));
        var PowMax = Mathf.RoundToInt((float)Cal(BufSet.PowMax, AddSta, this));
        BufSets(BufSet.Buf, BufSet.Index,BufSet.Set, BufSet.TimeVal, PowVal, BufSet.TimeMax, PowMax);
    }
    public void BufSets(Enum_Bufs BufID, int Index, Enum_BufSet Sets, int Time, int Pow, int TMax=0, int PMax=0)
    {
        photonView.RPC(nameof(RPC_BufSet), RpcTarget.All, (int)BufID, Index, (int)Sets, Time, Pow, TMax, PMax);
    }
    public int BufPowGet(Enum_Bufs BufID)
    {
        int Pow = 0;
        for(int i = 0; i < Bufs.Count; i++)
        {
            if (Bufs[i].ID == (int)BufID) Pow += Bufs[i].Pow;
        }
        return Pow;
    }
    public bool BufCheck(Enum_Bufs BufID)
    {
        for (int i = 0; i < Bufs.Count; i++)
        {
            if (Bufs[i].ID == (int)BufID) return true;
        }
        return false;
    }
    public float BufTPMultGet(Enum_Bufs BufID)
    {
        float Val = 0;
        for (int i = 0; i < Bufs.Count; i++)
        {
            if (Bufs[i].ID == (int)BufID) Val += Bufs[i].Time * Bufs[i].Pow;
        }
        return Val;
    }
    public void HitEvents(State_Base HitSta,Vector3 Pos, Enum_DamageType DamType, bool ShortAtk)
    {
        if (Player)
        {
            var TaruLV = PriSetGet.PassiveLVGet(Enum_Passive.タルタル);
            if (TaruLV > 0 && !HitSta.LocalCTs.ContainsKey((int)Enum_PassiveAtk.タルタル))
            {
                HitSta.LocalCTs.Add((int)Enum_PassiveAtk.タルタル, 60 * 3);
                State_Atk.ShotAdd(this, TaruLV, DB.PassiveAtks[(int)Enum_PassiveAtk.タルタル], 0, Pos, Vector3.zero);
            }
            var AddShashLV = PriSetGet.PassiveLVGet(Enum_Passive.追斬);
            if (AddShashLV > 0 && !LocalCTs.ContainsKey((int)Enum_PassiveAtk.追斬))
            {
                LocalCTs.Add((int)Enum_PassiveAtk.追斬, 60 * 4);
                State_Atk.ShotAdd(this, AddShashLV, DB.PassiveAtks[(int)Enum_PassiveAtk.追斬], 0, Pos, Vector3.zero);
            }
            var WSystemLV = PriSetGet.PassiveLVGet(Enum_Passive.Wシステム);
            if(WSystemLV > 0 && !LocalCTs.ContainsKey((int)Enum_PassiveAtk.Wシステム))
            {
                LocalCTs.Add((int)Enum_PassiveAtk.Wシステム, 60 * 1);
                int Timed = 60 * 6;
                if (ShortAtk) BufSets(Enum_Bufs.遠距離強化, -1000, Enum_BufSet.付与増加, Timed, 2 * WSystemLV, Timed, 40);
                else BufSets(Enum_Bufs.近距離強化, -1000, Enum_BufSet.付与増加, Timed, 3 * WSystemLV, Timed, 60);
            }
        }
    }
    public void AddInfoAdd(int Dam)
    {
        AddDamTotal += Dam;
        AddDams[0]+=Dam;
        AddHitTotal++;
        AddHits[0]++;
    }
    public void AddInfoReset()
    {
        AddTimer = 0;
        for(int i = 0; i < AddDams.Length; i++)
        {
            AddDams[i] = 0;
            AddHits[i] = 0;
        }
        AddDamTotal = 0;
        AddHitTotal = 0;
    }
    #endregion
    #region RPCメソッド
    [PunRPC]
    void RPC_Damage(Vector3 HitPos, int Val)
    {
        if (HP <= 0) return;
        if (Val < 0)
        {
            int Healm = FMHP - (int)HP;
            Val = -Mathf.Min(-Val, Healm);
        }
        else
        {
            int Barria = BufPowGet(Enum_Bufs.バリア);
            if (Barria > 0)
            {
                Val /= 10;
                if (photonView.IsMine)
                {
                    BufPowRem(Enum_Bufs.バリア, 1);
                }
            }
            int Shilds = BufPowGet(Enum_Bufs.シールド);
            if (Shilds > 0)
            {
                int Vald = Val;
                Val = Mathf.Max(0, Val - Shilds);
                if (photonView.IsMine)
                {
                    BufPowRem(Enum_Bufs.シールド, Vald);
                }
            }
        }
        if (Val == 0) return;
        if (Player)
        {
            var LifeVibrationLV = PriSetGet.PassiveLVGet(Enum_Passive.生命の振動);
            if (LifeVibrationLV > 0 && !LocalCTs.ContainsKey((int)Enum_PassiveAtk.生命の振動))
            {
                LocalCTs.Add((int)Enum_PassiveAtk.生命の振動, 60 * 1);
                int Timed = 60 * 6;
                BufSets(Enum_Bufs.与ダメージ増加, -1000, Enum_BufSet.付与増加, Timed, 3 * LifeVibrationLV, Timed, 36);
            }
        }
        Color DamCol = Val >= 0 ? Color.white : Color.magenta;
        GameObject HitEffect = Val >= 0 ? DB.HitEffects[1] : DB.HealEffects[1];
        switch (Team)
        {
            case 0:
                DamCol = Val >= 0 ? Color.red : Color.green;
                HitEffect = Val >= 0 ? DB.HitEffects[0] : DB.HealEffects[0];
                break;
        }
        DamageObj.DamageSet(HitPos, Mathf.Abs(Val), DamCol);
        var InsHitEffect = Instantiate(HitEffect, HitPos, Quaternion.identity);
        if (Val >= 0)BTManager.SEPlay(DamageSE.Clip, HitPos, DamageSE.Volume, DamageSE.Pitch, true);

        if (!photonView.IsMine) return;
        HP -= Val;
    }
    [PunRPC]
    void RPC_DeathEffect()
    {
        if (DeathEffect != null) Instantiate(DeathEffect, PosGet(), Quaternion.identity);
    }
    [PunRPC]
    void RPC_BufSet(int BufID, int Index, int Sets, int Time, int Pow, int TMax, int PMax)
    {
        if (!photonView.IsMine) return;
        Class_Sta_BufInfo Bufi = null;
        for (int i = 0; i < Bufs.Count; i++)
        {
            var Bufd = Bufs[i];
            if(Bufd.ID == BufID && Bufd.Index == Index)
            {
                Bufi = Bufd;
                break;
            }
        }
        if (Sets != (int)Enum_BufSet.消去)
        {
            if (Bufi != null && Sets == (int)Enum_BufSet.切り替え) Bufs.Remove(Bufi);
            else
            {
                if (Bufi == null && Sets == (int)Enum_BufSet.不付与増加) return;
                if (Bufi == null)
                {
                    Bufi = new Class_Sta_BufInfo { ID = BufID, Index = Index, Time = 0, Pow = 0, TimeMax = 1 };
                    Bufs.Add(Bufi);
                }
                if (Sets == (int)Enum_BufSet.付与 || Sets == (int)Enum_BufSet.切り替え)
                {
                    Bufi.Time = Mathf.Max(Bufi.Time, Time);
                    Bufi.Pow = Mathf.Max(Bufi.Pow, Pow);
                }
                else
                {
                    if (TMax > 0) Bufi.Time = Mathf.Min(Bufi.Time + Time, TMax);
                    else Bufi.Time += Time;
                    if (PMax > 0) Bufi.Pow = Mathf.Min(Bufi.Pow + Pow, PMax);
                    else Bufi.Pow += Pow;
                }
                if (Bufi.Time <= 0) Bufi.TimeMax = 0;
                if (Bufi.TimeMax > 0) Bufi.TimeMax = Mathf.Max(Bufi.Time, Bufi.TimeMax);
            }
        }
        else if (Bufi != null) Bufs.Remove(Bufi);
    }
    #endregion
    void IPunObservable.OnPhotonSerializeView(Photon.Pun.PhotonStream stream, Photon.Pun.PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(MHP);
            stream.SendNext(MMP);
            stream.SendNext(Atk);
            stream.SendNext(Def);

            stream.SendNext(Team);

            stream.SendNext(HP);

            var WepSetKeys = WeponSets.Keys.ToArray();
            var WepSetIDs = WeponSets.Values.ToArray();
            var WepSetPoss = WeponPoss.Values.ToArray();
            var WepSetRots = WeponRots.Values.ToArray();
            stream.SendNext(WepSetKeys);
            stream.SendNext(WepSetIDs);
            stream.SendNext(WepSetPoss);
            stream.SendNext(WepSetRots);

            stream.SendNext(Anim_MoveID);
            stream.SendNext(Anim_AtkID);
            stream.SendNext(Anim_AtkSpeed);
            stream.SendNext(Anim_OtherID);

            var Buf_ID = new List<int>();
            var Buf_Index = new List<int>();
            var Buf_Time = new List<int>();
            var Buf_Pow = new List<int>();
            var Buf_TimeMax = new List<int>();
            for (int i = 0; i < Bufs.Count; i++)
            {
                Buf_ID.Add(Bufs[i].ID);
                Buf_Index.Add(Bufs[i].Index);
                Buf_Time.Add(Bufs[i].Time);
                Buf_Pow.Add(Bufs[i].Pow);
                Buf_TimeMax.Add(Bufs[i].TimeMax);
            }
            stream.SendNext(Buf_ID.ToArray());
            stream.SendNext(Buf_Index.ToArray());
            stream.SendNext(Buf_Time.ToArray());
            stream.SendNext(Buf_Pow.ToArray());
            stream.SendNext(Buf_TimeMax.ToArray());
        }
        else
        {
            MHP = (int)stream.ReceiveNext();
            MMP = (int)stream.ReceiveNext();
            Atk = (int)stream.ReceiveNext();
            Def = (int)stream.ReceiveNext();
            Team = (int)stream.ReceiveNext();

            HP = (float)stream.ReceiveNext();

            var WepSetKeys = (int[])stream.ReceiveNext();
            var WepSetIDs = (int[])stream.ReceiveNext();
            var WepSetPoss = (Vector3[])stream.ReceiveNext();
            var WepSetRots = (Vector3[])stream.ReceiveNext();
            WeponSets.Clear();
            WeponPoss.Clear();
            WeponRots.Clear();
            for (int i = 0; i < WepSetKeys.Length; i++)
            {
                WeponSets.Add(WepSetKeys[i], WepSetIDs[i]);
                WeponPoss.Add(WepSetKeys[i], WepSetPoss[i]);
                WeponRots.Add(WepSetKeys[i], WepSetRots[i]);
            }

            Anim_MoveID = (int)stream.ReceiveNext();
            Anim_AtkID = (int)stream.ReceiveNext();
            Anim_AtkSpeed = (float)stream.ReceiveNext();
            Anim_OtherID = (int)stream.ReceiveNext();

            var Buf_ID = (int[])stream.ReceiveNext();
            var Buf_Index = (int[])stream.ReceiveNext();
            var Buf_Time = (int[])stream.ReceiveNext();
            var Buf_Pow = (int[])stream.ReceiveNext();
            var Buf_TimeMax = (int[])stream.ReceiveNext();
            Bufs.Clear();
            for(int i = 0; i < Buf_ID.Length; i++)
            {
                Bufs.Add(new Class_Sta_BufInfo
                {
                    ID = Buf_ID[i],
                    Index = Buf_Index[i],
                    Time = Buf_Time[i],
                    Pow = Buf_Pow[i],
                    TimeMax = Buf_TimeMax[i],
                });
            }
        }
    }
}
