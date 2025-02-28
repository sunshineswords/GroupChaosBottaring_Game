using UnityEngine;
using Photon.Pun;
using TMPro;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;

using static DataBase;
using static PlayerValue;

public class Net_RoomUI : MonoBehaviour
{
    [Tooltip("ルーム名テキスト"), SerializeField]TextMeshProUGUI RoomName;
    [Tooltip("参加プレイヤー用サブUI"), SerializeField] List<Net_JoinPlayerUI> JoinPlayers;
    [Tooltip("マスター用UI"), SerializeField] GameObject MasterOnly;
    [Tooltip("プライベート切り替えトグル"), SerializeField] Toggle PrivateT;
    [Tooltip("非マスター用UI"), SerializeField] GameObject NoMaster;
    [SerializeField] TextMeshProUGUI StageTx;
    [SerializeField] RawImage StageImage;
    private void Start()
    {
        if (!PhotonNetwork.InRoom) return;
        if (!PhotonNetwork.IsMasterClient) return;
        PhotonNetwork.DestroyAll();
    }
    void LateUpdate()
    {
        var CRoom = PhotonNetwork.CurrentRoom;
        if (CRoom == null) return;
        if (PhotonNetwork.IsMasterClient) StageSet();
        CharaSet();
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
        var CPro = CRoom.CustomProperties;
        RoomName.text = CRoom.Name;
        MasterOnly.SetActive(PhotonNetwork.IsMasterClient);
        NoMaster.SetActive(!PhotonNetwork.IsMasterClient);
        PrivateT.isOn = !CRoom.IsVisible;
        CRoom.IsOpen = true;
        var SID = CPro.TryGetValue("StageID", out var oSID) ? (int)oSID : 0;
        var StageData = DB.Stages[SID];
        StageTx.text = StageData.Name;
        StageImage.texture = StageData.Icon;
    }
    void StageSet()
    {
        var CRoom = PhotonNetwork.CurrentRoom;
        var CPro = new ExitGames.Client.Photon.Hashtable();
        CPro["StageID"] = StageID;
        CRoom.SetCustomProperties(CPro);
    }
    void CharaSet()
    {
        var CPro = new ExitGames.Client.Photon.Hashtable();
        CPro["Chara"] = PriSetGet.CharaID;

        CPro["FAtk_0"] = PriSetGet.AtkF.N_AtkID;
        CPro["FAtk_1"] = PriSetGet.AtkF.S1_AtkID;
        CPro["FAtk_2"] = PriSetGet.AtkF.S2_AtkID;
        CPro["FAtk_3"] = PriSetGet.AtkF.E_AtkID;

        CPro["BAtk_0"] = PriSetGet.AtkB.N_AtkID;
        CPro["BAtk_1"] = PriSetGet.AtkB.S1_AtkID;
        CPro["BAtk_2"] = PriSetGet.AtkB.S2_AtkID;
        CPro["BAtk_3"] = PriSetGet.AtkB.E_AtkID;

        CPro["Passive_0"] = PriSetGet.Passive.P1_ID;
        CPro["Passive_1"] = PriSetGet.Passive.P2_ID;
        CPro["Passive_2"] = PriSetGet.Passive.P3_ID;
        CPro["Passive_3"] = PriSetGet.Passive.P4_ID;
        PhotonNetwork.SetPlayerCustomProperties(CPro);
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

        var CRoomCP = new ExitGames.Client.Photon.Hashtable();
        CRoomCP["SceneID"] = DB.Stages[PlayerValue.StageID].SceneID;
        CRoom.SetCustomProperties(CRoomCP);
    }
}
