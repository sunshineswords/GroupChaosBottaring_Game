using System;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

[CreateAssetMenu(fileName = "ActionIcones_DB", menuName = "Gabu/DB/ActionIcones_DB")]
public class ActionIcones_DB : ScriptableObject
{
    [Header("0:マウスカーソル、1:xbox、2:PlayStation、3:その他")]
    public Texture[] Menu_icons = new Texture[4];
    public Texture[] Move_icons = new Texture[4];
    public Texture[] Jump_icons = new Texture[4];
    public Texture[] N_Atk_icons = new Texture[4];
    public Texture[] S1_Atk_icons = new Texture[4];
    public Texture[] E_Atk_icons = new Texture[4];
    public Texture[] Look_icons = new Texture[4];
    public Texture[] UIMove_icons = new Texture[4];
    public Texture[] UICofirm_icons = new Texture[4];
    public Texture[] UIBack_icons = new Texture[4];
    public Texture[] VMouseMove_icons = new Texture[4];
}
