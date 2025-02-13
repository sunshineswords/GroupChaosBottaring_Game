using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_State : MonoBehaviour
{
    [SerializeField] State_Base Sta;
    [SerializeField] TextMeshProUGUI NameTx;
    [SerializeField] Image HPbar;
    [SerializeField] Image BarFill;
    void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        NameTx.text = Sta.Name;
        HPbar.fillAmount = (float)Sta.HP / Mathf.Max(1, Sta.MHP);
        switch (Sta.Team)
        {
            case 0:
                BarFill.color = Color.green;
                break;
            default:
                BarFill.color = Color.red;
                break;
        }
    }
}
