using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static DataBase;
using static PlayerValue;
public class UI_CharaSets : MonoBehaviour,UI_Sin_Set.SetReturn
{
    [SerializeField] GameObject SetUI;
    [SerializeField] TextMeshProUGUI SelChara_Name;
    [SerializeField] List<UI_Sin_Set> Chara_UIs;
    [SerializeField] TextMeshProUGUI SelN_AtkName;
    [SerializeField] List<UI_Sin_Set> N_Atk_UIs;
    bool Open = false;

    void Start()
    {
        if (!Open) SetUI.SetActive(false);
        Save();
    }
    void Update()
    {
        SelChara_Name.text = "";
        for (int i = 0; i < DB.Charas.Length; i++)
        {
            if (Chara_UIs.Count <= i)
            {
                Chara_UIs.Add(Instantiate(Chara_UIs[0], Chara_UIs[0].transform.parent));
            }
            var CharaD = DB.Charas[i];
            var SinUI = Chara_UIs[i];
            SinUI.Returns = this;
            SinUI.Type = "Chara";
            SinUI.ID = i;
            SinUI.BackImage.color = i == PSaves.CharaID ? Color.yellow : Color.white;
            SinUI.Name.text = CharaD.Name;
            SinUI.Icon.texture = CharaD.Icon;
        }
        SelN_AtkName.text = DB.N_Atks[PSaves.N_AtkID].Name;
        for (int i=0;i < DB.N_Atks.Length; i++)
        {
            if(N_Atk_UIs.Count <= i)
            {
                N_Atk_UIs.Add(Instantiate(N_Atk_UIs[0], N_Atk_UIs[0].transform.parent));
            }
            var NAtkD = DB.N_Atks[i];
            var SinUI = N_Atk_UIs[i];
            SinUI.Returns = this;
            SinUI.Type = "N_Atk";
            SinUI.ID = i;
            SinUI.BackImage.color = i == PSaves.N_AtkID ? Color.yellow : Color.white;
            SinUI.Name.text = NAtkD.Name;
            SinUI.Icon.texture = NAtkD.Icon;
        }
    }
    public void UIOC()
    {
        Open = true;
        SetUI.SetActive(!SetUI.activeSelf);
    }
    void UI_Sin_Set.SetReturn.ReturnID(string Type, int ID)
    {
        switch (Type)
        {
            case "Chara":
                PSaves.CharaID = ID;
                break;
            case "N_Atk":
                PSaves.N_AtkID = ID;
                break;
        }
    }
}
