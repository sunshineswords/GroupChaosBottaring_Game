using UnityEngine;
using static Statics;
using static BattleManager;
using static Manifesto;
using Photon.Pun;

public class Shot_Move : MonoBehaviourPun
{
    [SerializeField] Shot_Obj SObj;
    [SerializeField] Class_Shot_Move[] Moves;

    void FixedUpdate()
    {
        if (!photonView.IsMine) return;
        if (SObj.USta == null) return;
        for (int i = 0; i < Moves.Length; i++)
        {
            var Moved = Moves[i];
            Moved.TCT--;
            if (!V3IntTimeCheck(SObj.Times, Moved.Times)) continue;
            var RigVect = SObj.Rig.linearVelocity;
            switch (Moved.MoveMode)
            {
                case Enum_MoveMode.重力落下:
                    RigVect += Physics.gravity * 0.01f;
                    break;
                case Enum_MoveMode.速度向き:
                    SObj.transform.LookAt(SObj.transform.position + RigVect);
                    break;
                case Enum_MoveMode.加速:
                    RigVect += RigVect.normalized * Moved.Pow * 0.01f;
                    break;
                case Enum_MoveMode.速度変化:
                    RigVect = RigVect.normalized * Moved.Pow * 0.01f;
                    break;
                case Enum_MoveMode.直線補間ホーミング:
                    TargetSet(Moved);
                    if (Moved.Target != null || Moved.TargetHit != null)
                    {
                        var TVect = (Moved.Target!=null ? Moved.Target.PosGet() : Moved.TargetHit.PosGet()) - SObj.transform.position;
                        RigVect = Vector3.Lerp(RigVect.normalized, TVect.normalized, Moved.Pow * 0.01f).normalized * RigVect.magnitude;
                    }
                    break;
                case Enum_MoveMode.曲線補間ホーミング:
                    TargetSet(Moved);
                    if (Moved.Target != null || Moved.TargetHit != null)
                    {
                        var TVect = (Moved.Target != null ? Moved.Target.PosGet() : Moved.TargetHit.PosGet()) - SObj.transform.position;
                        RigVect = Vector3.Slerp(RigVect.normalized, TVect.normalized, Moved.Pow * 0.01f).normalized * RigVect.magnitude;
                    }
                    break;
                case Enum_MoveMode.瞬間移動:
                    TargetSet(Moved);
                    if (Moved.Target != null || Moved.TargetHit != null)
                    {
                        SObj.Rig.position = Moved.Target != null ? Moved.Target.PosGet() : Moved.TargetHit.PosGet();
                    }
                    break;

            }
            SObj.Rig.linearVelocity = RigVect;
        }
    }
    void TargetSet(Class_Shot_Move Moved)
    {
        if (Moved.TCT <= 0)
        {
            Moved.Target = null;
            Moved.TargetHit = null;
            Moved.TCT = Moved.TargetCT;
            if(Moved.TargetMode == Enum_TargetMode.ターゲット || Moved.TargetMode == Enum_TargetMode.近敵ターゲット優先)
            {
                    Moved.Target = SObj.USta.Target;
                    Moved.TargetHit = SObj.USta.TargetHit;
            }
            if (Moved.TargetMode == Enum_TargetMode.自身) Moved.Target = SObj.USta;
            if (Moved.Target != null || Moved.TargetHit!=null) return;
            float NearDis = -1;
            foreach(var THit in BTManager.HitList)
            {
                bool Enemy = false;
                bool Flend = false;
                switch (Moved.TargetMode)
                {
                    case Enum_TargetMode.近敵ターゲット優先:
                    case Enum_TargetMode.近敵:
                        Enemy = true;
                        break;
                    case Enum_TargetMode.味方:
                        Flend = true;
                        break;
                }
                if (TeamCheck(SObj.USta, THit.Sta, Enemy, Flend))
                {
                    var Dis = Vector3.Distance(SObj.USta.PosGet(), THit.PosGet());
                    if(NearDis <= 0 || NearDis > Dis)
                    {
                        NearDis = Dis;
                        Moved.TargetHit = THit;
                    }
                }
                
            }

        }
    }
}
