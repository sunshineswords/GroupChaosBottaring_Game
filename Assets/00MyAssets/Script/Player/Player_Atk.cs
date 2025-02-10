using Photon.Pun;
using UnityEngine;
using static DataBase;
using static PlayerValue;
public class Player_Atk :MonoBehaviourPun
{
    [SerializeField] State_Base Sta;
    [SerializeField] Player_Cont PCont;
    public bool Backs;
    void Update()
    {
        if (!photonView.IsMine) return;
        if (PCont.Change_Enter) Backs = !Backs;
        var Atks = !Backs ? PriSetGet.AtkF : PriSetGet.AtkB;
        Sta.AtkInput(0, DB.N_Atks[Atks.N_AtkID], PCont.NAtk_Enter, PCont.NAtk_Stay);
        Sta.AtkInput(1, DB.S_Atks[Atks.S1_AtkID], PCont.S1Atk_Enter, PCont.S1Atk_Stay);
        Sta.AtkInput(2, DB.S_Atks[Atks.S2_AtkID], PCont.S2Atk_Enter, PCont.S2Atk_Stay);
        Sta.AtkInput(10, DB.E_Atks[Atks.E_AtkID], PCont.EAtk_Enter, PCont.EAtk_Stay);

    }
}
