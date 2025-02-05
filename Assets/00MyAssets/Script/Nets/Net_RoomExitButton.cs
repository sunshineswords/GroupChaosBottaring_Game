using Photon.Pun;
using UnityEngine;

public class Net_RoomExitButton : MonoBehaviour
{
    //‘ÞŽº
    public void ExitB()
    {
        PhotonNetwork.LeaveRoom();
    }
}
