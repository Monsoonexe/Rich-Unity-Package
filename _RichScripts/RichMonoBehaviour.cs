using System.Diagnostics;
using UnityEngine;

[SelectionBase]
public class RichMonoBehaviour : MonoBehaviour
{
#if UNITY_EDITOR
    [SerializeField, TextArea]
    private string developerDescription = "Please enter a description or a note.";
#endif

    /// <summary>
    /// This call will be stripped out of Builds. Use anywhere.
    /// </summary>
    /// <param name="newDes"></param>
    [Conditional("UNITY_EDITOR")]
    public void SetDevDescription(string newDes)
    {
#if UNITY_EDITOR
        developerDescription = newDes;
#endif
    }

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
        => myTransform = GetComponent<Transform>();

    /// <summary>
    /// Set a reference to a singleton to the given instance if valid.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="instance"></param>
    /// <param name="singletonRef">Reference to the Singleton object, typically a static class variable.</param>
    /// <returns>False if a SingletonError occured.</returns>
    protected static bool InitSingleton<T>(T instance, ref T singletonRef, 
        bool dontDestroyOnLoad = true)
        where T : RichMonoBehaviour
    {
        var valid = true; //return value
        if(singletonRef == null)
        {   //we are the singleton
            singletonRef = instance;
            if(dontDestroyOnLoad) instance.DontDestroyOnLoad();
        }
        else if(!instance.Equals(singletonRef))
        {   //there are two Singletons
            //throw new SingletonException(string.Format("[SingletonError] Two instances of a singleton exist: {0} and {1}.",
                //instance.ToString(), singletonRef.ToString()));
            valid = false;
        }
        return valid;
    }

    [Conditional("UNITY_EDITOR")]
    public void MarkDirtySafely()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
            UnityEditor.EditorUtility.SetDirty(this);
#endif
    }

    public static T Construct<T>() where T : RichMonoBehaviour
        => new GameObject().AddComponent<T>();

    public static T Construct<T>(string name) where T : RichMonoBehaviour
        => new GameObject(name).AddComponent<T>();
}
