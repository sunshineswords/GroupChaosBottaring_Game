using UnityEngine;
using static BattleManager;
using static Statics;
public class Player_Target : MonoBehaviour
{
    [SerializeField] Player_Cont PCont;
    [SerializeField] State_Base Sta;
    [SerializeField] Camera Cam;
    [SerializeField] int TargetTimes;
    [SerializeField] GameObject TargetObj;
    int StayTime;

    void FixedUpdate()
    {
        if (PCont.Target_Stay)
        {
            StayTime++;
            if (StayTime >= TargetTimes)
            {
                Sta.TargetHit = null;
                float NearDis = -1;
                foreach (var Hits in BTManager.HitList)
                {
                    if (!TeamCheck(Sta, Hits.Sta)) continue;
                    var CPos = Cam.WorldToViewportPoint(Hits.PosGet());
                    if (CPos.z <= 0) continue;
                    CPos.x -= 0.5f;
                    CPos.y -= 0.5f;
                    float Dis = new Vector2(CPos.x, CPos.y).magnitude;
                    if (NearDis < 0 || NearDis > Dis)
                    {
                        NearDis = Dis;
                        Sta.TargetHit = Hits;
                    }
                }
            }
        }
        else if (StayTime > 0)
        {
            if (StayTime < TargetTimes)
            {
                Sta.TargetHit = null;
            }
            StayTime = 0;
        }
        if(Sta.TargetHit!=null && Sta.TargetHit.Sta.HP<=0)Sta.TargetHit = null;
    }
    private void LateUpdate()
    {
        if (Sta.TargetHit != null)
        {
            TargetObj.SetActive(true);
            TargetObj.transform.position = Sta.TargetHit.PosGet();
        }
        else TargetObj.SetActive(false);
    }
}
