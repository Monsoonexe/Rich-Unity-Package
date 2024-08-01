using System;
using UnityEngine;
using DG.Tweening;
using NaughtyAttributes;
using RichPackage.Assertions;

/// <summary>
/// Fades a CanvasGroup in/out.
/// </summary>
public sealed class FadeScreenTransition : ATransitionComponent
{
    [Header("---Settings---")]
    public bool isOutAnimation;

    /// <summary>
    /// How many seconds to complete the Animation.
    /// </summary>
    [Tooltip("How many seconds to complete the Animation.")]
    [Min(0)]
    public float duration = 0.5f;
    public Ease ease = Ease.Linear;

    [Header("---Scene Refs---")]
    [Required]
    public CanvasGroup canvasGroup;

    public override bool IsAnimating
    {
        get => Tween != null;
        protected set => throw new NotImplementedException(nameof(IsAnimating));
    }

    //public Tween Tween { get => animationTween; }

    //runtime data
    public Tweener Tween { get; private set; }
    private float cachedAlpha;

    protected override void Awake()
    {
        base.Awake();

        //validate refs
        canvasGroup.ShouldNotBeNull();
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

        Tween = canvasGroup.DOFade(fadeAmount, duration);

        //rig on complete callback
        onCompleteCallback += OnAnimationComplete;
        Tween.OnComplete(onCompleteCallback.Invoke);
    }

    private void OnAnimationComplete()
    {
        Tween = null;//clear flag
        canvasGroup.alpha = cachedAlpha; //leave it how you found it
    }

    public override void Stop()
        => Tween?.Kill();
}
