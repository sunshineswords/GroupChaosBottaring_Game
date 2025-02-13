using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static DataBase;
using static PlayerValue;
public class UI_Player : MonoBehaviour
{
    [SerializeField] State_Base Sta;
    [SerializeField] Player_Cont PCont;
    [SerializeField] Player_Atk PAtk;
    [SerializeField] Image HPBackBar;
    [SerializeField] Image HPBackFill;
    [SerializeField] Image HPFrontBar;
    [SerializeField] Image HPFrontFill;
    [SerializeField] Image MPBar;
    [SerializeField] Image MPFill;
    [SerializeField] TextMeshProUGUI AtkFBTx;
    [SerializeField] UI_Sin_Atk[] AtkUIs;
    [SerializeField] TextMeshProUGUI AtkInfoTx;
    float CHPPer;
    private void Start()
    {
        CHPPer = 1f;
    }
    void LateUpdate()
    {
        float ChangeSpeed = 0.01f;
        float HPPer = Sta.HP / Mathf.Max(1, Sta.MHP);
        if(HPPer < CHPPer)
        {
            HPBackBar.fillAmount = CHPPer;
            HPBackFill.color = new Color(1f, 0.5f, 0f);
            HPFrontBar.fillAmount = HPPer;
            HPFrontFill.color = Color.green;
            CHPPer = Mathf.Max(CHPPer - ChangeSpeed,HPPer);
        }
        else
        {
            HPBackBar.fillAmount = HPPer;
            HPBackFill.color = new Color(0.5f, 1f, 0.5f); 
            HPFrontBar.fillAmount = CHPPer;
            HPFrontFill.color = Color.green;
            CHPPer = Mathf.Min(CHPPer + ChangeSpeed, HPPer);
        }
        MPBar.fillAmount = Sta.MP / Mathf.Max(1, Sta.MMP);
        MPFill.color = Sta.LowMP ? Color.red : Color.white;
        for(int i = 0; i < AtkUIs.Length; i++)
        {
            Data_Atk AtkD = null;
            bool Input = false;
            int Slot = i;
            AtkFBTx.text = !PAtk.Backs ? "表" : "裏";

            var Atks = PriSetGet.AtkGet(PAtk.Backs);
            switch (i)
            {
                case 0:
                    AtkD = DB.N_Atks[Atks.N_AtkID];
                    Input = PCont.NAtk_Stay;
                    break;
                case 1:
                    AtkD = DB.S_Atks[Atks.S1_AtkID];
                    Input = PCont.S1Atk_Stay;
                    break;
                case 2:
                    AtkD = DB.S_Atks[Atks.S2_AtkID];
                    Input = PCont.S2Atk_Stay;
                    break;
                case 3:
                    AtkD = DB.E_Atks[Atks.E_AtkID];
                    Input = PCont.EAtk_Stay;
                    Slot = 10;
                    break;
            }
            AtkUIs[i].BackImage.color = Input ? Color.yellow : Color.white;
            AtkUIs[i].Name.text = AtkD.Name;
            AtkUIs[i].Icon.texture = AtkD.Icon;
            Sta.AtkCTs.TryGetValue(Slot, out var AtkCTs);
            if (AtkCTs != null) AtkUIs[i].CTImage.fillAmount = ((float)AtkCTs.CT / Mathf.Max(1, AtkCTs.CTMax));
            else AtkUIs[i].CTImage.fillAmount = 0;
            if (AtkD.SPUse > 0)
            {
                AtkUIs[i].ChargeImage.fillAmount = (float)Sta.SP / AtkD.SPUse;
                if (Sta.SP < AtkD.SPUse) AtkUIs[i].BackImage.color = Color.gray;
            }
            else AtkUIs[i].ChargeImage.fillAmount = 0;
        }
        AtkInfoTx.text = "S" + Sta.AtkSlot;
        AtkInfoTx.text += "\nT" + Sta.AtkTime;
        AtkInfoTx.text += "\nB" + Sta.AtkBranch;

    }
}
