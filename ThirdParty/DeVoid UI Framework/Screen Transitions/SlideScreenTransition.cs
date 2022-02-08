﻿using System;
using DG.Tweening;
using UnityEngine;

/// <summary>
/// 
/// </summary>
/// <seealso cref="ScaleScreenTransition"/>
public class SlideScreenTransition : ATransitionComponent
{
    public enum Position
    {
        None = 0,
        Left = 1,
        Right = 2,
        Top = 3,
        Bottom = 4,
    }

    [SerializeField] protected Position origin = Position.Left;
    [SerializeField] protected bool isOutAnimation;
    [SerializeField] protected bool doFade;
    [SerializeField] protected float fadeDurationPercent = 0.5f;
    [SerializeField] protected Ease ease = Ease.Linear;

    private RectTransform rTransform;
    private Action callback;

    public Position Origin {
        get { return origin; }
        set { origin = value; }
    }

    public override void Animate(Transform target, Action callWhenFinished = null)
    {
        IsAnimating = true;
        rTransform = target as RectTransform;
        callback = callWhenFinished;

        var origAnchoredPos = rTransform.anchoredPosition;
        Vector3 startPosition = Vector3.zero;

        switch (origin) {
            case Position.Left:
                startPosition = new Vector3(-rTransform.rect.width, 0.0f, 0.0f);
                break;
            case Position.Right:
                startPosition = new Vector3(rTransform.rect.width, 0.0f, 0.0f);
                break;
            case Position.Top:
                startPosition = new Vector3(0.0f, rTransform.rect.height, 0.0f);
                break;
            case Position.Bottom:
                startPosition = new Vector3(0.0f, -rTransform.rect.height, 0.0f);
                break;
        }

        rTransform.anchoredPosition = isOutAnimation ? Vector3.zero : startPosition;

        rTransform.DOKill();

        CanvasGroup canvasGroup = null;
        if (doFade) 
        {
            canvasGroup = rTransform.GetComponent<CanvasGroup>() //TODO - null coalescing could be a lie!
                ?? rTransform.gameObject.AddComponent<CanvasGroup>();
            
            canvasGroup.DOFade(isOutAnimation ? 0f : 1f, duration * fadeDurationPercent);
        }

        rTransform.DOAnchorPos(isOutAnimation ? startPosition : Vector3.zero, duration, true)
            .SetEase(ease)
            .OnComplete(
                () => 
                {
                    IsAnimating = false;

                    rTransform.anchoredPosition = origAnchoredPos;
                    if (canvasGroup != null) {
                        canvasGroup.alpha = 1f;
                    }
                    callback?.Invoke();
                    callback = null;
                })
            .SetUpdate(true);
    }

    public override void Stop()
    {
        rTransform.DOKill();
        callback?.Invoke();
        callback = null;
    }
}
