using System;
using UnityEngine;
using UnityEngine.UI;

public class AttackButtonSystem_Gabu : MonoBehaviour
{
    #region 変数

    private float currentCT = 0f;
    public float ct = 0f;
    public AnimatorStatu statu = AnimatorStatu.Normal;

    [SerializeField] private Slider _CTSider;
    [SerializeField] private UISystem_Gabu[] _uiSystem;

    public enum AnimatorStatu : int
    {
        NONE = -1, Normal = 0, Highlighted, Pressed, Selected, Disabled
    }

    #endregion

    #region 関数


    #endregion

    private void Update()
    {
        if (currentCT == ct)
        {
            return;
        }
        currentCT = ct;
        _CTSider.value = currentCT;

        for (int i = 0; i < _uiSystem.Length; i++)
        {
            if (statu == AnimatorStatu.NONE)
            {
                _uiSystem[i].IsAlignment = true;
                continue;
            }
            _uiSystem[i].setState = (int)statu;
        }
    }
}