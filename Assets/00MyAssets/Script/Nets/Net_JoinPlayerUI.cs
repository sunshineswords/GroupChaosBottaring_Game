using Photon.Pun;
using TMPro;
using UnityEngine;

public class Net_JoinPlayerUI : MonoBehaviour
{
    Photon.Realtime.Player PlayerD;
    [Tooltip("番号テキスト"), SerializeField] TextMeshProUGUI IndexTx;
    [Tooltip("プレイヤー名テキスト"), SerializeField] TextMeshProUGUI NameTx;
    [Tooltip("マスター用管理UI"), SerializeField] GameObject MasterOnlyUI;
    [Tooltip("マスター表示"), SerializeField] GameObject IsMasterUI;

    public void UISet(int Index,Photon.Realtime.Player Player)
    {
        PlayerD = Player;
        IndexTx.text = "[" + Index + "]";
        NameTx.text = Player.NickName;
        MasterOnlyUI.SetActive(PhotonNetwork.IsMasterClient && PhotonNetwork.LocalPlayer != Player);
        IsMasterUI.SetActive(Player.IsMasterClient);
    }
    public void MasterChange()
    {
        PhotonNetwork.SetMasterClient(PlayerD);
    }
    public void KickPlayer()
    {
        PhotonNetwork.CloseConnection(PlayerD);
    }
}
