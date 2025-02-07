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
    [SerializeField] List<UI_Sin_BossUI> EnemyUIs;

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

        for (int i = 0; i < Mathf.Max(EnemyUIs.Count, BTManager.BossList.Count); i++)
        {
            if (i < BTManager.BossList.Count)
            {
                if (EnemyUIs.Count <= i) EnemyUIs.Add(Instantiate(EnemyUIs[0], EnemyUIs[0].transform.parent));
                var Sta = BTManager.BossList[i];
                EnemyUIs[i].NameTx.text = Sta.Name;
                EnemyUIs[i].HPBar.fillAmount = (float)Sta.HP / Mathf.Max(1, Sta.MHP);
            }
            EnemyUIs[i].gameObject.SetActive(i < BTManager.BossList.Count);
        }

    }
}
