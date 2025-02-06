using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BattleManager : MonoBehaviourPunCallbacks,IPunObservable
{
    static public BattleManager BTManager;

    [SerializeField] public int TimeLimSec;
    public int Time;
    public int DeathCount;
    [System.NonSerialized] public List<State_Base> StateList = new List<State_Base>();
    [System.NonSerialized] public List<State_Base> PlayerList = new List<State_Base>();
    [System.NonSerialized] public List<State_Base> BossList = new List<State_Base>();

    void Start()
    {
        BTManager = this;
        ListSet();
        if (!PhotonNetwork.InRoom) return;
        if (photonView.IsMine)
        {
            Time = TimeLimSec * 60;
        }
    }


    void FixedUpdate()
    {
        if (!PhotonNetwork.InRoom) return;
        ListSet();
        if (photonView.IsMine)
        {
            Time--;
        }
    }
    void ListSet()
    {
        StateList = FindObjectsByType<State_Base>(FindObjectsSortMode.None).OrderBy(x => x.photonView.ViewID).ToList();
        PlayerList.Clear();
        BossList.Clear();
        for (int i = 0; i < StateList.Count; i++)
        {
            var Sta = StateList[i];
            if (Sta.Player) PlayerList.Add(Sta);
            if (Sta.Boss) BossList.Add(Sta);
        }
    }
    public void DeathAdd()
    {
        photonView.RPC(nameof(RPC_DeathAdd), RpcTarget.All);
    }
    [PunRPC]
    void RPC_DeathAdd()
    {
        if (!photonView.IsMine) return;
        DeathCount++;
    }
    void IPunObservable.OnPhotonSerializeView(Photon.Pun.PhotonStream stream, Photon.Pun.PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(Time);
            stream.SendNext(DeathCount);
        }
        else
        {
            Time = (int)stream.ReceiveNext();
            DeathCount = (int)stream.ReceiveNext();
        }
    }
}
