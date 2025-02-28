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
    int StayTime = 0;
    int AutoTime = 0;
    void FixedUpdate()
    {
        if (PCont.Target_Stay)
        {
            StayTime++;
            if (StayTime >= TargetTimes)
            {
                Sta.TargetHit = null;
                TargetSet();
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
        AutoTime--;
        if (Sta.TargetHit != null && Sta.TargetHit.Sta.HP <= 0)
        {
            AutoTime = 30;
        }
        if (AutoTime > 0)
        {
            Sta.TargetHit = null;
            TargetSet();
            if (Sta.TargetHit != null) AutoTime = 0;
        }
    }
    void TargetSet()
    {
        float NearDis = -1;
        foreach (var Hits in BTManager.HitList)
        {
            if (!TeamCheck(Sta, Hits.Sta)) continue;
            var CPos = Cam.WorldToViewportPoint(Hits.PosGet());
            if (CPos.z <= 0) continue;
            if (CPos.x < 0f || CPos.x > 1f) continue;
            if (CPos.y < 0f || CPos.y > 1f) continue;
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
