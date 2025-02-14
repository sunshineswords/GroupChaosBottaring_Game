using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using System;

public class UI_Sin_Atk : MonoBehaviour, IUIAnimation
{
    public Image BackImage;
    public RawImage Icon;
    public TextMeshProUGUI Name;
    public Image ChargeImage;
    public Image CTImage;

    public float Duration = 0.3f;
    public Ease EaseType = Ease.Linear;
    public int LoopCount = 0;
    public LoopType LoopType = LoopType.Restart;

    #region interface変数
    public bool IsPlaying { get; }
    public bool IsPaused { get; }
    public bool IsReversed { get; }


    public event Action OnStart;
    public event Action OnComplete;
    public event Action OnUpdate;

    #endregion
    #region interface関数
    public void Play()
    {

    }

    public void Pause()
    {
    }

    public void Resume()
    {
    }

    public void Reverse()
    {
    }

    public void Stop()
    {
    }

    public void SetDuration(float duration)
    {
    }

    public void SetEasing(Ease easeType)
    {
    }

    public void SetLoop(int loopCount, LoopType loopType)
    {
    }

    #endregion
}
