using UnityEngine;

/// <summary>
/// Common base class for UI elements. Comes with some a handy Hide/Show interface.
/// </summary>
/// <seealso cref="Editor.RichUIElement_Inspector"/>
/// <seealso cref="RichUIButton"/>
[SelectionBase]
public class RichUIElement : RichMonoBehaviour
{
    public RectTransform rectTransform { get => (RectTransform)myTransform; }

    protected virtual void Start()
    {
        UpdateUI();
    }

    /// <summary>
    /// Definitely override this with custom implmentation (canvas.enabled recommended). 
    /// No need for base.ToggleVisuals().
    /// </summary>
    /// <param name="active"></param>
    public virtual void ToggleVisuals(bool active)
    {
        //do show if not showing and should be -- and stop showing if are and should not be
        if (gameObject.activeSelf != active)//ignore if already in requested state
            gameObject.SetActive(active);
    }

    /// <summary>
    /// Show if hidden, hide if shown.
    /// </summary>
    public virtual void ToggleVisuals() => ToggleVisuals(gameObject.activeSelf);

    [ContextMenu("Show")]
    public void Show() => ToggleVisuals(true);

    [ContextMenu("Hide")]
    public void Hide() => ToggleVisuals(false);
    
    /// <summary>
    /// Used to refresh UI with current values.
    /// </summary>
    public virtual void UpdateUI()
    {
        //nada
    }
}

public class RichUIElement<T> : RichUIElement
{
    /// <summary>
    /// The data that this UI Element should show.
    /// </summary>
    public T targetData;

    protected virtual void OnEnable()
    {
        SubscribeToEvents();
        UpdateUI();
    }

    protected virtual void OnDisable()
    {
        UnsubscribeFromEvents();
    }

    protected virtual void SubscribeToEvents()
    {

    }

    protected virtual void UnsubscribeFromEvents()
    {

    }
}
