using Photon.Pun;

using UnityEngine;
using static Statics;
public class State_Atk
{
    static public void Shot(State_Base State, Data_Atk AtkD, Vector3 Pos, Vector3 Rot)
    {
        for (int i = 0; i < AtkD.Shots.Length; i++)
        {
            var AtShot = AtkD.Shots[i];
            for (int j = 0; j < AtShot.Fires.Length; j++)
            {
                var AtFire = AtShot.Fires[j];
                if (!V3IntTimeCheck(State.AtkTime, AtFire.Times)) continue;

                for (int k = 0; k < AtFire.Count; k++)
                {
                    var ShotIns = PhotonNetwork.Instantiate(AtShot.Obj.name, Pos, Quaternion.Euler(Rot));
                    var ShotRig = ShotIns.GetComponent<Rigidbody>();
                    ShotRig.linearVelocity += ShotIns.transform.forward * V2_Rand_Float(AtFire.Speed)*0.01f;
                    var SObj = ShotIns.GetComponent<Shot_Obj>();
                    SObj.USta = State;
                    SObj.Hitd = AtShot.Hits;
                }

            }
        }
    }
    static public void Anim(State_Base State, Data_Atk AtkD)
    {
        for (int i = 0; i < AtkD.Anims.Length; i++)
        {
            var AtAnim = AtkD.Anims[i];
            if (AtAnim.Times.x <= State.AtkTime && State.AtkTime <= AtAnim.Times.y)
            {
                if (AtAnim.ID != 0) State.Anim_AtkID = AtAnim.ID;
            }
        }
    }
}
