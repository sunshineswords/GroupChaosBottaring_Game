using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static BattleManager;
public class UI_BattleBase : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI TimeTx;
    [SerializeField] TextMeshProUGUI DeathTx;
    [SerializeField] List<UI_Sin_BossUI> BossUIs;

    [SerializeField] TextMeshProUGUI TimeStarTx;
    [SerializeField] TextMeshProUGUI DeathStarTx;

    void LateUpdate()
    {
        TimeTx.text = (BTManager.Time / 3600).ToString("D2") + ":" + (BTManager.Time / 60 % 60).ToString("D2");
        DeathTx.text = "Death:" + BTManager.DeathCount;

        TimeStarTx.text = (BTManager.Time >= BTManager.TimeStar * 60 ? "★" : "☆");
        TimeStarTx.text += (BTManager.TimeStar / 60).ToString("D2") + ":" + (BTManager.TimeStar % 60).ToString("D2");

        DeathStarTx.text = (BTManager.DeathCount < BTManager.DeathStar ? "★" : "☆");
        DeathStarTx.text += "Death:" + BTManager.DeathStar;

        for (int i = 0; i < Mathf.Max(BossUIs.Count, BTManager.BossList.Count); i++)
        {
            if (i < BTManager.BossList.Count)
            {
                if (BossUIs.Count <= i) BossUIs.Add(Instantiate(BossUIs[0], BossUIs[0].transform.parent));
                var Sta = BTManager.BossList[i];
                var BUI = BossUIs[i];
                BUI.NameTx.text = Sta.Name;
                float HPPer = Sta.HP / Mathf.Max(1, Sta.MHP);
                var HPCol = Color.red;
                switch (Sta.Team)
                {
                    case 0:
                        HPCol = Color.green;
                        break;
                }
                float ChangeSpeed = 0.01f;
                if (HPPer < BUI.CHPPer)
                {
                    BUI.HPFrontBar.fillAmount = HPPer;
                    BUI.HPFrontFill.color = HPCol;
                    BUI.HPBackBar.fillAmount = BUI.CHPPer;
                    BUI.HPBackFill.color = new Color(1f, 0.5f, 0);
                    BUI.CHPPer = Mathf.Max(BUI.CHPPer - ChangeSpeed, HPPer);
                }
                else
                {
                    BUI.HPFrontBar.fillAmount = BUI.CHPPer;
                    BUI.HPFrontFill.color = HPCol;
                    BUI.HPBackBar.fillAmount = HPPer;
                    BUI.HPBackFill.color = new Color(0.5f, 1f, 0.5f); 
                    BUI.CHPPer = Mathf.Min(BUI.CHPPer + ChangeSpeed, HPPer);
                }


            }
            BossUIs[i].gameObject.SetActive(i < BTManager.BossList.Count);
        }

    }
}
