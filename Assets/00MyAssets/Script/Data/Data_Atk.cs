using System.Collections.Generic;
using UnityEngine;
using static Manifesto;
[CreateAssetMenu(menuName ="DataCre/Atk")]
public class Data_Atk : ScriptableObject
{

    public string Name;
    [TextArea]public string Info;
    public Texture Icon;
    [Tooltip("タイプ")] public Enum_AtkType AtkType;
    [Tooltip("フィルター")] public List<Enum_AtkFilter> Filters;
    [Tooltip("終了時間(フレーム)")] public int EndTime;
    [Tooltip("CT(秒)")] public float CT;
    [Tooltip("SP消費")] public int SPUse;

    [Header("分岐情報")]public List<Class_Atk_BranchInfo> BranchInfos;
    [Header("分岐先")] public Class_Atk_Branch[] Branchs;
    [Header("制限")] public Class_Atk_Fixed[] Fixeds;
    [Header("弾発射")] public Class_Atk_Shot_Base[] Shots;
    [Header("移動")] public Class_Atk_Move[] Moves;
    [Header("ステータス変化")] public Class_Atk_State[] States;
    [Header("状態変化")] public Class_Atk_Buf[] Bufs;
    [Header("武器表示")] public Class_Atk_WeponSet[] WeponSets;
    [Header("アニメーション")] public Class_Atk_Anim[] Anims;
    [Header("効果音再生")] public Class_Atk_SEPlay[] SEPlays;



    public string InfoGets()
    {
        string Str = "";
        if (Info != "") Str = Info;
        if (Str != "") Str += "\n";
        Str += "CT" + CT+"秒";
        if (SPUse > 0)Str += "\nSP" + SPUse;
        
        if (BranchInfos.Count > 0)
        {
            for (int i = 0; i < BranchInfos.Count; i++)
            {
                var BInfo = BranchInfos[i];
                Str += "\n" + BInfo.Name;
                for (int j = 0; j < Shots.Length; j++)
                {
                    var Shot = Shots[j];
                    var OStr = Shot.OtherStrGet(BInfo.BID);
                    if (OStr != "") Str += "\n" + OStr;
                }
            }
        }
        else
        {
            for (int j = 0; j < Shots.Length; j++)
            {
                var Shot = Shots[j];
                Str += "\n" + Shot.OtherStrGet(0);
            }
        }
        return Str;
    }
    private void OnValidate()
    {
        if(Fixeds!=null)
        for (int i = 0; i < Fixeds.Length; i++)
        {
            var Fixed = Fixeds[i];
            Fixed.EditDisp = "[" + i + "]";
            Fixed.EditDisp += "BNum:" + Fixed.BranchNum;
            Fixed.EditDisp += "Time:" + Fixed.Times;

        }
        if (Branchs != null)
            for (int i = 0; i < Branchs.Length; i++)
            {
                var Branch = Branchs[i];
                Branch.EditDisp = "[" + i + "]";
                Branch.EditDisp += "BNum:";
                for (int j = 0; j < Branch.BranchNums.Length; j++)
                {
                    if (j > 0) Branch.EditDisp += ",";
                    Branch.EditDisp += Branch.BranchNums[j];
                }
                Branch.EditDisp += "Time:" + Branch.Times;
                Branch.EditDisp += "MP:" + Branch.UseMP;
                Branch.EditDisp += "Future{Num:" + Branch.FutureNum;
                Branch.EditDisp += "Time:" + Branch.FutureTime + "}";
            }
        if (Shots != null)
        for (int i = 0; i < Shots.Length; i++) Shots[i].EditDispSet();
        if (Anims != null)
        for (int i = 0; i < Anims.Length; i++)
        {
            var Anim = Anims[i];
            Anim.EditDisp = "[" + i + "]";
            Anim.EditDisp += "BNum:" + Anim.BranchNum;
            Anim.EditDisp += "Time:" + Anim.Times;
            Anim.EditDisp += "ID:" + Anim.ID;
        }
    }
}
