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
    public GameObject DeathEffect;
    public int Team;
    public bool Player;
    public bool Boss;
    public bool Undet;
    [Header("ステータス")]
    public int MHP;
    public int Atk;
    public int Def;
    [Header("変数")]
    public int HP;
    public int SP;
    public int DeathTime;
    public int DashTime;

    public float SpeedRem;
    public bool NoJump;
    public bool NoDash;
    public bool Aiming;

    public Data_Atk AtkD;
    public int AtkSlot;
    public int AtkTime;
    public int AtkBranch;

    public State_Base Target;
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
        if (!PhotonNetwork.OfflineMode)
        {
            if (Player) Name = photonView.Owner.NickName;
        }
    }
    private void FixedUpdate()
    {
        Anim_OtherID = 0;
        if (HP <= 0) Anim_OtherID = 2;

        if (!photonView.IsMine) return;
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
            else if (!Boss && Team!=0)
            {
                if (DeathTime >= 60) Deletes();
            }
            DeathTime++;
            DashTime = 0;
        }
        else
        {
            DeathTime = 0;
        }
        if (!Player && BTManager.End) Deletes();
        DashTime--;

        AtkPlays(CamTrans.eulerAngles);
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
            SP -= UseAtkD.SPUse;

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
        Color DamCol = Color.white;
        GameObject HitEffect = DB.EnemyHitEffect;
        switch (Team)
        {
            case 0:
                DamCol = Color.red;
                HitEffect = DB.PlayerHitEffect;
                break;
        }
        DamageObj.DamageSet(HitPos, Val, DamCol);
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
        SpeedRem = 0;
        NoJump = false;
        NoDash = false;
        Aiming = false;
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
            stream.SendNext(Anim_MoveID);
            stream.SendNext(Anim_AtkID);
            stream.SendNext(Anim_OtherID);

        }
        else
        {
            MHP = (int)stream.ReceiveNext();
            Team = (int)stream.ReceiveNext();

            HP = (int)stream.ReceiveNext();
            Anim_MoveID = (int)stream.ReceiveNext();
            Anim_AtkID = (int)stream.ReceiveNext();
            Anim_OtherID = (int)stream.ReceiveNext();
        }
    }
}
