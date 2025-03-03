using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static PlayerValue;
public class UI_GameOptions : MonoBehaviour
{
    [SerializeField] Slider CamSlider;
    [SerializeField] TextMeshProUGUI CamValTx;
    [SerializeField] Slider TargetSlider;
    [SerializeField] TextMeshProUGUI TargetValTx;

    private void Start()
    {
        CamSlider.value = PSaves.CamSpeed;
        TargetSlider.value = PSaves.TargetSpeed;
        UISet();
    }

    public void CamSets()
    {
        PSaves.CamSpeed = CamSlider.value;
        UISet();
    }
    public void TargetSets()
    {
        PSaves.TargetSpeed = TargetSlider.value;
        UISet();
    }
    void UISet()
    {
        CamValTx.text = (PSaves.CamSpeed * 100).ToString("F0") + "%";
        TargetValTx.text = (PSaves.TargetSpeed * 100).ToString("F0") + "%";
    }
}
