using Photon.Pun;

using UnityEngine;
using static Statics;
public class State_Atk
{
    static public void MoveFixed(State_Base USta)
    {
        var AtkD = USta.AtkD;
        if (AtkD.Move_Fixeds == null) return;
        for(int i = 0; i < AtkD.Move_Fixeds.Length; i++)
        {
            var MFixed = AtkD.Move_Fixeds[i];
            if (V3IntTimeCheck(USta.AtkTime, (Vector3Int)MFixed.Times))
            {
                USta.SpeedRem = MFixed.SpeedRem;
                USta.NoDash = MFixed.NoDash;
                USta.NoJump = MFixed.NoJump;
            }
        }
    }
    static public void Shot(State_Base USta, Vector3 CharaPos, Vector3 CharaRot,Vector3 CamRot)
    {
        var AtkD = USta.AtkD;
        if (AtkD.Shots == null) return;
        for (int i = 0; i < AtkD.Shots.Length; i++)
        {
            var AtShot = AtkD.Shots[i];
            for (int j = 0; j < AtShot.Fires.Length; j++)
            {
                var AtFire = AtShot.Fires[j];
                if (!V3IntTimeCheck(USta.AtkTime, AtFire.Times)) continue;

                for (int k = 0; k < AtFire.Count; k++)
                {
                    float WaySet = k - ((AtFire.Count - 1) / 2f);
                    var Pos = CharaPos;
                    #region 座標設定
                    switch (AtFire.Trans.PosBase)
                    {
                        case Data_Atk.PosBaseE.ターゲット位置:
                            if (USta.Target != null) Pos = USta.Target.PosGet();
                            break;
                    }
                    Pos += Quaternion.Euler(CharaRot) * AtFire.Trans.PosChange;
                    Pos += Quaternion.Euler(CharaRot) * AtFire.Trans.PosWay * WaySet;
                    Pos += Quaternion.Euler(CharaRot) * AtFire.Trans.PosTime * USta.AtkTime;
                    Vector3 RPos;
                    RPos.x = Float_NegRand(AtFire.Trans.PosRand.x);
                    RPos.y = Float_NegRand(AtFire.Trans.PosRand.y);
                    RPos.z = Float_NegRand(AtFire.Trans.PosRand.z);
                    Pos += Quaternion.Euler(CharaRot) * RPos;
                    #endregion
                    var Rot = CharaRot;
                    #region 角度設定
                    switch (AtFire.Trans.RotBase)
                    {
                        case Data_Atk.RotBaseE.固定: Rot = Vector3.zero; break;
                        case Data_Atk.RotBaseE.ターゲット方向:
                            if (USta.Target != null)
                            {
                                var TVect = USta.Target.PosGet() - Pos;
                                Rot = Quaternion.LookRotation(Vector3.forward, TVect).eulerAngles;
                            }
                            break;
                        case Data_Atk.RotBaseE.使用者カメラ方向: Rot = CamRot; break;
                    }
                    Rot += AtFire.Trans.RotChange;
                    Rot += AtFire.Trans.RotWay * WaySet;
                    Rot += AtFire.Trans.RotTime * USta.AtkTime;
                    Rot.x += Float_NegRand(AtFire.Trans.RotRand.x);
                    Rot.y += Float_NegRand(AtFire.Trans.RotRand.y);
                    Rot.z += Float_NegRand(AtFire.Trans.RotRand.z);
                    #endregion

                    #region 発射
                    var ShotIns = PhotonNetwork.Instantiate(AtShot.Obj.name, Pos, Quaternion.Euler(Rot));
                    var ShotRig = ShotIns.GetComponent<Rigidbody>();
                    ShotRig.linearVelocity += ShotIns.transform.forward * V2_Rand_Float(AtFire.Speed)*0.01f;
                    var SObj = ShotIns.GetComponent<Shot_Obj>();
                    SObj.USta = USta;
                    SObj.Hitd = AtShot.Hits;
                    #endregion
                }

            }
        }
    }
    static public void Anim(State_Base USta)
    {
        var AtkD = USta.AtkD;
        if (AtkD.Anims == null) return;
        for (int i = 0; i < AtkD.Anims.Length; i++)
        {
            var AtAnim = AtkD.Anims[i];
            if (V3IntTimeCheck(USta.AtkTime, (Vector3Int)AtAnim.Times))
            {
                if (AtAnim.ID != 0) USta.Anim_AtkID = AtAnim.ID;
            }
        }
    }
}
