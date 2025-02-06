using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static BattleManager;
public class UI_BattleBase : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI TimeTx;
    [SerializeField] TextMeshProUGUI DeathTx;
    [SerializeField] List<UI_Sin_BossUI> EnemyUIs;

    void LateUpdate()
    {
        TimeTx.text = (BTManager.Time / 3600).ToString("D2") + ":" + (BTManager.Time / 60 % 60).ToString("D2");
        DeathTx.text = "Death:" + BTManager.DeathCount;

        for(int i = 0; i < BTManager.BossList.Count; i++)
        {
            if(EnemyUIs.Count <= i)EnemyUIs.Add(Instantiate(EnemyUIs[0], EnemyUIs[0].transform.parent));
            EnemyUIs[i].UISet(BTManager.BossList[i]);
        }
    }
}
