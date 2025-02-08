using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class OperatingInstructionsManager_Gabu : MonoBehaviour
{
    #region 変数

    public ActionIcones_DB actionIcones_DB;
    public PlayerInput playerInput;
    public GameObject space;
    public GameObject textPrefab;
    public GameObject iconPrefab;
    public GameObject spacingPrefab;
    public Status statu = Status.UI;

    public enum Status : int
    {
        UI,
        Play,
        NONE
    }

    #endregion

    #region 関数

    private void UpDateIcones()
    {
        foreach (Transform child in space.transform)
        {
            Destroy(child.gameObject);
        }

        int deviceIndex = GetDeviceIndex();
        SetIcones(GetIcones(deviceIndex, statu));
    }

    private ActionIcone[] GetIcones(int deviceIndex, Status statu)
    {
        // デフォルトの操作説明
        List<ActionIcone> icones = new List<ActionIcone>()
        { actionIcones_DB.Menu_icons[deviceIndex] };


        if (Status.Play == statu)  // ゲーム画面の操作説明
        {
            icones.Add(actionIcones_DB.Move_icons[deviceIndex]);
            icones.Add(actionIcones_DB.Jump_icons[deviceIndex]);
            icones.Add(actionIcones_DB.N_Atk_icons[deviceIndex]);
            icones.Add(actionIcones_DB.S1_Atk_icons[deviceIndex]);
            icones.Add(actionIcones_DB.E_Atk_icons[deviceIndex]);
            icones.Add(actionIcones_DB.Look_icons[deviceIndex]);
        }
        else if (Status.UI == statu) // UI操作が必要な画面の操作説明
        {
            icones.Add(actionIcones_DB.UIMove_icons[deviceIndex]);
            icones.Add(actionIcones_DB.UICofirm_icons[deviceIndex]);
            icones.Add(actionIcones_DB.UIBack_icons[deviceIndex]);
        }
        else
        {
            Debug.LogWarning("未定義のアクションマップが使われています");
        }
        return icones.ToArray();
    }

    private int GetDeviceIndex()
    {
        switch (playerInput.currentControlScheme)
        {
            case "Keyboard&Mouse":
                return 0;
            case "Xbox Controller":
                return 1;
            case "PlayStation Controller":
                return 2;
            default:
                Debug.LogWarning("未定義のデバイスが使われています");
                return 3;
        }
    }

    private void SetIcones(ActionIcone[] icones)
    {
        if (icones == null)
        {
            Debug.LogWarning("アイコンが設定されていません");
            return;
        }

        space.SetActive(true);

        // 端のスペースを生成
        GameObject spacing1 = Instantiate(iconPrefab, space.transform);
        spacing1.GetComponent<RawImage>().texture = null;
        spacing1.GetComponent<RawImage>().color = new Color(0, 0, 0, 0);

        for (int i = 0; i < icones.Length; i++)
        {
            // アイコンの生成
            GameObject icon = Instantiate(iconPrefab, space.transform);
            icon.GetComponent<RawImage>().texture = icones[i].icon;

            // テキストの生成
            GameObject tmpObj = Instantiate(textPrefab, space.transform);
            TextMeshProUGUI tmpro = tmpObj.GetComponent<TextMeshProUGUI>();
            tmpro.text = icones[i].title;
            // スペースを開ける
            GameObject spacing = Instantiate(spacingPrefab, space.transform);

            // テキストとアイコンの順番を合わせる
            icon.transform.SetAsLastSibling();
            tmpro.transform.SetAsLastSibling();
            spacing.transform.SetAsLastSibling();

            Debug.Log($"{i} title:{icones[i].title}, {tmpro.text}");
        }

        // 端のスペースを生成
        GameObject spacing2 = Instantiate(iconPrefab, space.transform);
        spacing2.GetComponent<RawImage>().texture = null;
        spacing2.GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
        spacing1.transform.SetAsFirstSibling();
        spacing2.transform.SetAsLastSibling();
    }

    #endregion

    private void Start()
    {
        UpDateIcones();
    }
}
