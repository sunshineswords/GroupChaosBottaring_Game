using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Net_BaseUI : MonoBehaviourPunCallbacks
{
    [Tooltip("同期するプレファブ"), SerializeField] Net_PhotonPrefabs Prefabs;
    [Tooltip("オフラインUI"), SerializeField] GameObject OfflineUI;
    [Tooltip("オンラインUI"), SerializeField] GameObject OnlineUI;
    [Tooltip("接続待機UI"), SerializeField] GameObject ConnectWait;
    [Tooltip("接続完了UI"), SerializeField] GameObject ConnectEnd;
    [Tooltip("ルーム選択処理"), SerializeField] UIChange Selects;
    [Tooltip("ルーム選択用UI"), SerializeField] GameObject SelectUI;
    [Tooltip("プレイヤー名入力"), SerializeField] TMP_InputField PlayerNameIn;
    [Tooltip("作成ルーム名入力"), SerializeField] TMP_InputField CreateNameIn;
    [Tooltip("作成メッセージ入力"), SerializeField] TMP_InputField MessageIn;
    [Tooltip("作成プライベート"),SerializeField] Toggle PrivateT;
    [Tooltip("参加ルーム名入力"), SerializeField] TMP_InputField JoinNameIn;
    [Tooltip("ロビー用サブUI"), SerializeField] List<SinsUI_LobbyRoomUIs> LobbySinUIs;
    [Tooltip("ルーム内UI"), SerializeField] GameObject InRoomUI;

    private void Start()
    {
        Prefabs.PrefabPoolSet();
        Net_MyCustomTypes.Register();
        Application.targetFrameRate = 60;
        PhotonNetwork.OfflineMode = true;
        PhotonNetwork.EnableCloseConnection = true;
        PhotonNetwork.AutomaticallySyncScene = true;
        PlayerNameIn.text = PlayerPrefs.GetString("PlayerName", "");
        Net_PlayerNameSet();

        PlayerValue.Load();
    }
    void LateUpdate()
    {
        if (!PlayerNameIn.isFocused) PlayerNameIn.text = PhotonNetwork.NickName;
        UIActives();
        LobbyIns();
    }
    /// <summary>UI表示</summary>
    void UIActives()
    {
        bool Connect = !PhotonNetwork.IsConnected || !PhotonNetwork.IsConnectedAndReady;
        OfflineUI.SetActive(PhotonNetwork.OfflineMode);
        OnlineUI.SetActive(!PhotonNetwork.OfflineMode);
        ConnectWait.SetActive(Connect);
        ConnectEnd.SetActive(!Connect);
        SelectUI.SetActive(!PhotonNetwork.InRoom);
        InRoomUI.SetActive(PhotonNetwork.InRoom);
    }
    /// <summary>ロビー参加</summary>
    void LobbyIns()
    {
        if (Selects.UIID == 3 && !PhotonNetwork.InLobby) PhotonNetwork.JoinLobby();
        if (Selects.UIID != 3 && PhotonNetwork.InLobby) PhotonNetwork.LeaveLobby();
    }
    //プレイヤー名変更
    public void Net_PlayerNameSet()
    {
        if (PlayerNameIn.text != "") PhotonNetwork.NickName = PlayerNameIn.text;
        else PhotonNetwork.NickName = "無名" + Random.Range(1000,10000);
        PlayerPrefs.SetString("PlayerName", PhotonNetwork.NickName);
    }
    //サーバー接続
    public void Net_Connects()
    {
        PhotonNetwork.ConnectUsingSettings();
    }
    //サーバー切断
    public void Net_Disconnect()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.OfflineMode = true;
    }
    //新規ルーム作成
    public void Net_RoomCreate()
    {
        var RoomIDs = CreateNameIn.text;
        if (RoomIDs == "")
        {
            if (Random.value >= 0.1f) RoomIDs = "CreateRoom";
            else RoomIDs = "NaNameRoom";
            RoomIDs += Random.Range(0, 10000).ToString("D4");
        }
        PhotonNetwork.CreateRoom(RoomIDs,RoomOptionGet(MessageIn.text,PrivateT.isOn));
    }
    //名前ルーム参加
    public void Net_RoomJoin()
    {
        PhotonNetwork.JoinRoom(JoinNameIn.text);
    }
    //ランダムルーム参加
    public void Net_RandomJoin()
    {
        PhotonNetwork.JoinRandomRoom();
    }


    //ランダムルーム用作成
    public override void OnJoinRandomFailed(short returnCode, string message)
    {

        string RoomIDs;
        if (Random.value >= 0.1f) RoomIDs = "RandomRoom";
        else RoomIDs = "BackRoom";
        RoomIDs += Random.Range(0, 10000).ToString("D4");
        PhotonNetwork.CreateRoom(RoomIDs, RoomOptionGet("ランダムルーム",false));
    }
    /// <summary>ルームオプション設定</summary>
    public RoomOptions RoomOptionGet(string Message, bool Private)
    {
        var RoomOP = new RoomOptions();
        var RoomHash = new ExitGames.Client.Photon.Hashtable();
        RoomHash["Message"] = Message;
        RoomHash["GameVer"] = Application.version;
        RoomHash["GameStarts"] = false;
        RoomOP.MaxPlayers = 20;
        RoomOP.CustomRoomProperties = RoomHash;
        RoomOP.CustomRoomPropertiesForLobby = new string[] { "Message", "GameVer", "GameStarts" };
        RoomOP.IsVisible = !Private;
        return RoomOP;
    }
    //オフライン化
    public override void OnDisconnected(DisconnectCause cause)
    {
        PhotonNetwork.OfflineMode = true;
    }
    //ロビー用
    Net_RoomList roomList = new Net_RoomList();
    public override void OnRoomListUpdate(List<RoomInfo> changedRoomList)
    {
        roomList.Update(changedRoomList);
        if (PhotonNetwork.InLobby)
        {
            List<RoomInfo> Rooms = roomList.ToList();
            for (int i = 0; i < Mathf.Max(LobbySinUIs.Count, Rooms.Count); i++)
            {
                if (i < Rooms.Count)
                {
                    if (LobbySinUIs.Count <= i)
                    {
                        LobbySinUIs.Add(Instantiate(LobbySinUIs[0], LobbySinUIs[0].transform.parent));
                    }
                    var RoomSin = LobbySinUIs[i];
                    RoomSin.gameObject.SetActive(true);
                    RoomSin.Disp(Rooms[i]);
                }
                else LobbySinUIs[i].gameObject.SetActive(false);
            }
        }
    }

}
