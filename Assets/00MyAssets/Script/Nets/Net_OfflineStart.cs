using Photon.Pun;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Net_OfflineStart : MonoBehaviour
{
    public void OfflineStart()
    {
        PhotonNetwork.OfflineMode = true;
        PhotonNetwork.CreateRoom("Offline");
        SceneManager.LoadScene(1);
    }
}
