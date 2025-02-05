using Photon.Pun;
using UnityEngine;
using static Statics;
public class Shot_Hit : MonoBehaviourPun
{
    [SerializeField] Shot_Obj SObj;
    private void OnTriggerStay(Collider other)
    {
        if (!photonView.IsMine) return;
        var Hit = other.GetComponent<State_Hit>();
        if (Hit != null)
        {
            if (!TeamCheck(SObj.USta, Hit.Sta)) return;
            SObj.Hits(Hit, other.ClosestPoint(transform.position));
        }
        
    }
}
