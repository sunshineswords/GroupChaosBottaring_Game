using Photon.Pun;
using UnityEngine;

public class Player_BattleValues : MonoBehaviour, IPunObservable
{
    [System.NonSerialized] public int AddTimer = 0;
    [System.NonSerialized] public int[] AddDams = new int[10];
    [System.NonSerialized] public float AddDamTotal = 0;
    [System.NonSerialized] public int[] AddHits = new int[10];
    [System.NonSerialized] public int AddHitTotal = 0;
    [System.NonSerialized] public float AddHeal = 0;
    [System.NonSerialized] public int AddBuf = 0;
    [System.NonSerialized] public int AddDBuf = 0;
    [System.NonSerialized] public int E_AtkCount = 0;
    [System.NonSerialized] public float ReceiveDam = 0;
    [System.NonSerialized] public int DeathCount = 0;

    void IPunObservable.OnPhotonSerializeView(Photon.Pun.PhotonStream stream, Photon.Pun.PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(AddDamTotal);
            stream.SendNext(AddHitTotal);
            stream.SendNext(AddHeal);
            stream.SendNext(AddBuf);
            stream.SendNext(AddDBuf);
            stream.SendNext(E_AtkCount);
            stream.SendNext(ReceiveDam);
            stream.SendNext(DeathCount);
        }
        else
        {
            AddDamTotal = (float)stream.ReceiveNext();
            AddHitTotal = (int)stream.ReceiveNext();
            AddHeal = (float)stream.ReceiveNext();
            AddBuf = (int)stream.ReceiveNext();
            AddDBuf = (int)stream.ReceiveNext();
            E_AtkCount = (int)stream.ReceiveNext();
            ReceiveDam = (float)stream.ReceiveNext();
            DeathCount = (int)stream.ReceiveNext();
        }
    }
}
