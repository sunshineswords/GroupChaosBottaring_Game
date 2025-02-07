using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static BattleManager;
public class UI_EndResult : MonoBehaviour
{
    [SerializeField] Image BackImage;
    [SerializeField] TextMeshProUGUI WinsTx;
    [SerializeField] TextMeshProUGUI StartsTx;
    [SerializeField] TextMeshProUGUI TimeTx;
    [SerializeField] TextMeshProUGUI DeathTx;


    void Update()
    {
        BackImage.gameObject.SetActive(BTManager.End);
        if (!BTManager.End) return;
        Color BackCol;
        float Alpha = BackImage.color.a;
        if (BTManager.BossCheck)
        {
            BackCol = Color.gray;
            WinsTx.text = "勝利";
        }
        else
        {
            BackCol = Color.red;
            WinsTx.text = "敗北";
        }
        BackCol.a = Alpha;
       BackImage.color = BackCol;

        int CTime = (BTManager.TimeLimSec * 60) - BTManager.Time;
        StartsTx.text = "";
        for (int i = 0; i < 3; i++) StartsTx.text += i < BTManager.Star ? "★" : "☆";
        TimeTx.text = (CTime / 3600).ToString("D2") + ":" + (CTime / 60 % 60).ToString("D2");
        DeathTx.text = "Death:" + BTManager.DeathCount;
    }
}
