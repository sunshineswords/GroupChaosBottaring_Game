using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static DataBase;
using static PlayerValue;
public class UI_CharaSets : MonoBehaviour,UI_Sin_Set.SetReturn
{
    [SerializeField] List<UI_Sin_Set> PriSet_Sin_UIs;
    //[SerializeField] Toggle FBToggle;
    [SerializeField] TextMeshProUGUI InfoTx;
    [SerializeField] UI_Sin_Set[] TypeUIs;
    [SerializeField] GameObject AtkUIs;
    [SerializeField] GameObject PassiveUIs;
    [SerializeField] UI_Sin_Set[] SetSelectUIs;
    [SerializeField] List<UI_Sin_Set> Set_Sin_UIs;
    int TypeID = 0;
    int SelectID = 0;
    void Update()
    {


        for (int i = 0; i < PriSets.Length; i++)
        {
            if (PriSet_Sin_UIs.Count <= i) PriSet_Sin_UIs.Add(Instantiate(PriSet_Sin_UIs[0], PriSet_Sin_UIs[0].transform.parent));
            var SinUI = PriSet_Sin_UIs[i];
            SinUI.Returns = this;
            SinUI.Type = "PriSet";
            SinUI.BackImage.color = PSaves.PriSetID == i ? Color.yellow : Color.white;
            SinUI.ID = i;
            SinUI.Name.text = (i + 1).ToString("");
        }
        for (int i = 0; i < TypeUIs.Length; i++)
        {
            TypeUIs[i].Returns = this;
            TypeUIs[i].Type = "TypeChange";
            TypeUIs[i].BackImage.color = TypeUIs[i].ID == TypeID ? Color.yellow : Color.white;
        }
        AtkUIs.gameObject.SetActive(TypeID <= 1);
        PassiveUIs.gameObject.SetActive(TypeID == 10);
        for (int i = 0; i < SetSelectUIs.Length; i++)
        {
            SetSelectUIs[i].Returns = this;
            SetSelectUIs[i].Type = "SelectChange";
            SetSelectUIs[i].BackImage.color = SetSelectUIs[i].ID == SelectID ? Color.yellow : Color.white;
        }
        var Atks = PriSetGet.AtkGet(TypeID == 1);

        var Chara_Data = DB.Charas[PriSetGet.CharaID];
        SetSelectUIs[0].Name.text = Chara_Data.Name;
        SetSelectUIs[0].Icon.texture = Chara_Data.Icon;
        var N_Atk_Data = DB.N_Atks[Atks.N_AtkID];
        SetSelectUIs[1].Name.text = N_Atk_Data.Name;
        SetSelectUIs[1].Icon.texture = N_Atk_Data.Icon;
        var S1_Atk_Data = DB.S_Atks[Atks.S1_AtkID];
        SetSelectUIs[2].Name.text = S1_Atk_Data.Name;
        SetSelectUIs[2].Icon.texture = S1_Atk_Data.Icon;
        var S2_Atk_Data = DB.S_Atks[Atks.S2_AtkID];
        SetSelectUIs[3].Name.text = S2_Atk_Data.Name;
        SetSelectUIs[3].Icon.texture = S2_Atk_Data.Icon;
        var E_Atk_Data = DB.E_Atks[Atks.E_AtkID];
        SetSelectUIs[4].Name.text = E_Atk_Data.Name;
        SetSelectUIs[4].Icon.texture = E_Atk_Data.Icon;
        for (int i = 5; i < 5 + 4; i++)
        {
            int PassID = -1;
            switch (i)
            {
                case 5: PassID = PriSetGet.Passive.P1_ID; break;
                case 6: PassID = PriSetGet.Passive.P2_ID; break;
                case 7: PassID = PriSetGet.Passive.P3_ID; break;
                case 8: PassID = PriSetGet.Passive.P4_ID; break;
            }
            var Pass_Data = DB.Passives[PassID];
            SetSelectUIs[i].Name.text = Pass_Data.Name;
            SetSelectUIs[i].Icon.texture = Pass_Data.Icon;
        }
        int DataCount = 0;
        InfoTx.text = "";
        switch (SelectID)
        {
            case 0:
                DataCount = DB.Charas.Length;
                InfoTx.text = Chara_Data.Name;
                break;
            case 10:
                DataCount = DB.N_Atks.Length;
                InfoTx.text = N_Atk_Data.Name;
                InfoTx.text += "\n" + N_Atk_Data.InfoGets();
                break;
            case 11:
            case 12:
                DataCount = DB.S_Atks.Length;
                if (SelectID == 11)
                {
                    InfoTx.text = S1_Atk_Data.Name;
                    InfoTx.text += "\n" + S1_Atk_Data.InfoGets();
                }
                else
                {
                    InfoTx.text = S2_Atk_Data.Name;
                    InfoTx.text += "\n" + S2_Atk_Data.InfoGets();
                }
                break;
            case 20: 
                DataCount = DB.E_Atks.Length;
                InfoTx.text = E_Atk_Data.Name;
                InfoTx.text += "\n" + E_Atk_Data.InfoGets();
                break;
            case 30:
            case 31:
            case 32:
            case 33:
                DataCount = DB.Passives.Length;
                Data_Passive PassD=null;
                switch (SelectID)
                {
                    case 30:
                        PassD = DB.Passives[PriSetGet.Passive.P1_ID];
                        break;
                    case 31:
                        PassD = DB.Passives[PriSetGet.Passive.P2_ID];
                        break;
                    case 32:
                        PassD = DB.Passives[PriSetGet.Passive.P3_ID];
                        break;
                    case 33:
                        PassD = DB.Passives[PriSetGet.Passive.P4_ID];
                        break;
                }
                InfoTx.text = PassD.Name;
                InfoTx.text += "\n" + PassD.Info;
                break;
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
                        SinUI.BackImage.color = i == PriSetGet.CharaID ? Color.yellow : Color.white;
                        SinUI.Name.text = CData.Name;
                        SinUI.Icon.texture = CData.Icon;
                        break;
                    case 10:
                        var NAtkData = DB.N_Atks[i];
                        SinUI.Type = "N_Atk";
                        SinUI.BackImage.color = i == Atks.N_AtkID ? Color.yellow : Color.white;
                        SinUI.Name.text = NAtkData.Name;
                        SinUI.Icon.texture = NAtkData.Icon;
                        break;
                    case 11:
                    case 12:
                        var SAtkData = DB.S_Atks[i];
                        if(SelectID == 11)
                        {
                            SinUI.Type = "S1_Atk";
                            SinUI.BackImage.color = i == Atks.S1_AtkID ? Color.yellow : Color.white;
                        }
                        else
                        {
                            SinUI.Type = "S2_Atk";
                            SinUI.BackImage.color = i == Atks.S2_AtkID ? Color.yellow : Color.white;
                        }
                        SinUI.Name.text = SAtkData.Name;
                        SinUI.Icon.texture = SAtkData.Icon;
                        break;
                    case 20:
                        var EAtkData = DB.E_Atks[i];
                        SinUI.Type = "E_Atk";
                        SinUI.BackImage.color = i == Atks.E_AtkID ? Color.yellow : Color.white;
                        SinUI.Name.text = EAtkData.Name;
                        SinUI.Icon.texture = EAtkData.Icon;
                        break;
                    case 30:
                    case 31:
                    case 32:
                    case 33:
                        var PassData = DB.Passives[i];
                        switch (SelectID)
                        {
                            case 30:
                                SinUI.Type = "P1";
                                SinUI.BackImage.color = i == PriSetGet.Passive.P1_ID ? Color.yellow : Color.white;
                                break;
                            case 31:
                                SinUI.Type = "P2";
                                SinUI.BackImage.color = i == PriSetGet.Passive.P2_ID ? Color.yellow : Color.white;
                                break;
                            case 32:
                                SinUI.Type = "P3";
                                SinUI.BackImage.color = i == PriSetGet.Passive.P3_ID ? Color.yellow : Color.white;
                                break;
                            case 33:
                                SinUI.Type = "P4";
                                SinUI.BackImage.color = i == PriSetGet.Passive.P4_ID ? Color.yellow : Color.white;
                                break;
                        }
                        SinUI.Name.text = PassData.Name;
                        SinUI.Icon.texture = PassData.Icon;
                        break;
                }

            }
            Set_Sin_UIs[i].gameObject.SetActive(i < DataCount);
        }

    }
    void UI_Sin_Set.SetReturn.ReturnID(string Type, int ID)
    {
        var Atks = PriSetGet.AtkGet(TypeID==1);
        switch (Type)
        {
            case "PriSet":
                PSaves.PriSetID = ID;
                break;
            case "TypeChange":
                TypeID = ID;
                break;
            case "SelectChange":
                SelectID = ID;
                break;
            case "Chara":
                PriSetGet.CharaID = ID;
                break;
            case "N_Atk":
                Atks.N_AtkID = ID;
                break;
            case "S1_Atk":
                Atks.S1_AtkID = ID;
                break;
            case "S2_Atk":
                Atks.S2_AtkID = ID;
                break;
            case "E_Atk":
                Atks.E_AtkID = ID;
                break;
            case "P1":
                PriSetGet.Passive.P1_ID = ID;
                break;
            case "P2":
                PriSetGet.Passive.P2_ID = ID;
                break;
            case "P3":
                PriSetGet.Passive.P3_ID = ID;
                break;
            case "P4":
                PriSetGet.Passive.P4_ID = ID;
                break;

        }
    }
    public void Saves()
    {
        Save();
    }
}
