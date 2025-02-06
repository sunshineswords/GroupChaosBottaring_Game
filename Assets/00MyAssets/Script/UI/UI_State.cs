using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_State : MonoBehaviour
{
    [SerializeField] State_Base Sta;
    [SerializeField] TextMeshProUGUI NameTx;
    [SerializeField] Image HPbar;
    void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        NameTx.text = Sta.Name;
        HPbar.fillAmount = (float)Sta.HP / Mathf.Max(1, Sta.MHP);
    }
}
