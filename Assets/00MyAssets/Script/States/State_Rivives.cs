using UnityEngine;
using Photon.Pun;
using static Manifesto;
public class State_Rivives : MonoBehaviourPun
{
    [SerializeField] State_Base Sta;
    [SerializeField] int RiviveSecond;
    [SerializeField] float HPPer;
    bool Flag = false;

    void FixedUpdate()
    {
        if (!photonView.IsMine) return;
        if (Sta.HP > 0) Flag = false;
        else if (!Sta.BufCheck(Enum_Bufs.復活待機))
        {
            if (Flag)
            {
                Sta.HP = Sta.MHP * HPPer * 0.01f;
            }
            else
            {
                Sta.BufSets(Enum_Bufs.復活待機,0,Enum_BufSet.付与,RiviveSecond * 60,0);
                Flag = true;
            }
        }
    }
}
