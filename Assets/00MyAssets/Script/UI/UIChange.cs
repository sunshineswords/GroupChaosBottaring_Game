using UnityEngine;
using UnityEngine.UI;

public class UIChange : MonoBehaviour
{
    [Tooltip("切り替えUIオブジェクト"), SerializeField] GameObject[] UIs;
    [Tooltip("ボタンイメージ"), SerializeField] Image[] ButtonUIs;
    [Tooltip("非選択ボタン色"), SerializeField] Color NonColor;
    [Tooltip("選択中ボタン色"), SerializeField] Color SelectColor;
    public int UIID;

    void LateUpdate()
    {
        for (int i = 0; i < UIs.Length; i++) UIs[i].SetActive(i == UIID);
        for(int i = 0; i < ButtonUIs.Length; i++)
        {
            ButtonUIs[i].color = i == UIID ? SelectColor : NonColor;
        }
    }
    public void Change(int ID)
    {
        UIID = ID;
    }
}
