using Photon.Pun;
using UnityEngine;

public class Net_MyRigView : MonoBehaviour, IPunObservable
{
    private PhotonView pv;
    private Rigidbody rb;

    // 回転用
    private Quaternion targetRotation;

    void Awake()
    {
        pv = GetComponent<PhotonView>();
        rb = GetComponent<Rigidbody>();
        targetRotation = transform.rotation;
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting) // 自分が所有者
        {
            // Rigidbody の位置と速度を送る
            stream.SendNext(rb.position);
            stream.SendNext(rb.linearVelocity);

            // Transform の回転を送る
            stream.SendNext(transform.rotation);
        }
        else // 他人の表示側
        {
            // Rigidbody の同期
            Vector3 receivedPosition = (Vector3)stream.ReceiveNext();
            Vector3 receivedVelocity = (Vector3)stream.ReceiveNext();
            rb.position = receivedPosition;
            rb.linearVelocity = receivedVelocity;

            // 回転の同期（補間用に保存）
            targetRotation = (Quaternion)stream.ReceiveNext();
        }
    }

    void Update()
    {
        // 回転の補間（物理じゃないので Transform で OK）
        if (!pv.IsMine)
        {
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);
        }
    }
}
