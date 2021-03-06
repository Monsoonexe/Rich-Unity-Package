﻿using System;
using UnityEngine;

/// <summary>
/// Translates a target transform.
/// </summary>
public abstract class ATransitionComponent : RichMonoBehaviour
{
    /// <summary>
    /// How many seconds to complete the Animation.
    /// </summary>
    [Tooltip("How many seconds to complete the Animation.")]
    [SerializeField]
    protected float duration = 0.5f;

    public float Duration { get => duration; }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="targetXform">What to animate.</param>
    /// <param name="onCompleteCallback">What to do when finished.</param>
    public abstract void Animate(Transform targetXform, Action onCompleteCallback = null);

    public abstract void Stop();
}
