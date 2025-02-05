using Photon.Pun;
using UnityEngine;
using static BattleManager;
using static DataBase;
public class Player_State : State_Base
{
    private void Start()
    {
        HP = MHP;
    }
    private void FixedUpdate()
    {
        if (!photonView.IsMine) return;
        if(HP <= 0)
        {
            BTManager.DeathAdd();
            HP = MHP;
        }
        Atk();
    }
    

}
