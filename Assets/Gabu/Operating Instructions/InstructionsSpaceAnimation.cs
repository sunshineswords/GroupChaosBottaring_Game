using DG.Tweening;
using System;
using UnityEngine;

public class InstructionsSpaceAnimation : MonoBehaviour, IUIAnimation
{
    #region 変数

    private Tween _tween;

    [SerializeField] Vector2 _startPosition;
    [SerializeField] Vector2 _endPosition;
    [SerializeField] float _duration = 0.3f;
    [SerializeField] Ease _easeType = Ease.Linear;
    [SerializeField] int _loopCount = 0;
    [SerializeField] LoopType _loopType = LoopType.Restart;

    public bool IsPlaying => _tween != null && _tween.IsActive() && _tween.IsPlaying();
    public bool IsPaused => _tween != null && _tween.IsActive() && !_tween.IsPlaying();

    public event Action OnStart;
    public event Action OnComplete;
    public event Action OnUpdate;


    #endregion

    #region 関数

    public void Play()
    {
        if (_tween == null)
        {
            _tween = CreateAnimation();
        }

        _tween.Play();
        OnStart?.Invoke();
    }

    public void Pause()
    {
        _tween?.Pause();
    }

    public void Resume()
    {
        _tween?.Play();
    }

    public void Stop()
    {
        _tween?.Kill();
        _tween = null;
    }

    public void SetDuration(float duration)
    {
        _duration = duration;
        RestartTween();
    }

    public void SetEasing(Ease easeType)
    {
        _easeType = easeType;
        RestartTween();
    }

    public void SetLoop(int loopCount, LoopType loopType)
    {
        _loopCount = loopCount;
        _loopType = loopType;
        RestartTween();
    }

    private void RestartTween()
    {
        if (_tween != null)
        {
            _tween.Kill();
        }
        _tween = CreateAnimation();
    }

    private Tween CreateAnimation()
    {
        transform.position = _startPosition;
        return transform.DOMoveY(_endPosition.y, _duration)
            .SetEase(_easeType)
            .SetLoops(_loopCount, _loopType)
            .OnComplete(() => OnComplete?.Invoke())
            .OnUpdate(() => OnUpdate?.Invoke());
    }

    #endregion

    private void Start()
    {

        Play();
    }
}
