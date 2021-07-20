using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

/// <summary>
/// Raises UnityEvents in response to Mouse Events. 
/// Rig in the Inspector.
/// </summary>
[RequireComponent(typeof(Collider))]
public class MouseUnityEvent : RichMonoBehaviour
{
    [Header("---Scene Refs---")]
    [Tooltip("[Optional] Assumes self if null.")]
    [SerializeField]
    private Collider myCollider;
    
    [Foldout("---Events---")]
    public UnityEvent onMouseDownEvent = new UnityEvent();

    [Foldout("---Events---")]
    public UnityEvent onMouseUpAsButtonEvent = new UnityEvent();

    [Foldout("---Events---")]
    public UnityEvent onMouseEnterEvent = new UnityEvent();

    [Foldout("---Events---")]
    public UnityEvent onMouseExitEvent = new UnityEvent();

    [ShowNativeProperty]
    public bool IsInteractable
    {
        get => myCollider.enabled;
        set => myCollider.enabled = value;
    }

    protected override void Awake()
    {
        base.Awake();
        if(myCollider == null)
            myCollider = GetComponent<Collider>();
    }

    private void Reset()
    {
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

    [Button]
    public void ToggleInteractable() => IsInteractable = !IsInteractable;
}
