using Photon.Pun;
using UnityEngine;
using static DataBase;
using static Statics;
using static BattleManager;
using System.Collections.Generic;
using System.Linq;

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
    [Tooltip("死亡エフェクト")] public GameObject DeathEffect;
    [Header("ステータス")]
    [Tooltip("最大HP")]public int MHP;
    [Tooltip("秒間HP回復速度")] public float HPRegene;
    [Tooltip("最大MP(移動力)")] public int MMP;
    [Tooltip("秒間MP回復速度")] public float MPRegene;
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
    public int Anim_OtherID;

    public Dictionary<int,AtkCTC> AtkCTs = new Dictionary<int,AtkCTC>();
    [System.Serializable]
    public class AtkCTC
    {
        public int CT;
        public int CTMax;
    }
    
    private void Start()
    {
        HP = MHP;
        MP = MMP;
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
        MP = Mathf.Min(MP, MMP);
        if (LowMP && MP >= MMP * 0.2f) LowMP = false;
        if (HP <= 0)
        {

            if (Player)
            {
                if(DeathTime == 0)BTManager.DeathAdd();
                if(DeathTime >= 300)HP = MHP;
            }
            else if (Undet)
            {
                if (DeathTime >= 300)HP = MHP;
            }
            else if (!Boss)
            {
                if (DeathTime >= 60) Deletes();
            }
            DeathTime++;
            DashTime = 0;
        }
        else
        {
            HP += HPRegene / 60f;
            HP = Mathf.Min(HP, MHP);
            DeathTime = 0;
        }
        if (!Player && Team != 0 && BTManager.End) Deletes();
        DashTime--;

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
            AtkCTs.Add(UseAtkSlot, new AtkCTC { CT = CTs, CTMax = CTs });
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

    void Deletes()
    {
        photonView.RPC(nameof(RPC_DeathEffect), RpcTarget.All);
        PhotonNetwork.Destroy(gameObject);
    }
    [PunRPC]
    void RPC_Damage(Vector3 HitPos, int Val)
    {
        Color DamCol = Val >= 0 ? Color.white : Color.magenta;
        GameObject HitEffect = DB.EnemyHitEffect;
        switch (Team)
        {
            case 0:
                DamCol = Val >= 0 ? Color.red : Color.green;
                HitEffect = DB.PlayerHitEffect;
                break;
        }
        DamageObj.DamageSet(HitPos, Mathf.Abs(Val), DamCol);
        var InsHitEffect = Instantiate(HitEffect, HitPos, Quaternion.identity);
        if (!photonView.IsMine) return;
        HP -= Val;
    }
    [PunRPC]
    void RPC_DeathEffect()
    {
        if (DeathEffect != null) Instantiate(DeathEffect, PosGet(), Quaternion.identity);
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
        AtkTime++;
        if (AtkTime > AtkD.EndTime) AtkD = null;
        #endregion
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
            Anim_OtherID = (int)stream.ReceiveNext();
        }
    }
}
