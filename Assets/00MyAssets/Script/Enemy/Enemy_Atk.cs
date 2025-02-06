using Photon.Pun;
using UnityEngine;
using static Statics;
public class Enemy_Atk : MonoBehaviourPun
{
    [SerializeField] State_Base Sta;
    [SerializeField] AtkAIC[] AtkAIs;
    [SerializeField] int TimerLim;
    [SerializeField] bool NoTargetResetTime;
    int timer;
    [System.Serializable]
    class AtkAIC
    {
        public Vector2Int TimeIf;
        public OtherIfsC[] OtherIfs;
        public int AtkSlot;
        public Data_Atk AtkD;
    }
    [System.Serializable]
    class OtherIfsC
    {
        public OtherIfsE Ifs;
        public Vector2 Val;
    }
    enum OtherIfsE
    {
        無=-1,
        HP割合_x以下 = 0,
        HP割合_x以上,
        ターゲット距離_x以下=10,
        ターゲット距離_x以上,
    }

    void FixedUpdate()
    {
        if (!photonView.IsMine) return;
        if (Sta.Target == null)
        {
            if(NoTargetResetTime) timer = 0;
            return;
        }
        timer++;
        if (timer > TimerLim) timer = 0;
        for(int i = 0; i < AtkAIs.Length; i++)
        {
            var AtkAI = AtkAIs[i];
            if (!V3IntTimeCheck(timer, (Vector3Int)AtkAI.TimeIf)) continue;
            bool OIf = true;
            for(int j = 0; j < AtkAI.OtherIfs.Length; j++)
            {
                var OtherIf = AtkAI.OtherIfs[j];
                float Vals;
                switch (OtherIf.Ifs)
                {
                    case OtherIfsE.HP割合_x以下:
                        if ((float)Sta.HP/ Sta.MHP > OtherIf.Val.x * 0.01f) OIf = false;
                        break;
                    case OtherIfsE.HP割合_x以上:
                        if ((float)Sta.HP / Sta.MHP > OtherIf.Val.x * 0.01f) OIf = false;
                        break;
                    case OtherIfsE.ターゲット距離_x以上:
                        Vals = Vector3.Distance(Sta.PosGet(), Sta.Target.PosGet());
                        if (Vals < OtherIf.Val.x) OIf = false;
                        break;
                    case OtherIfsE.ターゲット距離_x以下:
                        Vals = Vector3.Distance(Sta.PosGet(), Sta.Target.PosGet());
                        if (Vals > OtherIf.Val.x) OIf = false;
                        break;
                }
                if (!OIf) break;
            }
            if(!OIf) continue;
            Sta.AtkInput(AtkAI.AtkSlot, AtkAI.AtkD, true, true);
        }

    }
}
