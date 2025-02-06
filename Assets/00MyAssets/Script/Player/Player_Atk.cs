using Photon.Pun;
using UnityEngine;
using static DataBase;
using static PlayerValue;
public class Player_Atk :MonoBehaviourPun
{
    [SerializeField] State_Base Sta;
    [SerializeField] Player_Cont PCont;

    void Update()
    {
        if (!photonView.IsMine) return;
        Sta.AtkInput(0, DB.N_Atks[PSaves.N_AtkID], PCont.NAtk_Enter, PCont.NAtk_Stay);
        Sta.AtkInput(1, DB.S_Atks[PSaves.S1_AtkID], PCont.S1Atk_Enter, PCont.S1Atk_Stay);
        Sta.AtkInput(10, DB.E_Atks[PSaves.E_AtkID], PCont.EAtk_Enter, PCont.EAtk_Stay);

    }
}
