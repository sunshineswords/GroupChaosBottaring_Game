using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem.HID;

public class Shot_Obj : MonoBehaviourPun
{
    public State_Base USta;
    public Rigidbody Rig;
    [SerializeField] int RemTime;
    [SerializeField] bool HitRem;
    [System.NonSerialized] public Data_Atk.ShotC_Base ShotD;
    public Data_AddShot[] DelAddShots;

    public int Times = 0;
    public int BranchNum;
    bool Dels = false;
    public Dictionary<State_Base,int> HitList = new Dictionary<State_Base,int>();
    private void FixedUpdate()
    {
        if (!photonView.IsMine) return;
        Times++;
        if (ShotD.HitCT > 0)
        {
            var HitKeys = HitList.Keys.ToArray();
            for (int i = 0; i < HitKeys.Length; i++)
            {
                HitList[HitKeys[i]]--;
                if (HitList[HitKeys[i]] <= 0) HitList.Remove(HitKeys[i]);
            }
        }
        if (Times >= RemTime) ShotDel();
    }
    public void Hits(State_Hit HitState,Vector3 HitPos)
    {
        if (HitList.ContainsKey(HitState.Sta)) return;
        HitList.Add(HitState.Sta, ShotD.HitCT);
        if(HitState.Sta.DashTime > 0)
        {
            DamageObj.DamageSet(HitPos, "Miss", Color.gray);
            return;
        }
        for(int i=0;i< ShotD.Hits.Length; i++)
        {
            var Hit = ShotD.Hits[i];
            if (BranchNum != Hit.BranchNum) continue;
            float Dam = Hit.BaseDam;
            Dam += USta.Atk * Hit.AtkDamPer * 0.01f;
            Dam += USta.Def * Hit.DefDamPer * 0.01f;
            Dam -= HitState.Sta.Def * Hit.DefRemPer * 0.01f;
            Dam *= 1f + HitState.DamAdds * 0.01f;
            if (Dam < 1) Dam = 1;
            HitState.Sta.Damage(HitPos, Mathf.RoundToInt(Dam));
            USta.SP += Hit.SPAdd;
        }
        if (HitRem) ShotDel();
    }
    public void ShotDel()
    {
        if (!Dels)
        {
            Dels = true;
            if (DelAddShots != null)
                for (int i = 0; i < DelAddShots.Length; i++)
                {
                    State_Atk.ShotAdd(USta, DelAddShots[i],Times, transform.position, transform.eulerAngles);
                }
        }

        PhotonNetwork.Destroy(gameObject);
    }
}
