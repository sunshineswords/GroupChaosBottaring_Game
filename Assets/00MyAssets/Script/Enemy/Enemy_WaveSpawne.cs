using UnityEngine;
using System.Collections.Generic;
using Photon.Pun;
using static Manifesto;
public class Enemy_WaveSpawne : MonoBehaviourPun,IPunObservable
{
    public bool Clear;
    [SerializeField]int Wave;
    [SerializeField]Class_Wave[] Waves;

    List<State_Base> Enemys = new List<State_Base>();
    private void Start()
    {
        Wave = 0;
        Clear = false;
    }
    void FixedUpdate()
    {
        if (!photonView.IsMine) return;
        Debug.Log("1");
        bool Check=true;
        for(int i = 0; i < Enemys.Count; i++)
        {
            if (Enemys[i] != null && Enemys[i].HP > 0) Check = false;
        }
        if (!Check) return;
        Debug.Log("2");
        if (Wave < Waves.Length)
        {
            Enemys.Clear();
            for (int i = 0; i < Waves[Wave].Enemys.Length; i++)
            {
                var Enemy = Waves[Wave].Enemys[i];
                var Pos = transform.position;
                if (i < Waves[Wave].Pos.Length) Pos += Waves[Wave].Pos[i];
                var EnemyIns = PhotonNetwork.InstantiateRoomObject(Enemy.name, Pos, Quaternion.identity);
                var EnemyState = EnemyIns.GetComponent<State_Base>();
                if (EnemyState != null) Enemys.Add(EnemyState);
            }
            Wave++;
        }
        else Clear = true;
    }
    void IPunObservable.OnPhotonSerializeView(Photon.Pun.PhotonStream stream, Photon.Pun.PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(Clear);
        }
        else
        {
            Clear = (bool)stream.ReceiveNext();
        }
    }
}
