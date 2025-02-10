using Photon.Pun;
using UnityEngine;

public class Enemy_TestMove : MonoBehaviourPun
{
    [SerializeField] State_Base Sta;
    [SerializeField] float SpeedRem;
    [SerializeField] float MoveSpeed;
    [SerializeField] float LerpSpeed;

    void FixedUpdate()
    {
        if (!photonView.IsMine) return;
        if (Sta.HP <= 0) return;
        Sta.TargetSet();
        var RigVect = Sta.Rig.linearVelocity;
        var Rem = 1f - SpeedRem * 0.01f;
        RigVect.x *= Rem;
        RigVect.y *= Rem;
        Sta.Anim_MoveID = 0;
        if (Sta.Target != null)
        {
            Sta.Anim_MoveID = 1;
            var MoveVect = Sta.Target.PosGet() - Sta.PosGet();
            MoveVect.y = 0;
            RigVect += MoveVect.normalized * MoveSpeed * 0.01f * (1f - Sta.SpeedRem * 0.01f);
            var LookVect = Sta.Rig.transform.forward;
            LookVect = Vector3.Slerp(LookVect.normalized, MoveVect.normalized, LerpSpeed * 0.01f);
            LookVect.y = 0;
            Sta.Rig.transform.LookAt(Sta.Rig.transform.position + LookVect);
        }
        Sta.Rig.linearVelocity = RigVect;
    }
}
