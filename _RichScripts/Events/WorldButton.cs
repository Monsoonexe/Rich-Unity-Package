using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Forwards mouse events to UnityEvents.
/// </summary>
[RequireComponent(typeof(Collider))]
public class WorldButton : RichMonoBehaviour
{
    public UnityEvent onMouseDownEvent = new UnityEvent();

    public UnityEvent onMouseUpAsButtonEvent = new UnityEvent();

    public UnityEvent onMouseEnterEvent = new UnityEvent();

    public UnityEvent onMouseExitEvent = new UnityEvent();

    public void OnMouseDown()
        => onMouseDownEvent.Invoke();

    public void OnMouseUpAsButton()
        => onMouseUpAsButtonEvent.Invoke();

    public void OnMouseEnter()
        => onMouseEnterEvent.Invoke();

    public void OnMouseExit()
        => onMouseExitEvent.Invoke();
}
