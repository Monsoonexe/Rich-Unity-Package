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
    
    public void Enable() => this.enabled = true;
    public void Disable() => this.enabled = false;

    /// <summary>
    /// Set a reference to a singleton to the given instance if valid.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="instance"></param>
    /// <param name="singletonRef">Reference to the Singleton object, typically a static class variable.</param>
    /// <returns>False if a SingletonError occured.</returns>
    private static void InitSingleton<T>(T instance, ref T singletonRef)
    {
        if(singletonRef == null)
        {   //we are the singleton
            singletonRef = instance;
        }
        else if(!singletonRef.Equals(instance))
        {   //there are two Singletons
            throw new SingletonException(string.Format("[SingletonError] Two instances of a singleton exist: {0} and {1}.",
                instance.ToString(), singletonRef.ToString()));
        }
    }

}
