using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_State : MonoBehaviour
{
    [SerializeField] State_Base Sta;
    [SerializeField] TextMeshProUGUI NameTx;
    [SerializeField] Image HPBackBar;
    [SerializeField] Image HPBackFill;
    [SerializeField] Image HPFrontBar;
    [SerializeField] Image HPFrontFill;
    [SerializeField] Image HPChangebar;
    float CHPPer;
    private void Start()
    {
        CHPPer = 1;
    }
    void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        NameTx.text = Sta.Name;
        float HPPer= Sta.HP / Mathf.Max(1, Sta.MHP);
        Color HPCol = Color.red;
        switch (Sta.Team)
        {
            case 0:
                HPCol = Color.green;
                break;
        }
        float ChangeSpeed = 0.01f;
        if (HPPer < CHPPer)
        {
            HPBackBar.fillAmount = CHPPer;
            HPBackFill.color = new Color(1f, 0.5f, 0f);
            HPFrontBar.fillAmount = HPPer;
            HPFrontFill.color = HPCol;
            CHPPer = Mathf.Max(CHPPer - ChangeSpeed, HPPer);
        }
        else
        {
            HPBackBar.fillAmount = HPPer;
            HPBackFill.color = new Color(0.5f, 1f, 0.5f); 
            HPFrontBar.fillAmount = CHPPer;
            HPFrontFill.color = HPCol;
            CHPPer = Mathf.Min(CHPPer + ChangeSpeed, HPPer);
        }

    }
}
