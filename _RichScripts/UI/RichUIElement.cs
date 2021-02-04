using UnityEngine;

/// <summary>
/// Common base class for UI elements. Comes with some a handy Hide/Show interface.
/// </summary>
/// <seealso cref="Editor.RichUIElement_Inspector"/>
/// <seealso cref="RichUIButton"/>
[SelectionBase]
public class RichUIElement : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField]
    [TextArea]
    protected string developerDescription = "Please enter a description";
#endif

    /// <summary>
    /// Definitely override this with custom implmentation (canvas.enabled recommended). 
    /// No need for base.ToggleVisuals().
    /// </summary>
    /// <param name="active"></param>
    public virtual void ToggleVisuals(bool active)
    {
        //do show if not showing and should be -- and stop showing if are and should not be
        if (gameObject.activeSelf != active)//hide
            gameObject.SetActive(active);//if hit ground
    }

    [ContextMenu("Hide")]
    public void Show() => ToggleVisuals(true);

    [ContextMenu("Show")]
    public void Hide() => ToggleVisuals(false);
}

public class RichUIElement<T> : RichUIElement
{
    /// <summary>
    /// The data that this UI Element should show.
    /// </summary>
    public T targetData;
}
