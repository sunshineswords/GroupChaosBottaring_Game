using Photon.Pun;
using UnityEngine;
using static DataBase;
public class State_Base : MonoBehaviourPun,IPunObservable
{
    public string Name;
    public Rigidbody Rig;
    public float Hight;
    public int Team;
    public int MHP;
    public Data_Atk[] Atks;

    public int HP;
    public int AtkID = -1;
    public int AtkTime;
    public int Anim_MoveID;
    public int Anim_AtkID;
    public int Anim_OtherID;

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
    [PunRPC]
    protected void RPC_Damage(Vector3 HitPos, int Val)
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
    public void AtkStart(int AtkIDs)
    {
        if (AtkID >= 0) return;
        var AtkD = Atks[AtkIDs];
        AtkID = AtkIDs;
        AtkTime = 0;
    }
    protected void Atk()
    {
        Anim_AtkID = 0;
        if (AtkID < 0)
        {
            AtkTime = 0;
            return;
        }
        var AtkD = Atks[AtkID];
        State_Atk.Shot(this, AtkD, PosGet(), RotGet());
        State_Atk.Anim(this, AtkD);

        AtkTime++;

        if (AtkTime > AtkD.EndTime) AtkID = -1;
    }
    void IPunObservable.OnPhotonSerializeView(Photon.Pun.PhotonStream stream, Photon.Pun.PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(MHP);

            stream.SendNext(HP);
            stream.SendNext(Anim_MoveID);
            stream.SendNext(Anim_AtkID);
            stream.SendNext(Anim_OtherID);

        }
        else
        {
            MHP = (int)stream.ReceiveNext();

            HP = (int)stream.ReceiveNext();
            Anim_MoveID = (int)stream.ReceiveNext();
            Anim_AtkID = (int)stream.ReceiveNext();
            Anim_OtherID = (int)stream.ReceiveNext();
        }
    }
}
