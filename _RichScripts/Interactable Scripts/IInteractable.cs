using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

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

    [Foldout("Events")]
    [SerializeField]
    protected UnityEvent enterRangeEvent = new UnityEvent();

    [Foldout("Events")]
    [SerializeField]
    protected UnityEvent exitRangeEvent = new UnityEvent();

    [Foldout("Events")]
    [SerializeField]
    protected UnityEvent enterHoverEvent = new UnityEvent();

    [Foldout("Events")]
    [SerializeField]
    protected UnityEvent exitHoverEvent = new UnityEvent();

    [Foldout("Events")]
    [SerializeField] 
    protected UnityEvent interactEvent = new UnityEvent();

    [Foldout("Events")]
    [SerializeField]
    protected UnityEvent endInteractEvent = new UnityEvent();

    protected override void Awake()
    {
        base.Awake();
        RegisterWithManager();
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

    protected void RegisterWithManager()
        => InteractionManager.RegisterInteractable(this);

    protected void UnregisterWithManager()
        => InteractionManager.UnregisterInteractable(this);
}
