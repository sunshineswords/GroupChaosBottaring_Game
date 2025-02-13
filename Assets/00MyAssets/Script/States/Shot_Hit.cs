using Photon.Pun;
using UnityEngine;
using static Statics;
public class Shot_Hit : MonoBehaviourPun
{
    [SerializeField] Shot_Obj SObj;
    [SerializeField] bool WallRem;
    private void OnTriggerStay(Collider other)
    {
        if (!photonView.IsMine) return;
        var Hit = other.GetComponent<State_Hit>();
        if (Hit != null)
        {
            if (!TeamCheck(SObj.USta, Hit.Sta)) return;
            if (Hit.Sta.HP <= 0) return;
            SObj.Hits(Hit, other.ClosestPoint(transform.position));
        }
        if (WallRem && other.tag == "Wall") SObj.ShotDel();
        
    }
}
