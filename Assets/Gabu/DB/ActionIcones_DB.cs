using UnityEngine;

[CreateAssetMenu(fileName = "ActionIcones_DB", menuName = "Gabu/DB/ActionIcones_DB")]
public class ActionIcones_DB : ScriptableObject
{
    [Header("0:マウスカーソル、1:xbox、2:PlayStation、3:その他")]
    public ActionIcone[] Menu_icons = new ActionIcone[4];
    public ActionIcone[] Move_icons = new ActionIcone[4];
    public ActionIcone[] Jump_icons = new ActionIcone[4];
    public ActionIcone[] N_Atk_icons = new ActionIcone[4];
    public ActionIcone[] S1_Atk_icons = new ActionIcone[4];
    public ActionIcone[] E_Atk_icons = new ActionIcone[4];
    public ActionIcone[] Look_icons = new ActionIcone[4];
    public ActionIcone[] UIMove_icons = new ActionIcone[4];
    public ActionIcone[] UICofirm_icons = new ActionIcone[4];
    public ActionIcone[] UIBack_icons = new ActionIcone[4];
    public ActionIcone[] VMouseMove_icons = new ActionIcone[4];
}

