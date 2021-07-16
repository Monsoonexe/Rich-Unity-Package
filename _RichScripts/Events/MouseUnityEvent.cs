using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Raises UnityEvents in response to Mouse Events. 
/// Rig in the Inspector.
/// </summary>
[RequireComponent(typeof(Collider))]
public class MouseUnityEvent : RichMonoBehaviour
{
    public UnityEvent onMouseDownEvent = new UnityEvent();

    public UnityEvent onMouseUpAsButtonEvent = new UnityEvent();

    public UnityEvent onMouseEnterEvent = new UnityEvent();

    public UnityEvent onMouseExitEvent = new UnityEvent();

    public bool IsInteractable
    {
        get => myCollider.enabled;
        set => myCollider.enabled = value;
    }

    //member components
    private Collider myCollider;

    protected override void Awake()
    {
        base.Awake();
        myCollider = GetComponent<Collider>();
    }

    public void OnMouseDown()
        => onMouseDownEvent.Invoke();

    public void OnMouseUpAsButton()
        => onMouseUpAsButtonEvent.Invoke();

    public void OnMouseEnter()
        => onMouseEnterEvent.Invoke();

    public void OnMouseExit()
        => onMouseExitEvent.Invoke();
}
