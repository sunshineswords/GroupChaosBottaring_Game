using Photon.Pun;
using UnityEngine;

public class Player_Move : MonoBehaviourPun
{
    public State_Base Sta;
    [SerializeField] Player_Cont PCont;
    [SerializeField] Transform CamRotTrans;
    [SerializeField] Transform CamPosTrans;
    [SerializeField] float ZomeSpeed;
    [SerializeField] float ZomeDis;
    [SerializeField] float CamHight;
    [SerializeField] Vector2 RotSpeed;
    [SerializeField] Vector2 RotLim;
    [SerializeField] Camera Cam;
    [SerializeField] float SpeedRem;
    [SerializeField] float MoveSpeed;
    [SerializeField] int DashTime;
    [SerializeField] float DashSpeed;
    [SerializeField] float LerpSpeed;
    [SerializeField] float JumpPow;

    bool GroundB;
    bool Ground;
    Vector3 DashVect;
    Vector3 PosBase;
    private void Start()
    {
        PosBase = CamPosTrans.localPosition;
    }
    private void Update()
    {
        if (!photonView.IsMine) return;
        #region カメラ
        var LookInput = PCont.Look;
        LookInput.x *= RotSpeed.y * 0.01f;
        LookInput.y *= -RotSpeed.x * 0.01f;
        var CamRot = CamRotTrans.eulerAngles;
        CamRot.x = Mathf.Clamp(Mathf.Repeat(CamRot.x + 180 + LookInput.y, 360f) - 180f, RotLim.x, RotLim.y);
        CamRot.y += LookInput.x;
        CamRotTrans.eulerAngles = CamRot;
        CamRotTrans.position = Sta.Rig.position + (Vector3.up * CamHight);
        #endregion
        var RigVect = Sta.Rig.linearVelocity;
        #region ジャンプ
        if (!Sta.NoJump && GroundB && PCont.Jump_Enter) RigVect.y = JumpPow * 0.01f;
        #endregion
        #region ダッシュ
        var MoveInput = PCont.Move;
        var MoveVect = Cam.transform.forward * MoveInput.y + Cam.transform.right * MoveInput.x;
        MoveVect.y = 0;

        if (!Sta.NoDash && PCont.Dash_Enter)
        {
            Sta.AtkD = null;
            Sta.DashTime = DashTime;
            DashVect = MoveVect.magnitude >= 0.1f ? MoveVect : Sta.Rig.transform.forward;
        }
        #endregion
        Sta.Rig.linearVelocity = RigVect;
    }
    void FixedUpdate()
    {
        if (!photonView.IsMine) return;
        GroundB = Ground;
        Ground = false;

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
            CamPosTrans.localPosition = Vector3.Lerp(CamPosTrans.localPosition, PosBase * ZomeDis * 0.01f, ZomeSpeed * 0.01f);
        }
        else
        {
            CamPosTrans.localPosition = Vector3.Lerp(CamPosTrans.localPosition, PosBase, ZomeSpeed * 0.01f);
        }
        #endregion

        Sta.Rig.linearVelocity = RigVect;
    }
    private void OnCollisionStay(Collision collision)
    {
        Ground = true;
    }
}
