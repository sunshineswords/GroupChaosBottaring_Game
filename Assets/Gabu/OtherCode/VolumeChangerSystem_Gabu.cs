using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using static PlayerValue;
public class VolumeChangerSystem_Gabu : MonoBehaviour
{
    #region 変数

    [SerializeField] AudioMixer audioMixer;
    [SerializeField] string MasterValue;
    [SerializeField] string BGMValue;
    [SerializeField] string SEValue;
    [SerializeField] string SystemValue;

    [SerializeField] Slider MasterSlider;
    [SerializeField] Slider BGMSlider;
    [SerializeField] Slider SESlider;
    [SerializeField] Slider SystemSlider;

    [SerializeField] TextMeshProUGUI MasterValTx;
    [SerializeField] TextMeshProUGUI BGMValTx;
    [SerializeField] TextMeshProUGUI SEValTx;
    [SerializeField] TextMeshProUGUI SystemValTx;

    #endregion
    #region 関数

    public void ChengeMain(float sliderValue)
    {
        ChangeVolume(sliderValue, MasterValue);
    }
    public void ChengeBGM(float sliderValue)
    {
        ChangeVolume(sliderValue, BGMValue);
    }
    public void ChengeSE(float sliderValue)
    {
        ChangeVolume(sliderValue, SEValue);
    }
    public void ChengeSystem(float sliderValue)
    {
        ChangeVolume(sliderValue, SystemValue);
    }

    public void ChangeVolume(float sliderValue, string targetName)
    {
        audioMixer.SetFloat(targetName, Mathf.Clamp(Mathf.Log(sliderValue, 10) * 60f, -80, 20));
        ValTxSet();
    }

    public void SaveVolume()
    {
        PSaves.MasterVol = MasterSlider.value;
        PSaves.BGMVol = BGMSlider.value;
        PSaves.SEVol = SESlider.value;
        PSaves.SystemVol = SystemSlider.value;
        Save();
    }
    void ValTxSet()
    {
        MasterValTx.text = (MasterSlider.value * 100f).ToString("F0")+"%";
        BGMValTx.text = (BGMSlider.value * 100f).ToString("F0") + "%";
        SEValTx.text = (SESlider.value * 100f).ToString("F0") + "%";
        SystemValTx.text = (SystemSlider.value * 100f).ToString("F0") + "%";
    }
    #endregion

    private void Start()
    {
        audioMixer.SetFloat(MasterValue, Mathf.Clamp(Mathf.Log(PSaves.MasterVol, 10) * 60f, -80, 20));
        audioMixer.SetFloat(BGMValue, Mathf.Clamp(Mathf.Log(PSaves.BGMVol, 10) * 60f, -80, 20));
        audioMixer.SetFloat(SEValue, Mathf.Clamp(Mathf.Log(PSaves.SEVol, 10) * 60f, -80, 20));
        audioMixer.SetFloat(SystemValue, Mathf.Clamp(Mathf.Log(PSaves.SystemVol, 10) * 60f, -80, 20));

        MasterSlider.value = PSaves.MasterVol;
        BGMSlider.value = PSaves.BGMVol;
        SESlider.value = PSaves.SEVol;
        SystemSlider.value = PSaves.SystemVol;

        ValTxSet();

    }

}
