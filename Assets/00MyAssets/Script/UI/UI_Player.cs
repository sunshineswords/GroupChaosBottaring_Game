using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static DataBase;
using static PlayerValue;
public class UI_Player : MonoBehaviour
{
    [SerializeField] State_Base Sta;
    [SerializeField] Player_Cont PCont;
    [SerializeField] Image HPBar;
    [SerializeField] UI_Sin_Atk[] AtkUIs;
    [SerializeField] TextMeshProUGUI AtkInfoTx;

    void LateUpdate()
    {
        HPBar.fillAmount = (float)Sta.HP / Mathf.Max(1, Sta.MHP);

        for(int i = 0; i < AtkUIs.Length; i++)
        {
            Data_Atk AtkD = null;
            bool Input = false;
            int Slot = i;
            switch (i)
            {
                case 0:
                    AtkD = DB.N_Atks[PSaves.N_AtkID];
                    Input = PCont.NAtk_Stay;
                    break;
                case 1:
                    AtkD = DB.S_Atks[PSaves.S1_AtkID];
                    Input = PCont.S1Atk_Stay;
                    break;
                case 2:
                    AtkD = DB.E_Atks[PSaves.E_AtkID];
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
