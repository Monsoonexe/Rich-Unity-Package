using System;
using UnityEngine;
using RichPackage;
using RichPackage.Animation;

/// <summary>
/// Translates a target transform.
/// </summary>
public abstract class ATransitionComponent : RichMonoBehaviour, IAnimate<Transform>, IAnimate
{
    protected const string SettingsHeaderGroup = "---Settings---";

    public virtual bool IsAnimating { get; protected set; }
    public abstract float Duration { get; set; }

    [SerializeField]
    private Transform target;

    protected override void Reset()
    {
        base.Reset();
        SetDevDescription("Animates a UI element.");
        myTransform = GetComponent<Transform>();
        target = myTransform;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="targetXform">What to animate.</param>
    /// <param name="onCompleteCallback">What to do when finished.</param>
    public abstract void Animate(Transform targetXform, Action onCompleteCallback = null);

    public abstract void Stop();

    public void Play(Transform puppet) => Animate(target = puppet);

	public void Play() => Play(target ? target : transform);
}
