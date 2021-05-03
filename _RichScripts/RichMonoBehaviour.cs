using UnityEngine;

public class RichMonoBehaviour : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField]
    [TextArea]
    private string developerDescription = "Please enter a description or a note.";
#endif

    /// <summary>
    /// Cached Transform.
    /// </summary>
    protected Transform myTransform;

    /// <summary>
    /// Cached Transform  
    /// (because native 'transform' is secretly 'GetComponent()' which is expensive on repeat).
    /// </summary>
    public new Transform transform { get => myTransform; }

    protected virtual void Awake()
    {
        myTransform = this.GetComponent<Transform>();
    }

    /// <summary>
    /// Delete this Mono's GameObject.
    /// </summary>
    /// <remarks>Useful for hooking to event.</remarks>
    public void DestroyGameObject() => Destroy(this.gameObject);

}
