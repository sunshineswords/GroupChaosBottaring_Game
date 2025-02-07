using Photon.Pun;
using UnityEngine;
using static DataBase;
using static PlayerValue;
public class Player_CharaSet : MonoBehaviourPun,IPunObservable
{
    [SerializeField] State_Base Sta;
    int CharaID;
    int BCharaID=-1;
    [SerializeField] GameObject CharaObj;

    void Update()
    {
        if (photonView.IsMine) CharaID = PSaves.CharaID;
        if (BCharaID != CharaID)
        {
            BCharaID = CharaID;
            if(CharaObj!=null)Destroy(CharaObj);
            var InsChara = Instantiate(DB.Charas[CharaID].ModelObj, transform.position, transform.rotation);
            InsChara.transform.parent = transform;
            InsChara.Sta = Sta;
            CharaObj = InsChara.gameObject;
        }
    }
    void IPunObservable.OnPhotonSerializeView(Photon.Pun.PhotonStream stream, Photon.Pun.PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(CharaID);
        }
        else
        {
            CharaID = (int)stream.ReceiveNext();
        }
    }
}
