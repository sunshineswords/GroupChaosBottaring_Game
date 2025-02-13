using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Sin_BossUI : MonoBehaviour
{
    public TextMeshProUGUI NameTx;
    public Image HPBackBar;
    public Image HPBackFill;
    public Image HPFrontBar;
    public Image HPFrontFill;
    public float CHPPer;
    private void Start()
    {
        CHPPer = 1;
    }
}
