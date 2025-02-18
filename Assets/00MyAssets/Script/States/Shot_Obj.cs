using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Statics;
using static Manifesto;
public class Shot_Obj : MonoBehaviourPun
{
    public State_Base USta;
    public Rigidbody Rig;
    [SerializeField] int RemTime;
    [SerializeField] bool HitRem;
    [System.NonSerialized] public Class_Atk_Shot_Base ShotD;
    public Data_AddShot[] DelAddShots;
    public ParticleSystem[] SepParticles;
    public TrailRenderer[] SepTrails;

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
        bool HitCh = false;
        for(int i=0;i< ShotD.Hits.Length; i++)
        {
            var Hit = ShotD.Hits[i];
            if (BranchNum != Hit.BranchNum) continue;
            if (!TeamCheck(USta, HitState.Sta, Hit.EHit, Hit.FHit, Hit.MHit)) continue;
            if (HitState.Sta.DashTime > 0)
            {
                DamageObj.DamageSet(HitPos, "Miss", Color.gray);
                return;
            }
            HitCh = true;
            float Dam = Hit.BaseDam;
            Dam += USta.MHP * Hit.MHPDamPer * 0.01f;
            Dam += USta.HP * Hit.HPDamPer * 0.01f;
            Dam += USta.FAtk * Hit.AtkDamPer * 0.01f;
            Dam += USta.FDef * Hit.DefDamPer * 0.01f;
            Dam -= HitState.Sta.FDef * Hit.DefRemPer * 0.01f;
            Dam *= 1f + HitState.DamAdds * 0.01f;
            if (Dam < 1) Dam = 1;
            HitState.Sta.Damage(HitPos, Mathf.RoundToInt(Dam) * (Hit.Heals ? -1 : 1));
            if(Hit.BufSets!=null)
            for (int j = 0; j < Hit.BufSets.Length; j++) HitState.Sta.BufSets(Hit.BufSets[j]);
            USta.SP += Hit.SPAdd;
        }
        if (HitRem && HitCh) ShotDel();
    }
    public void ShotDel()
    {
        if (!Dels)
        {
            Dels = true;
            if (DelAddShots != null)
                for (int i = 0; i < DelAddShots.Length; i++)
                {
                    State_Atk.ShotAdd(USta, DelAddShots[i], Times, transform.position, transform.eulerAngles);
                    State_Atk.SEPlayAdd(DelAddShots[i], transform.position);
                }
            photonView.RPC(nameof(RPC_SepObj), RpcTarget.All);
        }
        PhotonNetwork.Destroy(gameObject);
    }
    [PunRPC]
    void RPC_SepObj()
    {
        if (SepParticles != null)
            for (int i = 0; i < SepParticles.Length; i++)
            {
                SepParticles[i].transform.parent = null;
                var ParMain = SepParticles[i].main;
                ParMain.loop = false;
                ParMain.stopAction = ParticleSystemStopAction.Destroy;
            }
        if(SepTrails!=null)
            for (int i = 0; i < SepTrails.Length; i++)
            {
                SepTrails[i].transform.parent = null;
                SepTrails[i].autodestruct = true;
            }

    }
}
