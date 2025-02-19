using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static PlayerValue;
using static DataBase;
public class UI_StageSelect : MonoBehaviour,UI_Sin_Set.SetReturn
{
    [SerializeField] TextMeshProUGUI StageInfoTx;
    [SerializeField] List<UI_Sin_Set> StageUIs;
    private void Update()
    {
        StageInfoTx.text = DB.Stages[StageID].Name;
        StageInfoTx.text += "\n" + DB.Stages[StageID].Info;
        for (int i = 0; i < DB.Stages.Length; i++)
        {
            if (StageUIs.Count <= i) StageUIs.Add(Instantiate(StageUIs[0], StageUIs[0].transform.parent));
            var StageD = DB.Stages[i];
            var SinUI = StageUIs[i];
            SinUI.Returns = this;
            SinUI.ID = i;
            SinUI.BackImage.color = i == StageID ? Color.yellow : Color.white;
            SinUI.Name.text = StageD.Name;
            SinUI.Icon.texture = StageD.Icon;
        }
    }
    void UI_Sin_Set.SetReturn.ReturnID(string Type, int ID)
    {
        StageID = ID;
    }
}
