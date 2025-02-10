using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Sin_Branch : MonoBehaviour
{
    [SerializeField]Image BackImage;
    [SerializeField]TextMeshProUGUI Name;
    [SerializeField]TextMeshProUGUI Ifs;
    public void UISet(State_Base Sta,int BranchID,string IfStr)
    {
        BackImage.color = Sta.AtkBranch == BranchID ? Color.yellow : Color.white;
        var BInfoD = Sta.AtkD.BranchInfos.Find(x => x.BID == BranchID);
        if (BInfoD != null)
        {
            Name.text = BInfoD.Name;
            Ifs.text = IfStr;
        }
        else
        {
            Name.text = BranchID.ToString("");
            Ifs.text = IfStr;
        }
    }
}
