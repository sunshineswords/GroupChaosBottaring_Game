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

    [System.NonSerialized] public List<Player_State> PlayerList = new List<Player_State>();
    [System.NonSerialized]public List<Enemy_State> EnemyList = new List<Enemy_State>();
    [System.NonSerialized] public List<Enemy_State> BossList = new List<Enemy_State>();

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
        PlayerList = FindObjectsByType<Player_State>(FindObjectsSortMode.None).OrderBy(x => x.photonView.ViewID).ToList();
        EnemyList = FindObjectsByType<Enemy_State>(FindObjectsSortMode.None).OrderBy(x => x.photonView.ViewID).ToList();
        BossList.Clear();
        for(int i=0;i< EnemyList.Count; i++)
        {
            if (EnemyList[i].Boss)BossList.Add(EnemyList[i]);
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
