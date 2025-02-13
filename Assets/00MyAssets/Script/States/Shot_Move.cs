using UnityEngine;
using static Statics;
using static BattleManager;
public class Shot_Move : MonoBehaviour
{
    [SerializeField] Shot_Obj SObj;
    [SerializeField] MoveC[] Moves;
    enum MoveModeE
    {
        重力落下 = -2,
        速度向き = -1,
        加速 = 0,
        速度変化=1,
        直線補間ホーミング = 10,
        曲線補間ホーミング = 11,
        瞬間移動 = 12,
    }
    enum TargetModeE
    {
        ターゲット,
        近敵ターゲット優先,
        近敵,
        自身=10,
        味方=20,
    }
    [System.Serializable]
    class MoveC
    {
        public Vector3Int Times;
        public MoveModeE MoveMode;
        public float Pow;
        public TargetModeE TargetMode;
        public int TargetCT;
        [HideInInspector]public State_Base Target;
        [HideInInspector] public State_Hit TargetHit;
        [HideInInspector]public int TCT;
    }

    void FixedUpdate()
    {
        for(int i = 0; i < Moves.Length; i++)
        {
            var Moved = Moves[i];
            Moved.TCT--;
            if (!V3IntTimeCheck(SObj.Times, Moved.Times)) continue;
            var RigVect = SObj.Rig.linearVelocity;
            switch (Moved.MoveMode)
            {
                case MoveModeE.重力落下:
                    RigVect += Physics.gravity * 0.01f;
                    break;
                case MoveModeE.速度向き:
                    SObj.transform.LookAt(SObj.transform.position + RigVect);
                    break;
                case MoveModeE.加速:
                    RigVect += RigVect.normalized * Moved.Pow * 0.01f;
                    break;
                case MoveModeE.速度変化:
                    RigVect = RigVect.normalized * Moved.Pow * 0.01f;
                    break;
                case MoveModeE.直線補間ホーミング:
                    TargetSet(Moved);
                    if (Moved.Target != null || Moved.TargetHit != null)
                    {
                        var TVect = (Moved.Target!=null ? Moved.Target.PosGet() : Moved.TargetHit.PosGet()) - SObj.transform.position;
                        RigVect = Vector3.Lerp(RigVect.normalized, TVect.normalized, Moved.Pow * 0.01f).normalized * RigVect.magnitude;
                    }
                    break;
                case MoveModeE.曲線補間ホーミング:
                    TargetSet(Moved);
                    if (Moved.Target != null || Moved.TargetHit != null)
                    {
                        var TVect = (Moved.Target != null ? Moved.Target.PosGet() : Moved.TargetHit.PosGet()) - SObj.transform.position;
                        RigVect = Vector3.Slerp(RigVect.normalized, TVect.normalized, Moved.Pow * 0.01f).normalized * RigVect.magnitude;
                    }
                    break;
                case MoveModeE.瞬間移動:
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
    void TargetSet(MoveC Moved)
    {
        if (Moved.TCT <= 0)
        {
            Moved.Target = null;
            Moved.TargetHit = null;
            Moved.TCT = Moved.TargetCT;
            if(Moved.TargetMode == TargetModeE.ターゲット || Moved.TargetMode == TargetModeE.近敵ターゲット優先)
            {
                Moved.Target = SObj.USta.Target;
                Moved.TargetHit = SObj.USta.TargetHit;
            }
            if (Moved.TargetMode == TargetModeE.自身) Moved.Target = SObj.USta;
            if (Moved.Target != null || Moved.TargetHit!=null) return;
            float NearDis = -1;
            foreach(var THit in BTManager.HitList)
            {
                bool Enemy = false;
                bool Flend = false;
                switch (Moved.TargetMode)
                {
                    case TargetModeE.近敵ターゲット優先:
                    case TargetModeE.近敵:
                        Enemy = true;
                        break;
                    case TargetModeE.味方:
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
