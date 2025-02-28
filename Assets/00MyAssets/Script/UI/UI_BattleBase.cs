using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;
using static BattleManager;
public class UI_BattleBase : MonoBehaviour
{
    [SerializeField] GameObject InfoUI;
    [SerializeField] TextMeshProUGUI TimeTx;
    [SerializeField] TextMeshProUGUI DeathTx;
    [SerializeField] List<UI_Sin_BossUI> BossUIs;

    [SerializeField] TextMeshProUGUI TimeStarTx;
    [SerializeField] TextMeshProUGUI DeathStarTx;

    void LateUpdate()
    {
        if (BTManager.TimeLimSec > 0)
        {
            InfoUI.SetActive(true);
            TimeTx.text = (BTManager.Time / 3600).ToString("D2") + ":" + (BTManager.Time / 60 % 60).ToString("D2");
            DeathTx.text = "Death:" + BTManager.DeathCount;

            TimeStarTx.text = (BTManager.Time >= BTManager.TimeStar * 60 ? "★" : "☆");
            TimeStarTx.text += (BTManager.TimeStar / 60).ToString("D2") + ":" + (BTManager.TimeStar % 60).ToString("D2");

            DeathStarTx.text = (BTManager.DeathCount <= BTManager.DeathStar ? "★" : "☆");
            DeathStarTx.text += "Death:" + BTManager.DeathStar;
        }
        else InfoUI.SetActive(false);
        for (int i = 0; i < Mathf.Max(BossUIs.Count, BTManager.BossList.Count); i++)
        {
            if (i < BTManager.BossList.Count)
            {
                if (BossUIs.Count <= i) BossUIs.Add(Instantiate(BossUIs[0], BossUIs[0].transform.parent));
                var Sta = BTManager.BossList[i];
                var BUI = BossUIs[i];
                BUI.Sta = Sta;
                BUI.BaseSet();
            }
            BossUIs[i].gameObject.SetActive(i < BTManager.BossList.Count);
        }

    }
}
