using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static Statics;
using static Manifesto;
using static PlayerValue;
using static Calculation;
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
    public int SPAddCT = 0;
    public int BranchNum;
    bool Dels = false;
    public Dictionary<State_Base,int> HitList = new Dictionary<State_Base,int>();
    private void Start()
    {
        Times = 0;
        SPAddCT = 0;
    }
    private void FixedUpdate()
    {
        if (!photonView.IsMine) return;
        if (USta == null) ShotDel();
        Times++;
        SPAddCT--;
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
    public void ShotDel()
    {
        if (!Dels)
        {
            Dels = true;
            if (DelAddShots != null && USta != null)
                for (int i = 0; i < DelAddShots.Length; i++)
                {
                    State_Atk.ShotAdd(USta, BranchNum, DelAddShots[i], Times, transform.position, transform.eulerAngles);
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
                if (SepParticles[i] == null) continue;
                SepParticles[i].transform.parent = null;
                var ParMain = SepParticles[i].main;
                ParMain.loop = false;
                ParMain.stopAction = ParticleSystemStopAction.Destroy;
            }
        if (SepTrails != null)
            for (int i = 0; i < SepTrails.Length; i++)
            {
                if (SepTrails[i] == null) continue;
                SepTrails[i].transform.parent = null;
                SepTrails[i].autodestruct = true;
            }

    }
    public void Hits(State_Hit HitState,Vector3 HitPos)
    {
        if (HitState.Sta.NoDamage) return;
        if (HitList.ContainsKey(HitState.Sta)) return;
        HitList.Add(HitState.Sta, ShotD.HitCT);
        bool HitCh = false;
        for(int i=0;i< ShotD.Hits.Length; i++)
        {
            var Hit = ShotD.Hits[i];
            if (Hit.BranchNum >= 0 && BranchNum != Hit.BranchNum) continue;
            if (!TeamCheck(USta, HitState.Sta, Hit.EHit, Hit.FHit, Hit.MHit)) continue;
            if (HitState.Sta.DashTime > 0)
            {
                DamageObj.DamageSet(HitPos, "Miss", Color.gray);
                return;
            }
            HitCh = true;
            int Damage = DamSets(HitState, Hit);
            USta.AddInfoAdd(Damage);
            HitState.Sta.Damage(HitPos, Damage,Hit.BreakValue);
            if (Hit.BufSets!=null)
            for (int j = 0; j < Hit.BufSets.Length; j++) HitState.Sta.BufSets(Hit.BufSets[j],USta);
            if (SPAddCT <= 0)
            {
                if (USta.Player)USta.SP += Hit.SPAdd * 1f + PriSetGet.PassiveLVGet(Enum_Passive.SPブースト) * 0.25f;
                else USta.SP += Hit.SPAdd;
            }
            if(Damage>0) USta.HitEvents(HitState.Sta,HitPos,Hit.DamageType,Hit.ShortAtk);
        }
        if(HitCh) SPAddCT = ShotD.HitCT <= 0 ? RemTime : ShotD.HitCT;
        if (HitRem && HitCh) ShotDel();
    }
    int DamSets(State_Hit HitState, Class_Atk_Shot_Hit AtkHit)
    {
        float Dam = (float)Cal(AtkHit.DamCalc, USta, HitState.Sta);
        Dam *= 1f + HitState.DamAdds * 0.01f;
        if (AtkHit.DamCalc != "" && Dam < 1) Dam = 1;
        float DamAdd = 0;
        if (USta.Player)
        {
            switch (AtkHit.DamageType)
            {
                case Enum_DamageType.通常:
                    DamAdd += PriSetGet.PassiveLVGet(Enum_Passive.メイン強化) * 15;
                    DamAdd += PriSetGet.PassiveLVGet(Enum_Passive.通常強化) * 20;
                    break;
                case Enum_DamageType.重撃:
                    DamAdd += PriSetGet.PassiveLVGet(Enum_Passive.メイン強化) * 15;
                    DamAdd += PriSetGet.PassiveLVGet(Enum_Passive.重落強化) * 25;
                    break;
                case Enum_DamageType.落下:
                    DamAdd += PriSetGet.PassiveLVGet(Enum_Passive.メイン強化) * 15;
                    DamAdd += PriSetGet.PassiveLVGet(Enum_Passive.重落強化) * 25;
                    break;
                case Enum_DamageType.スキル:
                    DamAdd += PriSetGet.PassiveLVGet(Enum_Passive.スキル強化) * 25;
                    break;
                case Enum_DamageType.必殺:
                    DamAdd += PriSetGet.PassiveLVGet(Enum_Passive.必殺強化) * 30;
                    break;
            }
            if(AtkHit.ShortAtk) DamAdd += PriSetGet.PassiveLVGet(Enum_Passive.近距離強化) * 25;
            else DamAdd += PriSetGet.PassiveLVGet(Enum_Passive.遠距離強化) * 15;
        }
        DamAdd += USta.BufPowGet(Enum_Bufs.与ダメージ増加) * 1;
        if (AtkHit.ShortAtk) DamAdd += USta.BufPowGet(Enum_Bufs.近距離強化) * 1;
        else DamAdd += USta.BufPowGet(Enum_Bufs.遠距離強化) * 1;
        Dam *= 1f + DamAdd * 0.01f;
        if (AtkHit.DamCalc != "" && Dam < 1) Dam = 1;
        if (AtkHit.Heals && USta.PLValues!=null) USta.PLValues.AddHeal += Dam;
        return Mathf.RoundToInt(Dam) * (AtkHit.Heals ? -1 : 1);
    }
}
