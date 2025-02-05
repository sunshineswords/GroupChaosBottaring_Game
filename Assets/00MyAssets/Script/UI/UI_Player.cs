using UnityEngine;
using UnityEngine.UI;

public class UI_Player : MonoBehaviour
{
    [SerializeField] Player_State PSta;
    [SerializeField] Image HPBar;

    void LateUpdate()
    {
        HPBar.fillAmount = (float)PSta.HP / Mathf.Max(1, PSta.MHP);
    }
}
