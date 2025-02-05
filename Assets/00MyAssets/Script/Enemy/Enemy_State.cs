using Photon.Pun;
using UnityEngine;
using static DataBase;
using static BattleManager;
public class Enemy_State : State_Base
{
    [Header("İ’è")]
    public bool Boss;
    public GameObject DeathEffect;
    [Header("•Ï”")]
    public Player_State Target;
    void Start()
    {
        HP = MHP;
    }
    void FixedUpdate()
    {
        if (!photonView.IsMine) return;
        if (!Boss && HP <= 0)
        {
            photonView.RPC(nameof(RPC_DeathEffect), RpcTarget.All);
            PhotonNetwork.Destroy(gameObject);
        }
        Atk();
    }

    [PunRPC]
    void RPC_DeathEffect()
    {
        if (DeathEffect != null)Instantiate(DeathEffect, PosGet(), Quaternion.identity);
    }
    public void TargetSet()
    {
        float NearDis = -1;
        foreach(var PS in BTManager.PlayerList)
        {
            float Dis = Vector3.Distance(PosGet(), PS.PosGet());
            if(NearDis < 0 ||NearDis > Dis)
            {
                NearDis = Dis;
                Target = PS;
            }
        }
    }


}
