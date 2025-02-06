using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Sin_BossUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI NameTx;
    [SerializeField] Image HPBar;

    public void UISet(State_Base Sta)
    {
        NameTx.text = Sta.Name;
        HPBar.fillAmount = (float)Sta.HP / Mathf.Max(1, Sta.MHP);
    }
}
