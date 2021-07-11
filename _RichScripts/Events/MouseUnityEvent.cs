using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Raises UnityEvents in response to Mouse Events. 
/// Rig in the Inspector.
/// </summary>
[RequireComponent(typeof(Collider))]
public class MouseUnityEvent : RichMonoBehaviour
{
    [SerializeField]
    private UnityEvent onMouseDownEvent 
        = new UnityEvent();

    public UnityEvent OnMouseDownEvent
    {
        get => onMouseDownEvent;
    }

    [SerializeField]
    private UnityEvent onMouseEnterEvent 
        = new UnityEvent();

    public UnityEvent OnMouseEnterEvent
    {
        get => onMouseEnterEvent;
    }

    [SerializeField]
    private UnityEvent onMouseExitEvent 
        = new UnityEvent();

    public UnityEvent OnMouseExitEvent
    {
        get => onMouseExitEvent;
    }

    [SerializeField]
    private UnityEvent onMouseUpAsButtonEvent 
        = new UnityEvent();

    public UnityEvent OnMouseUpAsButtonEvent
    {
        get => onMouseUpAsButtonEvent;
    }

    private void OnMouseDown()
    {
        onMouseDownEvent.Invoke();
    }

    private void OnMouseEnter()
    {
        onMouseEnterEvent.Invoke();
    }

    private void OnMouseExit()
    {
        onMouseExitEvent.Invoke();
    }

    private void OnMouseUpAsButton()
    {
        onMouseUpAsButtonEvent.Invoke();
    }
}
