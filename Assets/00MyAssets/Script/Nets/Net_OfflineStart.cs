using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
using static DataBase;
public class Net_OfflineStart : MonoBehaviour
{
    public void OfflineStart()
    {
        PhotonNetwork.OfflineMode = true;
        PhotonNetwork.CreateRoom("Offline");
        SceneManager.LoadScene(DB.Stages[PlayerValue.StageID].SceneID);
    }
}
