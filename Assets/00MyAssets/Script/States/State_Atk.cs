using Photon.Pun;

using UnityEngine;
using System.Linq;
using static Statics;
using static DataBase;
public class State_Atk
{
    static public void Branch(State_Base USta, bool Enter, bool Stay)
    {
        var AtkD = USta.AtkD;
        if (AtkD.Branchs == null) return;
        for (int i = 0; i < AtkD.Branchs.Length; i++)
        {
            var BranchD = AtkD.Branchs[i];
            bool NumCheck = false;
            for (int j = 0; j < BranchD.BranchNums.Length; j++)
            {
                if (USta.AtkBranch == BranchD.BranchNums[j]) NumCheck = true;
            }
            if (!NumCheck) continue;
            if (!V3IntTimeCheck(USta.AtkTime, (Vector3Int)BranchD.Times)) continue;
            bool Check = true;
            for (int j = 0; j < BranchD.Ifs.Length; j++)
            {
                switch (BranchD.Ifs[j])
                {
                    case Data_Atk.AtkIfE.攻撃単入力:
                        if (!Enter) Check = false;
                        break;
                    case Data_Atk.AtkIfE.攻撃長入力:
                        if (!Stay) Check = false;
                        break;
                    case Data_Atk.AtkIfE.攻撃未入力:
                        if (Enter || Stay) Check = false;
                        break;
                    case Data_Atk.AtkIfE.攻撃未長入力:
                        if (Stay) Check = false;
                        break;
                }
                if (!Check) break;
            }
            if (Check)
            {
                USta.AtkBranch = BranchD.FutureNum;
                USta.AtkTime = BranchD.FutureTime;
                return;
            }
        }
    }
    static public void Fixed(State_Base USta)
    {
        var AtkD = USta.AtkD;
        if (AtkD.Fixeds == null) return;
        for(int i = 0; i < AtkD.Fixeds.Length; i++)
        {
            var MFixed = AtkD.Fixeds[i];
            if (USta.AtkBranch != MFixed.BranchNum) continue;
            if (V3IntTimeCheck(USta.AtkTime, (Vector3Int)MFixed.Times))
            {
                USta.SpeedRem = MFixed.SpeedRem;
                USta.NoDash = MFixed.NoDash;
                USta.NoJump = MFixed.NoJump;
                USta.Aiming = MFixed.Aiming;
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
                if (USta.AtkBranch != AtFire.BranchNum) continue;
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
                    if (SObj != null)
                    {
                        SObj.USta = USta;
                        SObj.ShotD = AtShot;
                        SObj.BranchNum = USta.AtkBranch;
                    }
                    var Sta = ShotIns.GetComponent<State_Base>();
                    if (Sta != null)
                    {
                        Sta.Team = USta.Team;
                    }
                    #endregion
                }

            }
        }
    }

    static public void WeponSet(State_Base USta)
    {
        var AtkD = USta.AtkD;
        if (AtkD.WeponSets == null) return;
        for (int i = 0; i < AtkD.WeponSets.Length; i++)
        {
            var AtWep = AtkD.WeponSets[i];
            if (USta.AtkBranch != AtWep.BranchNum) continue;
            if (V3IntTimeCheck(USta.AtkTime, (Vector3Int)AtWep.Times))
            {
                USta.WeponSets.TryAdd((int)AtWep.Set, -1);
                USta.WeponPoss.TryAdd((int)AtWep.Set, Vector3.zero);
                USta.WeponRots.TryAdd((int)AtWep.Set, Vector3.zero);
                USta.WeponSets[(int)AtWep.Set] = DB.Wepons.IndexOf(AtWep.Obj);
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
            if (USta.AtkBranch != AtAnim.BranchNum) continue;
            if (V3IntTimeCheck(USta.AtkTime, (Vector3Int)AtAnim.Times))
            {
                if (AtAnim.ID != 0) USta.Anim_AtkID = AtAnim.ID;
            }
        }
    }
}
