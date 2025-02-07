using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static DataBase;
using static PlayerValue;
public class UI_CharaSets : MonoBehaviour,UI_Sin_Set.SetReturn
{
    [SerializeField] UI_Sin_Set[] SetSelectUIs;
    [SerializeField] List<UI_Sin_Set> Set_Sin_UIs;
    int SelectID = 0;
    void Update()
    {
        for(int i = 0; i < SetSelectUIs.Length; i++)
        {
            SetSelectUIs[i].Returns = this;
            SetSelectUIs[i].Type = "SelectChange";
            SetSelectUIs[i].BackImage.color = SetSelectUIs[i].ID == SelectID ? Color.yellow : Color.white;
        }
        var Chara_Data = DB.Charas[PSaves.CharaID];
        SetSelectUIs[0].Name.text = Chara_Data.Name;
        SetSelectUIs[0].Icon.texture = Chara_Data.Icon;
        var N_Atk_Data = DB.N_Atks[PSaves.N_AtkID];
        SetSelectUIs[1].Name.text = N_Atk_Data.Name;
        SetSelectUIs[1].Icon.texture = N_Atk_Data.Icon;
        var S1_Atk_Data = DB.S_Atks[PSaves.S1_AtkID];
        SetSelectUIs[2].Name.text = S1_Atk_Data.Name;
        SetSelectUIs[2].Icon.texture = S1_Atk_Data.Icon;
        var E_Atk_Data = DB.E_Atks[PSaves.E_AtkID];
        SetSelectUIs[3].Name.text = E_Atk_Data.Name;
        SetSelectUIs[3].Icon.texture = E_Atk_Data.Icon;
        int DataCount = 0;
        switch (SelectID)
        {
            case 0: DataCount = DB.Charas.Length; break;
            case 10: DataCount = DB.N_Atks.Length; break;
            case 11: DataCount = DB.S_Atks.Length; break;
            case 20: DataCount = DB.E_Atks.Length; break;
        }
        for (int i=0;i < Mathf.Max(DataCount,Set_Sin_UIs.Count); i++)
        {
            if (i < DataCount)
            {
                if (Set_Sin_UIs.Count <= i)
                {
                    Set_Sin_UIs.Add(Instantiate(Set_Sin_UIs[0], Set_Sin_UIs[0].transform.parent));
                }
                var SinUI = Set_Sin_UIs[i];
                SinUI.Returns = this;
                SinUI.ID = i;
                switch (SelectID)
                {
                    case 0:
                        var CData = DB.Charas[i];
                        SinUI.Type = "Chara";
                        SinUI.BackImage.color = i == PSaves.CharaID ? Color.yellow : Color.white;
                        SinUI.Name.text = CData.Name;
                        SinUI.Icon.texture = CData.Icon;
                        break;
                    case 10:
                        var NAtkData = DB.N_Atks[i];
                        SinUI.Type = "N_Atk";
                        SinUI.BackImage.color = i == PSaves.N_AtkID ? Color.yellow : Color.white;
                        SinUI.Name.text = NAtkData.Name;
                        SinUI.Icon.texture = NAtkData.Icon;
                        break;
                    case 11:
                        var SAtkData = DB.S_Atks[i];
                        SinUI.Type = "S1_Atk";
                        SinUI.BackImage.color = i == PSaves.S1_AtkID ? Color.yellow : Color.white;
                        SinUI.Name.text = SAtkData.Name;
                        SinUI.Icon.texture = SAtkData.Icon;
                        break;
                    case 20:
                        var EAtkData = DB.E_Atks[i];
                        SinUI.Type = "E_Atk";
                        SinUI.BackImage.color = i == PSaves.E_AtkID ? Color.yellow : Color.white;
                        SinUI.Name.text = EAtkData.Name;
                        SinUI.Icon.texture = EAtkData.Icon;
                        break;
                }

            }
            Set_Sin_UIs[i].gameObject.SetActive(i < DataCount);
        }
    }
    void UI_Sin_Set.SetReturn.ReturnID(string Type, int ID)
    {
        switch (Type)
        {
            case "SelectChange":
                SelectID = ID;
                break;
            case "Chara":
                PSaves.CharaID = ID;
                break;
            case "N_Atk":
                PSaves.N_AtkID = ID;
                break;
            case "S1_Atk":
                PSaves.S1_AtkID = ID;
                break;
            case "E_Atk":
                PSaves.E_AtkID = ID;
                break;
        }
    }
    public void Saves()
    {
        Save();
    }
}
