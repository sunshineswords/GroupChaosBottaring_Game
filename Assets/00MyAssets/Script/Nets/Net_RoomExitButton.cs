using Photon.Pun;
using TMPro;
using UnityEngine;

public class Net_RoomExitButton : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI Texts;
    [SerializeField] string MasterTxs;
    [SerializeField] string NoMasterTxs;

    bool Master => !PhotonNetwork.OfflineMode && PhotonNetwork.IsMasterClient;
    private void Update()
    {
        Texts.text = Master ? MasterTxs : NoMasterTxs;
    }
    //退室
    public void ExitB()
    {
        if (Master)PhotonNetwork.LoadLevel(0);
        else PhotonNetwork.LeaveRoom();
    }
}
