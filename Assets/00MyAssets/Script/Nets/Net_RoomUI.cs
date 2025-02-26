using UnityEngine;
using Photon.Pun;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

using static DataBase;

public class Net_RoomUI : MonoBehaviour
{
    [Tooltip("ルーム名テキスト"), SerializeField]TextMeshProUGUI RoomName;
    [Tooltip("参加プレイヤー用サブUI"), SerializeField] List<Net_JoinPlayerUI> JoinPlayers;
    [Tooltip("マスター用UI"), SerializeField] GameObject MasterOnly;
    [Tooltip("プライベート切り替えトグル"), SerializeField] Toggle PrivateT;
    [Tooltip("非マスター用UI"), SerializeField] GameObject NoMaster;

    void LateUpdate()
    {
        var CRoom = PhotonNetwork.CurrentRoom;
        if (CRoom == null) return;
        UISet();
        PlayerDisp();
    }

    void PlayerDisp()
    {
        var CRoom = PhotonNetwork.CurrentRoom;
        var PlayerKeys = CRoom.Players.Keys.ToArray();
        for (int i = 0; i < Mathf.Max(PlayerKeys.Length, JoinPlayers.Count); i++)
        {
            if (JoinPlayers.Count <= i)
            {
                JoinPlayers.Add(Instantiate(JoinPlayers[0], JoinPlayers[0].transform.parent));
            }
            if (i < PlayerKeys.Length)
            {
                JoinPlayers[i].UISet(i + 1, CRoom.Players[PlayerKeys[i]]);
            }
            JoinPlayers[i].gameObject.SetActive(i < PlayerKeys.Length);
        }
    }
    void UISet()
    {
        var CRoom = PhotonNetwork.CurrentRoom;
        RoomName.text = CRoom.Name;
        MasterOnly.SetActive(PhotonNetwork.IsMasterClient);
        NoMaster.SetActive(!PhotonNetwork.IsMasterClient);
        PrivateT.isOn = !CRoom.IsVisible;
        CRoom.IsOpen = true;
        PhotonNetwork.DestroyAll();
    }

    //ルーム退室
    public void Net_RoomExit()
    {
        PhotonNetwork.LeaveRoom();
    }
    //プライベート切り替え
    public void Net_PrivateTChange()
    {
        var CRoom = PhotonNetwork.CurrentRoom;
        CRoom.IsVisible = !PrivateT.isOn;
    }
    //ゲーム開始
    public void Net_GameStart()
    {
        var CRoom = PhotonNetwork.CurrentRoom;
        CRoom.IsOpen = false;
        PhotonNetwork.LoadLevel(DB.Stages[PlayerValue.StageID].SceneID);
    }
}
