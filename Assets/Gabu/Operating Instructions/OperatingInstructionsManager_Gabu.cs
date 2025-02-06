using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using System.Collections.Generic;
using TMPro;

public class OperatingInstructionsManager_Gabu : MonoBehaviour
{
    #region 変数

    public ActionIcones_DB actionIcones_DB;
    public PlayerInput playerInput;
    public GameObject space;
    public GameObject textPrefab;
    public GameObject iconPrefab;

    private readonly int DefaultRange = 0;
    private readonly int PlayerRange = 6;
    private readonly int UIRange = 10;

    #endregion

    #region 関数

    private void UpDateIcones()
    {
        foreach (Transform child in space.transform)
        {
            Destroy(child.gameObject);
        }

        int deviceIndex = GetDeviceIndex();
        string actionMap = playerInput.currentActionMap.name;

        List<Texture> icones = new List<Texture>();
        if ("Player" == actionMap)
        {
            for (int i = DefaultRange + 1; i <= PlayerRange; i++)
            {
                icones.Add(actionIcones_DB.Menu_icons[i]);
            }
        }
        else if ("UI" == actionMap)
        {
            for (int i = PlayerRange + 1; i <= UIRange; i++)
            {
                icones.Add(actionIcones_DB.Menu_icons[i]);
            }
        }
        else
        {
            space.SetActive(false);
            Debug.LogWarning("未定義のアクションマップが使われています");
            return;
        }

        SetIcones(icones.ToArray());
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
                return 3;
        }
    }

    private void SetIcones(Texture[] icones)
    {
        space.SetActive(true);

        // 端のスペースを生成
        GameObject spacing1 = Instantiate(iconPrefab, space.transform);
        spacing1.GetComponent<RawImage>().texture = null;
        spacing1.GetComponent<RawImage>().color = new Color(0, 0, 0, 0);

        for (int i = 0; i < icones.Length; i++)
        {
            // アイコンの生成
            GameObject icon = Instantiate(iconPrefab, space.transform);
            icon.GetComponent<RawImage>().texture = icones[i];

            // テキストの生成
            Instantiate(textPrefab, space.transform);
            textPrefab.GetComponent<TextMeshProUGUI>().text = playerInput.currentActionMap.actions[i].name;
        }

        // 端のスペースを生成
        GameObject spacing2 = Instantiate(iconPrefab, space.transform);
        spacing2.GetComponent<RawImage>().texture = null;
        spacing2.GetComponent<RawImage>().color = new Color(0, 0, 0, 0);
    }

    #endregion

    private void Start()
    {
        UpDateIcones();
    }
}
