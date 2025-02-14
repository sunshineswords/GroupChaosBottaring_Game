using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class OperatingInstructionsManager_Gabu : MonoBehaviour
{
    #region 変数

    public ActionIcones_DB actionIcones_DB;
    public PlayerInput playerInput;
    public GameObject space;
    public GameObject iconPrefab;
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
            icones.Add(actionIcones_DB.Target_icons[deviceIndex]);
            icones.Add(actionIcones_DB.Chenge_icons[deviceIndex]);
            icones.Add(actionIcones_DB.N_Atk_icons[deviceIndex]);
            icones.Add(actionIcones_DB.S1_Atk_icons[deviceIndex]);
            icones.Add(actionIcones_DB.S2_Atk_icons[deviceIndex]);
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

        for (int i = 0; i < icones.Length; i++)
        {
            // アイコンの生成
            GameObject icon = Instantiate(iconPrefab, space.transform);
            IconPrefabSystem_Gabu prefabSystem = icon.GetComponent<IconPrefabSystem_Gabu>();

            // アイコンの設定
            prefabSystem.icon.texture = icones[i].icon;

            // テキストの設定
            prefabSystem.tmpro.text = icones[i].title;
        }
    }

    #endregion

    private void Start()
    {
        UpDateIcones();
    }
}
