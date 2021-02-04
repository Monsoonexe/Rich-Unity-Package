using UnityEngine;

public class RichMonoBehaviour : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField]
    [TextArea]
    protected string developerDescription = "Please enter a description or a note.";
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

}
