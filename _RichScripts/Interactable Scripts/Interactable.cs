﻿using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// An IPlayerInteractable that comes with built-in behaviour and events already rigged.
/// </summary>
/// <remarks>Must be tagged "Interactable"</remarks>
[SelectionBase]
[RequireComponent(typeof(Collider))]
public class Interactable : RichMonoBehaviour, IInteractable
{
    //[Header("---Settings---")]
    //public bool autoInteract;

    [Header("---Events---")]
    [SerializeField]
    protected UnityEvent enterRangeEvent = new UnityEvent();

    [SerializeField]
    protected UnityEvent exitRangeEvent = new UnityEvent();

    [SerializeField]
    protected UnityEvent enterHoverEvent = new UnityEvent();

    [SerializeField]
    protected UnityEvent exitHoverEvent = new UnityEvent();

    [SerializeField] 
    protected UnityEvent interactEvent = new UnityEvent();

    [SerializeField]
    protected UnityEvent endInteractEvent = new UnityEvent();

    protected override void Awake()
    {
        base.Awake();
        RegisterWithManger();
    }

    protected virtual void OnDestroy()
        => UnregisterWithManager();

    public virtual void OnEnterHover(PlayerScript player)
        => enterHoverEvent.Invoke();

    public virtual void OnExitHover(PlayerScript player)
        => exitHoverEvent.Invoke();

    public virtual void OnEnterRange(PlayerScript player)
        => enterRangeEvent.Invoke();

    public virtual void OnExitRange(PlayerScript player)
        => exitRangeEvent.Invoke();

    public virtual void Interact(PlayerScript player)
        => interactEvent.Invoke();

    public virtual void EndInteraction()
        => endInteractEvent.Invoke();

    protected static void RegisterWithManager()
        => InteractionManager.RegisterInteractable(this);

    protected static void UnregisterWithManager()
        => InteractionManager.UnregisterInteractable(this);
}
