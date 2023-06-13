using System;
using DG.Tweening;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// UI Window scale in/out animation.
/// </summary>
/// <seealso cref="SlideScreenTransition"/>
public class ScaleScreenTransition : ATransitionComponent
{
    [Title("Options")]
    [SerializeField] 
    protected bool isOutAnimation;

    [SerializeField] 
    protected bool doFade;

    [SerializeField] 
    protected float fadeDurationPercent = 0.5f;

    [SerializeField] 
    protected Ease ease = Ease.Linear;

    [SerializeField] [Range(0f, 1f)] 
    protected float xYSplit = 0.25f;

    [SerializeField, Title("References")]
    private CanvasGroup canvasGroup = null;

    //cache variables to avoid Closure
    private Action callback;
    private RectTransform rTransform;

    public override void Animate(Transform target, Action callWhenFinished)
    {
        IsAnimating = true;
        rTransform = target as RectTransform;
        callback = callWhenFinished;
        //canvasGroup = null; // reset in case not fading
        
        if (doFade)
        {
            canvasGroup = GetComponentOnPrefabAtAllCosts<CanvasGroup>(canvasGroup);
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
            rTransform.localScale = new Vector3(0f, 0f, 1f);
        }

        var xScale = rTransform.DOScaleX(targetScale, duration * xYSplit).SetEase(ease);
        var yScale = rTransform.DOScaleY(targetScale, duration * 1f - xYSplit).SetEase(ease);

        scaleSequence.Join(xScale).Join(yScale);

        scaleSequence.Play();
    }

    private void Cleanup() 
    {
        IsAnimating = false;
        rTransform.localScale = Vector3.one;
        if(canvasGroup)
            canvasGroup.alpha = 1f;
        callback?.Invoke();
        callback = null;
    }

    public override void Stop()
    {
        rTransform.DOKill();
        callback?.Invoke();
        callback = null;
    }
}
