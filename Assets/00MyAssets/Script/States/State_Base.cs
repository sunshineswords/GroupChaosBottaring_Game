using Photon.Pun;
using UnityEngine;
using static DataBase;
using static Statics;
using static BattleManager;
using System.Collections.Generic;
using System.Linq;
using static PlayerValue;
using static Manifesto;
public class State_Base : MonoBehaviourPun,IPunObservable
{
    [Header("設定")]
    public string Name;
    public Rigidbody Rig;
    public Transform CamTrans;
    public float Hight;
    [Tooltip("チーム")] public int Team;
    [Tooltip("プレイヤー")] public bool Player;
    [Tooltip("ボス")] public bool Boss;
    [Tooltip("不死")] public bool Undet;
    public Class_Base_SEPlay DamageSE;
    public Class_Base_SEPlay DeathSE;
    [Tooltip("死亡エフェクト")] public GameObject DeathEffect;
    [Header("ステータス")]
    [Tooltip("最大HP")]public int MHP;
    [Tooltip("秒間HP回復速度")] public float HPRegene;
    [Tooltip("最大MP(移動力)")] public int MMP;
    [Tooltip("秒間MP回復速度")] public float MPRegene;
    [Tooltip("秒間SP回復速度")] public float SPRegene;
    [Tooltip("攻撃力")] public int Atk;
    [Tooltip("防御力")] public int Def;
    [Header("変数")]
    public float HP;
    public float MP;
    public float SP;
    public int DeathTime;

    public bool LowMP;

    public int DashTime;
    public bool GroundB;
    public bool Ground;

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

    public State_Base Target;
    public State_Hit TargetHit;

    public Data_Atk AtkD;
    public int AtkSlot;
    public int AtkTime;
    public int AtkBranch;

    public float SpeedRem;
    public bool NoJump;
    public bool NoDash;
    public bool Aiming;
    public bool NGravity;

    public Dictionary<int,int> WeponSets = new Dictionary<int, int>();
    public Dictionary<int, Vector3> WeponPoss = new Dictionary<int, Vector3>();
    public Dictionary<int, Vector3> WeponRots = new Dictionary<int, Vector3>();

    public int Anim_MoveID;
    public int Anim_AtkID;
    public float Anim_AtkSpeed;
    public int Anim_OtherID;

    public Dictionary<int,Class_Sta_AtkCT> AtkCTs = new Dictionary<int,Class_Sta_AtkCT>();
    public List<Class_Sta_BufInfo> Bufs = new List<Class_Sta_BufInfo>();

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

            if (Player)
            {
                if (DeathTime == 0)
                {
                    BTManager.SEPlay(DeathSE.Clip, PosGet(), DeathSE.Volume, DeathSE.Pitch);
                    BTManager.DeathAdd();
                }
                if(DeathTime >= 300)HP = FMHP;
            }
            else if (Undet)
            {
                if (DeathTime >= 300)HP = FMHP;
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
            DeathTime++;
            DashTime = 0;
        }
        else
        {
            HP += HPRegene / 60f;
            HP -= BufPowGet(Enum_Bufs.毒) / 60f;
            HP = Mathf.Min(HP, FMHP);
            SP += SPRegene / 60f;
            DeathTime = 0;
        }
        if (!Player && Team != 0 && BTManager.End) Deletes();
        DashTime--;
        BufRems();
        AtkPlays(CamTrans.eulerAngles);
        if (Rig) Rig.useGravity = !NGravity;
    }

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
            int CTs = Mathf.RoundToInt(UseAtkD.CT * 60);
            if (Player)
            {
                CTs = Mathf.RoundToInt(CTs * (1f - PriSetGet.PassiveLVGet(Enum_Passive.CTカット) * 0.10f));
            }
            AtkCTs.Add(UseAtkSlot, new Class_Sta_AtkCT { CT = CTs, CTMax = CTs });
            if(UseAtkD.SPUse>0)SP = 0;

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
    public void BufSets(Class_Base_BufSet BufSet)
    {
        BufSets(BufSet.Buf, BufSet.Index,BufSet.Set, BufSet.Time.x, BufSet.Pow.x, BufSet.Time.y, BufSet.Pow.y);
    }
    public void BufSets(Enum_Bufs BufID, int Index, Enum_BufSet Sets, int Time, int Pow, int TMax, int PMax)
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
    void Deletes()
    {
        photonView.RPC(nameof(RPC_DeathEffect), RpcTarget.All);
        PhotonNetwork.Destroy(gameObject);
    }
    [PunRPC]
    void RPC_Damage(Vector3 HitPos, int Val)
    {
        if (HP <= 0) return;
        int Healm = FMHP - (int)HP;
        if (Val < 0) Val = -Mathf.Min(-Val, Healm);
        if (Val == 0) return;
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
                    Bufi = new Class_Sta_BufInfo { ID = BufID, Index = Index, Time = 0, Pow = 0, TimeMax = 0 };
                    Bufs.Add(Bufi);
                }
                if (Sets == (int)Enum_BufSet.付与 || Sets == (int)Enum_BufSet.切り替え)
                {
                    Bufi.Time = Mathf.Max(Bufi.Time, Time);
                    Bufi.Pow = Mathf.Max(Bufi.Pow, Pow);
                }
                else
                {
                    Bufi.Time = Mathf.Min(Bufi.Time + Time,TMax);
                    Bufi.Pow = Mathf.Min(Bufi.Pow + Pow, PMax);
                }
                Bufi.TimeMax = Mathf.Max(Bufi.Time, Bufi.TimeMax);
            }
        }
        else if (Bufi != null) Bufs.Remove(Bufi);
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
        if (AtkD==null)
        {
            AtkTime = 0;
            return;
        }
        State_Atk.Fixed(this);
        State_Atk.Shot(this,PosGet(), RotGet(),CamRot);
        State_Atk.Move(this, CamRot);
        State_Atk.State(this);
        State_Atk.WeponSet(this);
        State_Atk.Anim(this);
        State_Atk.SEPlay(this);
        AtkTime++;
        if (AtkTime > AtkD.EndTime) AtkD = null;
        #endregion
    }
    void BufRems()
    {
        for(int i = Bufs.Count - 1; i >= 0; i--)
        {
            var Bufi = Bufs[i];
            Bufi.Time--;
            if (Bufi.Time <= 0) Bufs.RemoveAt(i);
        }
    }

    void IPunObservable.OnPhotonSerializeView(Photon.Pun.PhotonStream stream, Photon.Pun.PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(MHP);
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


        }
        else
        {
            MHP = (int)stream.ReceiveNext();
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
        }
    }
}
