using Photon.Pun;
using UnityEngine;
using static PlayerValue;
using static DataBase;
using static Manifesto;
using static Statics;
public class Player_Move : MonoBehaviourPun
{
    public State_Base Sta;
    [SerializeField] Player_Cont PCont;

    [SerializeField] Camera Cam;
    [SerializeField] float SpeedRem;
    [SerializeField] float MoveSpeed;
    [SerializeField] float LerpSpeed;
    [SerializeField] int DashTime;
    [SerializeField] float DashSpeed;
    [SerializeField] float DashMPCost;

    [SerializeField] float JumpPow;
    [SerializeField] float AirJumpMPCost;

  
    Vector3 DashVect;
    private void Start()
    {
        if (!photonView.IsMine) return;
        float SpeedAdd = 1f + PriSetGet.PassiveLVGet(Enum_Passive.速度増加) * 0.1f;
        MoveSpeed *= SpeedAdd;
        DashSpeed *= SpeedAdd;
    }
    private void Update()
    {
        if (!photonView.IsMine) return;
        var RigVect = Sta.Rig.linearVelocity;
        #region ジャンプ
        if (!Sta.NoJump && PCont.Jump_Enter && RigVect.y <= JumpPow * 0.005f)
        {
            if (Sta.Ground)RigVect.y = JumpPow * 0.02f;
            else if(!Sta.LowMP)
            {
                Sta.MP -= AirJumpMPCost;
                RigVect.y = JumpPow * 0.01f;
            }
        }
        #endregion
        #region ダッシュ
        var MoveInput = PCont.Move;
        var MoveVect = Cam.transform.forward * MoveInput.y + Cam.transform.right * MoveInput.x;
        MoveVect.y = 0;

        if (!Sta.NoDash && Sta.DashTime <= 0 && !Sta.LowMP && PCont.Dash_Enter)
        {
            Sta.AtkD = null;
            Sta.MP -= DashMPCost;
            Sta.DashTime = DashTime;
            DashVect = MoveVect.magnitude >= 0.1f ? MoveVect : Sta.Rig.transform.forward;
        }
        #endregion
        Sta.Rig.linearVelocity = RigVect;
    }
    void FixedUpdate()
    {
        if (!photonView.IsMine) return;


        var RigVect = Sta.Rig.linearVelocity;
        Sta.Anim_MoveID = 0;
        #region 減速
        float Rem = 1f - SpeedRem * 0.01f;
        RigVect.x *= Rem;
        RigVect.z *= Rem;
        #endregion
        #region 移動
        if (Sta.DashTime <= 0)
        {
            var MoveInput = PCont.Move;
            var MoveVect = Cam.transform.forward * MoveInput.y + Cam.transform.right * MoveInput.x;
            MoveVect.y = 0;
            if (MoveVect.magnitude > 0.1f)
            {
                Sta.Anim_MoveID = 1;
                RigVect += MoveVect.normalized * MoveSpeed * 0.01f * (1f - Sta.SpeedRem * 0.01f);
                var LookVect = Sta.Rig.transform.forward;
                LookVect = Vector3.Slerp(LookVect.normalized, MoveVect.normalized, LerpSpeed * 0.01f);
                LookVect.y = 0;
                Sta.Rig.transform.LookAt(Sta.Rig.transform.position + LookVect);
            }
        }
        else
        {
            RigVect = DashVect.normalized * DashSpeed * 0.01f;
            Sta.Rig.transform.LookAt(Sta.Rig.transform.position + DashVect);
            Sta.Anim_MoveID = 2;
        }
        #endregion
        #region 照準
        if (Sta.AtkD != null && Sta.Aiming)
        {
            var LookRot = Cam.transform.eulerAngles;
            LookRot.x = 0;
            Sta.Rig.transform.eulerAngles = LookRot;
        }
        else
        {
            if (Sta.TargetHit != null)
            {
                var TargetVect = Sta.TargetHit.PosGet() - Sta.Rig.transform.position;
                TargetVect.y = 0;
                Sta.Rig.transform.LookAt(Sta.Rig.transform.position + TargetVect);
            }
        }
        #endregion

        Sta.Rig.linearVelocity = RigVect;
    }
    private void OnCollisionStay(Collision collision)
    {
        Sta.GroundB = true;
    }
}
