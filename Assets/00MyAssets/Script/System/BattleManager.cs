using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static DataBase;
public class BattleManager : MonoBehaviourPunCallbacks,IPunObservable
{
    static public BattleManager BTManager;

    [SerializeField] public int TimeLimSec;
    public int TimeStar;
    public int DeathStar;

    public int Time;
    public int DeathCount;
    public int Star;
    public bool BossCheck;
    public bool End;
    [System.NonSerialized] public List<State_Base> StateList = new List<State_Base>();
    [System.NonSerialized] public List<State_Hit> HitList = new List<State_Hit>();
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
        if (!photonView.IsMine) return;
        if (!End)
        {
            if (Time > 0) Time--;
            BossCheck = BossList.Count > 0;
            for (int i = 0; i < BossList.Count; i++)
            {
                if (BossList[i].HP > 0) BossCheck = false;
            }
            if (BossCheck || Time <= 0) End = true;
            if (TimeLimSec < 0) End = false;
            Star = 3;
            if (Time <= TimeStar * 60) Star--;
            if (DeathCount > DeathStar) Star--;
            if (Time <= 0) Star--;
        }

    }
    void ListSet()
    {
        HitList = FindObjectsByType<State_Hit>(FindObjectsSortMode.None).OrderBy(x => x.Sta.photonView.ViewID).ToList();
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
    public void SEPlay(AudioClip SEClip, Vector3 Pos, float Volume, float Pitch,bool Local=false)
    {
        var SEID = DB.SEs.IndexOf(SEClip);
        if (SEID < 0) return;
        if (!Local) photonView.RPC(nameof(RPC_SEPlay), RpcTarget.All, SEID, Pos, Volume, Pitch);
        else RPC_SEPlay(SEID, Pos, Volume, Pitch);
    }
    [PunRPC]
    void RPC_DeathAdd()
    {
        if (!photonView.IsMine) return;
        DeathCount++;
    }
    [PunRPC]
    void RPC_SEPlay(int SEID, Vector3 Pos, float Volume, float Pitch)
    {
        int VCount = Mathf.CeilToInt(Volume / 100f);
        for (int i = 0; i < VCount; i++)
        {
            var SEObj = Instantiate(DB.SEObj, Pos, Quaternion.identity);
            SEObj.clip = DB.SEs[SEID];
            if (i == VCount - 1) SEObj.volume = (Volume * 0.01f) % 1f;
            else SEObj.volume = 1f;
            SEObj.pitch = Pitch / 100f;
            if (Pitch < 0) SEObj.time = DB.SEs[SEID].length - 0.01f;
            SEObj.Play();
            Destroy(SEObj.gameObject, 10f);
        }
    }
    void IPunObservable.OnPhotonSerializeView(Photon.Pun.PhotonStream stream, Photon.Pun.PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(Time);
            stream.SendNext(DeathCount);
            stream.SendNext(Star);
            stream.SendNext(BossCheck);
            stream.SendNext(End);
        }
        else
        {
            Time = (int)stream.ReceiveNext();
            DeathCount = (int)stream.ReceiveNext();
            Star = (int)stream.ReceiveNext();
            BossCheck = (bool)stream.ReceiveNext();
            End = (bool)stream.ReceiveNext();
        }
    }
}
