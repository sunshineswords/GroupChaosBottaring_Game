using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Sin_EnemyUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI NameTx;
    [SerializeField] Image HPBar;

    public void UISet(Enemy_State ESta)
    {
        NameTx.text = ESta.Name;
        HPBar.fillAmount = (float)ESta.HP / Mathf.Max(1, ESta.MHP);
    }
}
