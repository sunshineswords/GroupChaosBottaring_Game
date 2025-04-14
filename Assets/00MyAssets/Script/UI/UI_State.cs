using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static Manifesto;
using static DataBase;
public class UI_State : MonoBehaviour
{
    public State_Base Sta;
    [SerializeField] TextMeshProUGUI NameTx;
    [SerializeField] Image HPBackBar;
    [SerializeField] Image HPBackFill;
    [SerializeField] Image HPMiddleBar;
    [SerializeField] Image HPMiddleFill;
    [SerializeField] Image HPFrontBar;
    [SerializeField] Image HPFrontFill;
    [SerializeField] Image BreakBar;
    [SerializeField] Image BreakFill;
    [SerializeField] TextMeshProUGUI BreakText;
    [SerializeField] List<UI_Sin_Buf> BufUIs;
    float CHPPer;
    private void Start()
    {
        CHPPer = 1;
    }
    void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        BaseSet();
    }
    public void BaseSet()
    {
        if(NameTx!=null) NameTx.text = Sta.Name;
        float HPPer = Sta.HP / Mathf.Max(1, Sta.MHP);
        Color HPCol = Color.red;
        switch (Sta.Team)
        {
            case 0:
                HPCol = Color.green;
                break;
        }
        float BufChanges = -Sta.BufTPMultGet(Enum_Bufs.毒) / 60f;
        float BufPer = BufChanges / Mathf.Max(1, Sta.FMHP);
        float ChangeSpeed = 0.01f;
        if (HPPer < CHPPer)
        {
            HPBackBar.fillAmount = CHPPer;
            HPBackFill.color = new Color(1f, 0.5f, 0f);
            HPMiddleBar.fillAmount = HPPer;
            HPMiddleFill.color = Color.magenta;
            HPFrontBar.fillAmount = HPPer + BufPer;
            HPFrontFill.color = HPCol;
            CHPPer = Mathf.Max(CHPPer - ChangeSpeed, HPPer);
        }
        else
        {
            HPBackBar.fillAmount = HPPer;
            HPBackFill.color = new Color(0.5f, 1f, 0.5f);
            HPMiddleBar.fillAmount = CHPPer;
            HPMiddleFill.color = Color.magenta; 
            HPFrontBar.fillAmount = CHPPer + BufPer;
            HPFrontFill.color = HPCol;
            CHPPer = Mathf.Min(CHPPer + ChangeSpeed, HPPer);
        }
        CHPPer = Mathf.Clamp01(CHPPer);

        if (BreakBar != null)
        {
            if (Sta.BreakT <= 0)
            {
                BreakBar.fillAmount = Sta.BreakV / Mathf.Max(1f, Sta.MBreak);
                BreakFill.color = Color.cyan;
                BreakText.text = "";
            }
            else
            {
                BreakBar.fillAmount = Sta.BreakT / Mathf.Max(1f, Sta.BreakTime);
                BreakFill.color = Color.HSVToRGB(Mathf.Repeat(Time.time*1f, 1f), 1, 1);
                BreakText.text = "Break!!!";
            }
        }

        for (int i = 0; i < Mathf.Max(BufUIs.Count, Sta.Bufs.Count); i++)
        {
            if (i < Sta.Bufs.Count)
            {
                if (BufUIs.Count <= i) BufUIs.Add(Instantiate(BufUIs[0], BufUIs[0].transform.parent));
                var Bufi = Sta.Bufs[i];
                var BufD = DB.Bufs.Find(x => (int)x.Buf == Bufi.ID);
                if (BufD != null)
                {
                    BufUIs[i].BackImage.color = BufD.Col;
                    BufUIs[i].Icon.texture = BufD.Icon;
                    BufUIs[i].Icon.color = BufUIs[i].Icon.texture != null ? Color.white : Color.clear;
                }
                else
                {
                    BufUIs[i].BackImage.color = Color.white;
                    BufUIs[i].Icon.color = Color.clear;
                }
                BufUIs[i].NameTx.text = ((Enum_Bufs)Bufi.ID).ToString();
                BufUIs[i].PowTx.text = Bufi.Pow > 0 ? Bufi.Pow.ToString() : "";
                if (Bufi.TimeMax > 0) BufUIs[i].TimeImage.fillAmount = 1f - ((float)Bufi.Time / Bufi.TimeMax);
                else BufUIs[i].TimeImage.fillAmount = 0;
            }
            BufUIs[i].gameObject.SetActive(i < Sta.Bufs.Count);
        }
    }
}
