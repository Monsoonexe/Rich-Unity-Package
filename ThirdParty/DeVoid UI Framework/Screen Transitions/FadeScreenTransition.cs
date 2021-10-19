using System;
using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;
using RichPackage.RichDebug;

/// <summary>
/// Fades a CanvasGroup in/out.
/// </summary>
public class FadeScreenTransition : ATransitionComponent, IAnimate
{
    [Header("---Settings---")]
    public bool isOutAnimation;
    public Ease ease = Ease.Linear;

    [Header("---Scene Refs---")]
    [Required]
    public CanvasGroup canvasGroup;

    public bool IsAnimating { get => animationTween != null; }

    //runtime data
    private Tweener animationTween;
    private float cachedAlpha;

    protected override void Awake()
    {
        base.Awake();

        //validate refs
        RichDebug.AssertMyRefNotNull(this, canvasGroup);
    }

    protected override void Reset()
    {
        base.Reset();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public override void Animate(Transform targetXform, 
        Action onCompleteCallback = null)
    {
        //cache
        cachedAlpha = canvasGroup.alpha;

        //create tween
        float fadeAmount = 0.0f;
        if(isOutAnimation)
        {
            fadeAmount = 0.0f;
        }
        else //fade in
        {
            canvasGroup.alpha = 0; //start at zero
            fadeAmount = cachedAlpha; //current amount
        }

        animationTween = canvasGroup.DOFade(fadeAmount, duration);

        //rig on complete callback
        onCompleteCallback += OnAnimationComplete;
        animationTween.OnComplete(onCompleteCallback.Invoke);
    }

    private void OnAnimationComplete()
    {
        animationTween = null;//clear flag
        canvasGroup.alpha = cachedAlpha; //leave it how you found it
    }

    public override void Stop()
        => animationTween?.Kill();
}
