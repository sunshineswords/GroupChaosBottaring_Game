using Photon.Pun;
using UnityEngine;
using UnityEngine.InputSystem;

public class Player_Move : MonoBehaviourPun
{
    public Player_State PSta;
    [SerializeField] PlayerInput PI;
    [SerializeField] Transform CamRotTrans;
    [SerializeField] float CamHight;
    [SerializeField] Vector2 RotSpeed;
    [SerializeField] Vector2 RotLim;
    [SerializeField] Camera Cam;
    [SerializeField] float MoveSpeed;
    [SerializeField] float LerpSpeed;
    private void Update()
    {
        if (!photonView.IsMine) return;
        var LookInput = PI.actions["Look"].ReadValue<Vector2>();
        LookInput.x *= RotSpeed.y * 0.01f;
        LookInput.y *= -RotSpeed.x * 0.01f;
        var CamRot = CamRotTrans.eulerAngles;
        CamRot.x = Mathf.Clamp(Mathf.Repeat(CamRot.x + 180 + LookInput.y, 360f) - 180f, RotLim.x, RotLim.y);
        CamRot.y += LookInput.x;
        CamRotTrans.eulerAngles = CamRot;
        CamRotTrans.position = PSta.Rig.position + (Vector3.up * CamHight);
    }
    void FixedUpdate()
    {
        if (!photonView.IsMine) return;
        var MoveInput = PI.actions["Move"].ReadValue<Vector2>();
        var MoveVect = Cam.transform.forward * MoveInput.y + Cam.transform.right * MoveInput.x;
        MoveVect.y = 0;
        var RigVect = PSta.Rig.linearVelocity;
        PSta.Anim_MoveID = 0;
        if (MoveVect.magnitude > 0.1f)
        {
            PSta.Anim_MoveID = 1;
            RigVect += MoveVect.normalized * MoveSpeed * 0.01f;
            var LookVect = PSta.Rig.transform.forward;
            LookVect = Vector3.Slerp(LookVect.normalized, MoveVect.normalized, LerpSpeed * 0.01f);
            LookVect.y = 0;
            PSta.Rig.transform.LookAt(PSta.Rig.transform.position + LookVect);
        }

        PSta.Rig.linearVelocity = RigVect;
    }
}
