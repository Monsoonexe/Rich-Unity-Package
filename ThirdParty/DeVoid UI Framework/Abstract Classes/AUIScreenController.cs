﻿using System;
using UnityEngine;
using UnityEngine.UI;
using RichPackage;
using Sirenix.OdinInspector;

[RequireComponent(typeof(Canvas)), RequireComponent(typeof(GraphicRaycaster))]
public abstract class AUIScreenController<TProps> : RichMonoBehaviour, IUIScreenController
    where TProps : IScreenProperties
{
    [Header("---Transitions---")]
    [Tooltip("Transition IN animation.")]
    [SerializeField]
    private ATransitionComponent transitionINAnimator;
    public ATransitionComponent TransitionINAnimator
    { get => transitionINAnimator; set => transitionINAnimator = value; } // wrap serialized field

    [Tooltip("Transition OUT animation.")]
    [SerializeField]
    private ATransitionComponent transitionOUTAnimator;
    public ATransitionComponent TransitionOUTAnimator
    { get => transitionOUTAnimator; set => transitionOUTAnimator = value; } // wrap serialized field

    /// <summary>
    /// This is the data payload and settings for this screen. You can rig this directly in a prefab and/or pass it when you show this screen.
    /// </summary>
    [Header("---Properties Payload---")]
    [Tooltip( "This is the data payload and settings for this screen. You can rig this directly in a prefab and/or pass it when you show this screen.")]
    [SerializeField]
    private TProps properties;
    protected TProps Properties { get => properties; set => properties = value; }
    
    /// <summary>
    /// Unique Identifier for this ID. Maybe it's prefab name?
    /// </summary>
    [field: SerializeField, LabelText(nameof(ScreenID)),
        PropertyTooltip("[Optional]. If this is null/empty, it'll be set to its prefab name.")]
    public virtual string ScreenID { get; set; }

    /// <summary>
    /// Is this screen currently visible?
    /// </summary>
    /// <value><c>true</c> if visible; otherwise, <c>false</c>.</value>
    [ReadOnly, ShowInInspector]
    public bool IsVisible { get; private set; }

    /// <summary>
    /// Get called when Transition IN Animation is complete.
    /// </summary>
    public Action<IUIScreenController> OnTransitionInFinishedCallback { get; set; }

    /// <summary>
    /// Get called when Transition OUT Animation is complete.
    /// </summary>
    public Action<IUIScreenController> OnTransitionOutFinishedCallback { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value>The close request.</value>
    public Action<IUIScreenController, bool> CloseRequest { get; set; }

    /// <summary>
    /// 
    /// </summary>
    /// <value>The destruction action.</value>
    public Action<IUIScreenController> OnScreenDestroyed { get; set; }

	protected override void Reset()
    {
        gameObject.GetOrAddComponent<Canvas>();
        gameObject.GetOrAddComponent<GraphicRaycaster>();
    }

	protected override void Awake()
    {
        base.Awake();
        //add Canvas to limit redraw.
        gameObject.GetOrAddComponent<Canvas>();
        gameObject.GetOrAddComponent<GraphicRaycaster>();
    }

    protected virtual void OnEnable()
    {
        AddListeners();
    }

    protected virtual void OnDisable()
	{
        RemoveListeners();
	}

    protected virtual void OnDestroy()
    {
        //death rattle
        OnScreenDestroyed?.Invoke(this);

        //release refs
        OnTransitionInFinishedCallback = null; // release ref
        OnTransitionOutFinishedCallback = null; // release ref
        CloseRequest = null; // release ref
        OnScreenDestroyed = null; // release ref
    }

    /// <summary>
    /// Nada.
    /// </summary>
    protected virtual void AddListeners()
    {
        //nada
    }

    /// <summary>
    /// Nada.
    /// </summary>
    protected virtual void RemoveListeners()
    {
        //nada
    }

    /// <summary>
    /// Use this to set load data from properties. Called when Screen is Shown.
    /// </summary>
    protected virtual void OnPropertiesSet()
    {
        //nada
    }

    /// <summary>
    /// When the screen animates out, this is called immediately.
    /// </summary>
    protected virtual void OnHide()
    {
        //nada. 
    }

    /// <summary>
    /// Can override to react to certain conditions.
    /// </summary>
    /// <param name="newProperties"></param>
    protected virtual void SetProperties(TProps newProperties)
    {
        properties = newProperties;
    }

    /// <summary>
    /// Override if any special behavior to be called when the hierarchy is adjusted.
    /// </summary>
    protected virtual void OnHierarchyFix()
    {
        //nada
    }

    private void Animate(ATransitionComponent animator, 
        Action onCompleteCallback, bool isVisible)
    {
        if (!animator)
        {
            //just toggle and move on
            gameObject.SetActive(isVisible);
            onCompleteCallback?.Invoke();
        }
        else // then do the animation
        {
            //if trying to be shown and not already shown
            if(isVisible && !gameObject.activeSelf)
            {
                gameObject.SetActive(true);
            }

            animator.Animate(transform, onCompleteCallback);
        }
    }

    private void OnTransitionINFinished()
    {
        IsVisible = true;

        OnTransitionInFinishedCallback?.Invoke(this);
    }

    private void OnTransitionOUTFinished()
    {
        IsVisible = false;
        gameObject.SetActive(false);

        OnTransitionOutFinishedCallback?.Invoke(this);
    }

    #region API

    public void Hide(bool animate = true)
    {
        //cancel in animation
        transitionINAnimator?.Stop(); // 

        //do animation
        Animate(animate ? transitionOUTAnimator : null, 
            OnTransitionOUTFinished, false);

        // anything extra on Hide
        OnHide(); 
    }

    public void Show(IScreenProperties payload = null)
    {
        //validate 
        if (payload != null)
        {
            if(payload is TProps props)
            {
                SetProperties(props);
            }
            else
            {
                Debug.LogError($"[{nameof(AUIScreenController<TProps>)}] Properties passed have wrong type! " +
					$"({payload.GetType()} instead of {typeof(TProps)}).");

                // default to Inspector values
            }
        }//end validate

        OnHierarchyFix(); // react to change in hierarchy
        OnPropertiesSet(); // validate and load data

        if (!gameObject.activeSelf) // if currently hidden
        {
            //animate with this animator, when finished call this, show?
            Animate(transitionINAnimator, OnTransitionINFinished, true);
        }
        else // already visible, so just do OnFinish callback
        {
            OnTransitionInFinishedCallback?.Invoke(this);
        }

    }//end function

    #endregion
}
