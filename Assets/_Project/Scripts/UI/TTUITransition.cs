using DG.Tweening;
using Sirenix.Utilities;
using System;
using UnityEngine;



[Serializable]
public abstract class TTUITransition
{
    [field : SerializeField] public CanvasGroup canvasGroup { get; private set; }
    [field : SerializeField] public float duration { get; private set; }
    [field : SerializeField] public float delayBeforeTransition { get; private set; }
    protected Sequence _sequence;
    
    public virtual void Initialize()
    {
    }

    public virtual void DoTransition(bool activate, Action callback)
    {
        _sequence = DOTween.Sequence();
        _sequence.SetUpdate(UpdateType.Normal, true);

        _sequence.SetDelay(delayBeforeTransition);
    }
}

[Serializable]
public class TTUITransition_Fade : TTUITransition
{
    [field : SerializeField]public AnimationCurve animationCurve { get; private set; }

    public override void DoTransition(bool activate, Action callback)
    {
        base.DoTransition(activate, callback);
        float startValue = activate ? 0 : 1;
        float endValue = activate ? 1 : 0;
        canvasGroup.alpha = startValue;
        
        if(animationCurve != null)
            _sequence.Append(canvasGroup.DOFade(endValue, duration).SetEase(animationCurve));
        else
            _sequence.Append(canvasGroup.DOFade(endValue, duration).SetEase(Ease.Linear));
        _sequence.OnComplete(()=> callback?.Invoke());
    }
} 
    
[Serializable]
public class TTUITransition_Move : TTUITransition
{
    [field : SerializeField] public AnimationCurve xAnimationCurve { get; private set; }
    [field : SerializeField] public AnimationCurve yAnimationCurve { get; private set; }
    [field : SerializeField] public Vector2 movement { get; private set; }
    [field : SerializeField] public Color gizmosColor { get; private set; }

    
    RectTransform _transform;
    Vector2 startPosition;
    Vector2 endPosition;

    public override void Initialize()
    {
        base.Initialize();
        _transform = canvasGroup.transform as RectTransform;
        startPosition = _transform.anchoredPosition;
        endPosition = startPosition - movement;
    }
        
    public override void DoTransition(bool activate, Action callback)
    {
        base.DoTransition(activate, callback);
        Vector2 end = activate ? startPosition : endPosition;
        Vector2 start = activate ? endPosition : startPosition;
        _transform.anchoredPosition = start;
        
        if(xAnimationCurve != null)
            _sequence.Append(_transform.DOAnchorPosX(end.x, duration).SetEase(xAnimationCurve));
        else 
            _sequence.Append(_transform.DOAnchorPosX(end.x, duration).SetEase(Ease.Linear));
        
        if (yAnimationCurve != null)
            _sequence.Join(_transform.DOAnchorPosY(end.y, duration).SetEase(yAnimationCurve));
        else
            _sequence.Join(_transform.DOAnchorPosY(end.y, duration).SetEase(Ease.Linear));
        
        _sequence.OnComplete(()=> callback?.Invoke());
    }
} 