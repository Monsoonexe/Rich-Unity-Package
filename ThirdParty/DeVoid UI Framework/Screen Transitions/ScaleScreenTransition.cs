using System;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// 
/// </summary>
/// <seealso cref="SlideScreenTransition"/>
public class ScaleScreenTransition : ATransitionComponent
{
    [SerializeField] protected bool isOutAnimation;
    [SerializeField] protected bool doFade;
    [SerializeField] protected float fadeDurationPercent = 0.5f;
    [SerializeField] protected Ease ease = Ease.Linear;
    [SerializeField] [Range(0f, 1f)] 
    protected float xYSplit = 0.25f;

    //cache variables to avoid Closure
    private Action callback;
    private RectTransform rTransform;
    private CanvasGroup canvasGroup;

    public override void Animate(Transform target, Action callWhenFinished)
    {
        rTransform = target as RectTransform;
        callback = callWhenFinished;
        canvasGroup = null; // reset in case not fading
        
        if (doFade) {
            canvasGroup = rTransform.GetComponent<CanvasGroup>()
                ?? rTransform.gameObject.AddComponent<CanvasGroup>();

            canvasGroup.DOFade(isOutAnimation ? 0f : 1f, duration * fadeDurationPercent);
        }

        rTransform.DOKill(); // clear old tweens

        Sequence scaleSequence = DOTween.Sequence();
        scaleSequence.SetUpdate(true)
            .OnComplete(Cleanup);

        float targetScale;

        if (isOutAnimation)
        {
            targetScale = 0.01f;
        }
        else
        {
            targetScale = 1f;
            rTransform.localScale = new Vector3(0f, 0.02f, 0f);
        }

        var xScale = rTransform.DOScaleX(targetScale, duration * xYSplit).SetEase(ease);
        var yScale = rTransform.DOScaleY(targetScale, duration * 1f - xYSplit).SetEase(ease);

        scaleSequence.Append(xScale).Append(yScale);

        scaleSequence.Play();
    }

    private void Cleanup() {
        callback();
        callback = null;
        rTransform.localScale = Vector3.one;
        if(canvasGroup)
            canvasGroup.alpha = 1f;
    }

    public override void Stop()
    {
        rTransform.DOKill();
        callback?.Invoke();
        callback = null;
    }
}
