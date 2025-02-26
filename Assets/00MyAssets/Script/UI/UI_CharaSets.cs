using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static DataBase;
using static Manifesto;
using static PlayerValue;

public class UI_CharaSets : MonoBehaviour, UI_Sin_Set.SetReturn
{
    #region 変数

    [SerializeField] List<UI_Sin_Set> PriSet_Sin_UIs;
    //[SerializeField] Toggle FBToggle;
    [SerializeField] TextMeshProUGUI InfoTx;
    [EnumIndex(typeof(Enum_SetSlot))]
    [SerializeField] UI_Sin_Set[] SetSelectUIs;
    [SerializeField] List<UI_Sin_Set> Set_Sin_UIs;
    [SerializeField] TMP_Dropdown FilterDr;
    [SerializeField] ChangeButtonSettings buttonSetting;

    [SerializeField] Transform ModelTrans;

    int SelectID = 0;
    int FilterID = -1;
    GameObject ModelIns;

    #endregion

    private void Start()
    {
        // フィルターを更新
        UIUpdate();
        FilterUpdate();
    }
    void UIUpdate()
    {
        // PriSet_Sin_UIsの更新
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
        string Info = "";
        int DataCount = 0;
        // キャラデータの更新
        for (int i = 0; i < SetSelectUIs.Length; i++)
        {
            SetSelectUIs[i].Returns = this;
            SetSelectUIs[i].Type = "SelectChange";
            SetSelectUIs[i].ID = i;
            SetSelectUIs[i].BackImage.color = SetSelectUIs[i].ID == SelectID ? Color.yellow : Color.white;

            string Name = "";
            Texture Icon = null;

            int DCount = 0;
            Data_Chara CharaD = null;
            Data_Atk AtkD = null;
            Data_Passive PassD = null;

            switch ((Enum_SetSlot)i)
            {
                case Enum_SetSlot.キャラ:
                    CharaD = DB.Charas[PriSetGet.CharaID];
                    DCount = DB.Charas.Length;
                    break;
                case Enum_SetSlot.表通常:
                case Enum_SetSlot.裏通常:
                    AtkD = DB.N_Atks[PriSetGet.AtkGet(i == (int)Enum_SetSlot.裏通常).N_AtkID];
                    DCount = DB.N_Atks.Length;
                    break;
                case Enum_SetSlot.表スキル1:
                case Enum_SetSlot.裏スキル1:
                    AtkD = DB.S_Atks[PriSetGet.AtkGet(i == (int)Enum_SetSlot.裏スキル1).S1_AtkID];
                    DCount = DB.S_Atks.Length;
                    break;
                case Enum_SetSlot.表スキル2:
                case Enum_SetSlot.裏スキル2:
                    AtkD = DB.S_Atks[PriSetGet.AtkGet(i == (int)Enum_SetSlot.裏スキル2).S2_AtkID];
                    DCount = DB.S_Atks.Length;
                    break;
                case Enum_SetSlot.表必殺:
                case Enum_SetSlot.裏必殺:
                    AtkD = DB.E_Atks[PriSetGet.AtkGet(i == (int)Enum_SetSlot.裏必殺).E_AtkID];
                    DCount = DB.E_Atks.Length;
                    break;
                case Enum_SetSlot.パッシブ1:
                    PassD = DB.Passives[PriSetGet.Passive.P1_ID];
                    DCount = DB.Passives.Length;
                    break;
                case Enum_SetSlot.パッシブ2:
                    PassD = DB.Passives[PriSetGet.Passive.P2_ID];
                    DCount = DB.Passives.Length;
                    break;
                case Enum_SetSlot.パッシブ3:
                    PassD = DB.Passives[PriSetGet.Passive.P3_ID];
                    DCount = DB.Passives.Length;
                    break;
                case Enum_SetSlot.パッシブ4:
                    PassD = DB.Passives[PriSetGet.Passive.P4_ID];
                    DCount = DB.Passives.Length;
                    break;
            }
            if (CharaD != null)
            {
                Name = CharaD.Name;
                Icon = CharaD.Icon;
                if (i == SelectID) Info = CharaD.Name;
                if (ModelIns != null) Destroy(ModelIns);
                ModelIns = Instantiate(CharaD.ModelObj.gameObject, ModelTrans.position, Quaternion.identity);
                ModelIns.transform.parent = ModelTrans;
            }
            if (AtkD != null)
            {
                Name = AtkD.Name;
                Icon = AtkD.Icon;
                if (i == SelectID) Info = AtkD.Name + AtkD.InfoGets();
            }
            if (PassD != null)
            {
                Name = PassD.Name;
                Icon = PassD.Icon;
                if (i == SelectID) Info = PassD.Name + "\n" + PassD.Info;
            }
            if (i == SelectID) DataCount = DCount;
            SetSelectUIs[i].Name.text = Name;
            SetSelectUIs[i].Icon.texture = Icon;
        }

        InfoTx.text = Info;

        // Set_Sin_UIsの更新
        for (int i = 0; i < Mathf.Max(DataCount, Set_Sin_UIs.Count); i++)
        {
            bool Disp = true;
            if (i < DataCount)
            {
                if (Set_Sin_UIs.Count <= i)
                {
                    Set_Sin_UIs.Add(Instantiate(Set_Sin_UIs[0], Set_Sin_UIs[0].transform.parent));
                }
                var SinUI = Set_Sin_UIs[i];
                SinUI.Returns = this;
                SinUI.ID = i;

                buttonSetting.targets.Add(SinUI.GetComponent<ImageAnimationManager>());

                bool Select = false;
                string Name = "";
                Texture Icon = null;

                Data_Chara CharaD = null;
                Data_Atk AtkD = null;
                Data_Passive PassD = null;

                switch ((Enum_SetSlot)SelectID)
                {
                    case Enum_SetSlot.キャラ:
                        CharaD = DB.Charas[i];
                        SinUI.Type = "Chara";
                        Select = i == PriSetGet.CharaID;
                        break;
                    case Enum_SetSlot.表通常:
                    case Enum_SetSlot.裏通常:
                        AtkD = DB.N_Atks[i];
                        SinUI.Type = SelectID == (int)Enum_SetSlot.裏通常 ? "B_N_Atk" : "F_N_Atk";
                        Select = i == PriSetGet.AtkGet(SelectID == (int)Enum_SetSlot.裏通常).N_AtkID;
                        break;
                    case Enum_SetSlot.表スキル1:
                    case Enum_SetSlot.裏スキル1:
                        AtkD = DB.S_Atks[i];
                        SinUI.Type = SelectID == (int)Enum_SetSlot.裏スキル1 ? "B_S1_Atk" : "F_S1_Atk";
                        Select = i == PriSetGet.AtkGet(SelectID == (int)Enum_SetSlot.裏スキル1).S1_AtkID;
                        break;
                    case Enum_SetSlot.表スキル2:
                    case Enum_SetSlot.裏スキル2:
                        AtkD = DB.S_Atks[i];
                        SinUI.Type = SelectID == (int)Enum_SetSlot.裏スキル2 ? "B_S2_Atk" : "F_S2_Atk";
                        Select = i == PriSetGet.AtkGet(SelectID == (int)Enum_SetSlot.裏スキル2).S2_AtkID;
                        break;
                    case Enum_SetSlot.表必殺:
                    case Enum_SetSlot.裏必殺:
                        AtkD = DB.E_Atks[i];
                        SinUI.Type = SelectID == (int)Enum_SetSlot.裏必殺 ? "B_E_Atk" : "F_E_Atk";
                        Select = i == PriSetGet.AtkGet(SelectID == (int)Enum_SetSlot.裏必殺).E_AtkID;
                        break;
                    case Enum_SetSlot.パッシブ1:
                        PassD = DB.Passives[i];
                        SinUI.Type = "P1";
                        Select = i == PriSetGet.Passive.P1_ID;
                        break;
                    case Enum_SetSlot.パッシブ2:
                        PassD = DB.Passives[i];
                        SinUI.Type = "P2";
                        Select = i == PriSetGet.Passive.P2_ID;
                        break;
                    case Enum_SetSlot.パッシブ3:
                        PassD = DB.Passives[i];
                        SinUI.Type = "P3";
                        Select = i == PriSetGet.Passive.P3_ID;
                        break;
                    case Enum_SetSlot.パッシブ4:
                        PassD = DB.Passives[i];
                        SinUI.Type = "P4";
                        Select = i == PriSetGet.Passive.P4_ID;
                        break;
                }
                if (CharaD != null)
                {
                    Name = CharaD.Name;
                    Icon = CharaD.Icon;
                }
                if (AtkD != null)
                {
                    Name = AtkD.Name;
                    Icon = AtkD.Icon;
                    if (FilterDr.value > 0)
                    {
                        var FilterKeys = Enum.GetValues(typeof(Enum_AtkFilter));
                        if (!AtkD.Filters.Contains((Enum_AtkFilter)FilterKeys.GetValue(FilterDr.value - 1))) Disp = false;
                    }
                }
                if (PassD != null)
                {
                    Name = PassD.Name;
                    Icon = PassD.Icon;
                    if (FilterDr.value > 0)
                    {
                        var FilterKeys = Enum.GetValues(typeof(Enum_PassiveFilter));
                        if (!PassD.Filters.Contains((Enum_PassiveFilter)FilterKeys.GetValue(FilterDr.value - 1))) Disp = false;
                    }
                }
                SinUI.BackImage.color = Select ? Color.yellow : Color.white;
                SinUI.Name.text = Name;
                SinUI.Icon.texture = Icon;
            }
            Set_Sin_UIs[i].gameObject.SetActive(i < DataCount && Disp);
        }
    }
    #region　関数
    void FilterUpdate()
    {
        // フィルターのオプションをクリア
        FilterDr.options.Clear();
        FilterDr.options.Add(new TMP_Dropdown.OptionData { text = "無" });
        int FID = -1;
        // 選択IDに応じたフィルターの更新
        switch ((Enum_SetSlot)SelectID)
        {
            case Enum_SetSlot.表通常:
            case Enum_SetSlot.裏通常:
            case Enum_SetSlot.表スキル1:
            case Enum_SetSlot.裏スキル1:
            case Enum_SetSlot.表スキル2:
            case Enum_SetSlot.裏スキル2:
            case Enum_SetSlot.表必殺:
            case Enum_SetSlot.裏必殺:
                var AtkKeys = Enum.GetValues(typeof(Enum_AtkFilter));
                for (int i = 0; i < AtkKeys.Length; i++)
                {
                    FilterDr.options.Add(new TMP_Dropdown.OptionData { text = AtkKeys.GetValue(i).ToString() });
                }
                FID = 0;
                break;
            case Enum_SetSlot.パッシブ1:
            case Enum_SetSlot.パッシブ2:
            case Enum_SetSlot.パッシブ3:
            case Enum_SetSlot.パッシブ4:
                var PassKeys = Enum.GetValues(typeof(Enum_PassiveFilter));
                for (int i = 0; i < PassKeys.Length; i++)
                {
                    FilterDr.options.Add(new TMP_Dropdown.OptionData { text = PassKeys.GetValue(i).ToString() });
                }
                FID = 1;
                break;
        }
        if (FilterID != FID) FilterDr.value = 0;
        FilterID = FID;
    }
    void UI_Sin_Set.SetReturn.ReturnID(string Type, int ID)
    {
        // タイプに応じたIDの更新
        switch (Type)
        {
            case "PriSet":
                PSaves.PriSetID = ID;
                break;
            case "SelectChange":
                SelectID = ID;
                FilterUpdate();
                break;
            case "Chara":
                PriSetGet.CharaID = ID;
                break;
            case "F_N_Atk":
                PriSetGet.AtkF.N_AtkID = ID;
                break;
            case "F_S1_Atk":
                PriSetGet.AtkF.S1_AtkID = ID;
                break;
            case "F_S2_Atk":
                PriSetGet.AtkF.S2_AtkID = ID;
                break;
            case "F_E_Atk":
                PriSetGet.AtkF.E_AtkID = ID;
                break;
            case "B_N_Atk":
                PriSetGet.AtkB.N_AtkID = ID;
                break;
            case "B_S1_Atk":
                PriSetGet.AtkB.S1_AtkID = ID;
                break;
            case "B_S2_Atk":
                PriSetGet.AtkB.S2_AtkID = ID;
                break;
            case "B_E_Atk":
                PriSetGet.AtkB.E_AtkID = ID;
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
        UIUpdate();
        return;
        if (buttonSetting.currentIndex != ID)
        {
            Debug.Log("SetSettings");
            buttonSetting.targets.Clear();
            foreach (var item in Set_Sin_UIs)
            {
                buttonSetting.targets.Add(item.GetComponent<ImageAnimationManager>());
            }
            buttonSetting.SetSettings(ID);
        }
    }

    public void Saves()
    {
        // データを保存
        Save();
    }

    #endregion
}
