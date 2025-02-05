using Photon.Pun;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Shot_Obj : MonoBehaviourPun
{
    public State_Base USta;
    [SerializeField] int RemTime;
    [SerializeField] bool HitRem;
    [System.NonSerialized] public Data_Atk.ShotC_Hits Hitd;
    public int Times = 0;
    public Dictionary<State_Base,int> HitList = new Dictionary<State_Base,int>();
    private void FixedUpdate()
    {
        if (!photonView.IsMine) return;
        Times++;
        if (Hitd.HitCT > 0)
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
        HitList.Add(HitState.Sta,Hitd.HitCT);
        HitState.Sta.Damage(HitPos, Hitd.Damage);
        if(HitRem)ShotDel();
    }
    public void ShotDel()
    {
        PhotonNetwork.Destroy(gameObject);
    }
}
