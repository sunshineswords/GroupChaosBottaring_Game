using DG.Tweening;
using TMPro;
using UnityEngine;

public class TextAnimation_Gabu : UISystem_Gabu
{
    #region 変数
    private TextMeshProUGUI tmp;

    [SerializeField, Header("各状態のテキスト色")]
    private Color _normalTextColor;
    [SerializeField]
    private Color _highlightedTextColor;
    [SerializeField]
    private Color _pressedTextColor;
    [SerializeField]
    private Color _selectedTextColor;
    [SerializeField]
    private Color _disabledTextColor;
    #endregion

    #region 関数
    protected override void NormalAnimation()
    {
        if (_i_currentAnimation == _i_lastAnimation)
        {
            return;
        }

        Animate(_normalScaleMultiplier, _normalScaleDuration, _normalEase, _normalTextColor);
    }

    protected override void HighlightedAnimation()
    {
        if (_i_currentAnimation == _i_lastAnimation)
        {
            return;
        }

        Animate(_highlightedScaleMultiplier, _highlightedScaleDuration, _highlightedEase, _highlightedTextColor);
    }

    protected override void PressedAnimation()
    {
        if (_i_currentAnimation == _i_lastAnimation)
        {
            return;
        }

        Animate(_pressedScaleMultiplier, _pressedScaleDuration, _pressedEase, _pressedTextColor);
    }

    protected override void SelectedAnimation()
    {
        if (_i_currentAnimation == _i_lastAnimation)
        {
            return;
        }

        Animate(_selectedScaleMultiplier, _selectedScaleDuration, _selectedEase, _selectedTextColor);
    }

    protected override void DisabledAnimation()
    {
        if (_i_currentAnimation == _i_lastAnimation)
        {
            return;
        }

        Animate(_disabledScaleMultiplier, _disabledScaleDuration, _disabledEase, _disabledTextColor);
    }

    private void Animate(float scaleMultiplier, float duration, Ease ease, Color targetColor)
    {
        _transform.DOScale(_unitScale * scaleMultiplier, duration).SetEase(ease);
        tmp.DOColor(targetColor, duration);
    }
    #endregion

    protected override void Start()
    {
        if (tmp == null)
        {
            tmp = GetComponent<TextMeshProUGUI>();
        }

        base.Start();

        if (!_isReset)
        {
            return;
        }

        // 各プロパティの初期値を設定
        _normalScaleDuration = 0.4f;
        _highlightedScaleDuration = 0.2f;
        _pressedScaleDuration = 0f;
        _selectedScaleDuration = 0.2f;
        _disabledScaleDuration = 0.05f;

        _normalScaleMultiplier = 1.0f;
        _highlightedScaleMultiplier = 1.1f;
        _pressedScaleMultiplier = 1.05f;
        _selectedScaleMultiplier = 0.97f;
        _disabledScaleMultiplier = 1.0f;

        _normalEase = Ease.Linear;
        _highlightedEase = Ease.OutBack;
        _pressedEase = Ease.Linear;
        _selectedEase = Ease.OutBack;
        _disabledEase = Ease.Linear;

        _i_currentAnimation = CheckAnimationState();
        switch (_i_currentAnimation)
        {
            case (int)AnimatorState.Normal:
                NormalAnimation();
                break;
            case (int)AnimatorState.Highlighted:
                HighlightedAnimation();
                break;
            case (int)AnimatorState.Pressed:
                PressedAnimation();
                break;
            case (int)AnimatorState.Selected:
                SelectedAnimation();
                break;
            case (int)AnimatorState.Disabled:
                DisabledAnimation();
                break;
            default:
                Debug.LogWarning("予期しないアニメーションが参照されました");
                break;
        }
        _i_lastAnimation = _i_currentAnimation;
    }
}
