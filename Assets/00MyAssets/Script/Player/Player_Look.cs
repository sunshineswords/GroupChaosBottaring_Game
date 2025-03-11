using Photon.Pun;
using UnityEngine;
using static PlayerValue;
using static Statics;
using static DataBase;
public class Player_Look : MonoBehaviour
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
    [SerializeField] float TRotPer;

    Vector3 PosBase;

    void Start()
    {
        PosBase = CamPosTrans.localPosition;
    }

    // Update is called once per frame
    void Update()
    {
        var CamRot = CamRotTrans.eulerAngles;
        var LookInput = PCont.Look * PSaves.CamSpeed;
        if (Sta.TargetHit != null && LookInput.magnitude < 0.1f && !Sta.Aiming)
        {
            var CamFront = CamRotTrans.forward;
            var LookPos = Sta.TargetHit.PosGet();
            var CamTDis = HoriDistance(CamRotTrans.position, Sta.TargetHit.PosGet());
            var DisC = 1f - Mathf.Clamp01(CamTDis / (PosBase.magnitude * 2));
            LookPos.y = Mathf.Lerp(LookPos.y, CamRotTrans.position.y, DisC);
            var CamLook = LookPos - CamRotTrans.position;
            var CamVect = Vector3.Slerp(CamFront.normalized, CamLook.normalized, TRotPer * 0.01f * PSaves.TargetSpeed);
            CamRot = Quaternion.LookRotation(CamVect, Vector3.forward).eulerAngles;
        }
        LookInput.x *= RotSpeed.y * 0.01f;
        LookInput.y *= -RotSpeed.x * 0.01f;
        CamRot.x += LookInput.y;
        CamRot.y += LookInput.x;
        CamRot.x = Mathf.Clamp(Mathf.Repeat(CamRot.x + 180, 360f) - 180f, RotLim.x, RotLim.y);
        CamRot.z = 0;
        CamRotTrans.eulerAngles = CamRot;
        CamRotTrans.position = Sta.Rig.position + (Vector3.up * CamHight);
        if (Sta.AtkD != null && Sta.Aiming)
        {
            CamPosTrans.localPosition = Vector3.Lerp(CamPosTrans.localPosition, PosBase * ZomeDis * 0.01f, ZomeSpeed * 0.01f);

        }
        else
        {
            float CamDis = PosBase.magnitude;
            foreach (var RayHit in Physics.SphereCastAll(CamRotTrans.position, 0.2f, CamPosTrans.position - CamRotTrans.position, CamDis, DB.CamLayer))
            {
                if (CamDis > RayHit.distance) CamDis = RayHit.distance;
            }
            CamPosTrans.localPosition = Vector3.Lerp(CamPosTrans.localPosition, PosBase.normalized * CamDis, ZomeSpeed * 0.01f);
        }
    }
}
