using UnityEngine;
using UnityEngine.UI;

public class UI_Enemy : MonoBehaviour
{
    [SerializeField] Enemy_State Sta;
    [SerializeField] Image HPbar;
    void LateUpdate()
    {
        transform.LookAt(Camera.main.transform);
        HPbar.fillAmount = (float)Sta.HP / Mathf.Max(1, Sta.MHP);
    }
}
