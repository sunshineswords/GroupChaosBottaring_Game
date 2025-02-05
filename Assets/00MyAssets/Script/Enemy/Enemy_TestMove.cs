using Photon.Pun;
using UnityEngine;

public class Enemy_TestMove : MonoBehaviourPun
{
    [SerializeField] Enemy_State ESta;
    [SerializeField] float MoveSpeed;
    [SerializeField] float LerpSpeed;

    void FixedUpdate()
    {
        if (!photonView.IsMine) return;
        if (ESta.HP <= 0) return;
        ESta.TargetSet();
        var RigVect = ESta.Rig.linearVelocity;
        ESta.Anim_MoveID = 0;
        if (ESta.Target != null)
        {
            ESta.Anim_MoveID = 1;
            var MoveVect = ESta.Target.PosGet() - ESta.PosGet();
            MoveVect.y = 0;
            RigVect += MoveVect.normalized * MoveSpeed * 0.01f;
            var LookVect = ESta.Rig.transform.forward;
            LookVect = Vector3.Slerp(LookVect.normalized, MoveVect.normalized, LerpSpeed * 0.01f);
            LookVect.y = 0;
            ESta.Rig.transform.LookAt(ESta.Rig.transform.position + LookVect);
        }
        else
        {
            RigVect *= 0.9f;
        }
        ESta.Rig.linearVelocity = RigVect;
    }
}
