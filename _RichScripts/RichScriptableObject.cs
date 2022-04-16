using System.Diagnostics;
using UnityEngine;
using Sirenix.OdinInspector;

/// <summary>
/// Common base class for scriptable objects.
/// </summary>
public class RichScriptableObject : ScriptableObject
{
#if UNITY_EDITOR
    [SerializeField, TextArea]
    [PropertyOrder(-5)]
    private string developerDescription = "Please enter a description or a note.";
#endif

    private string _name;

    /// <summary>
    /// Caches the 'name' property on use. Prefer this because
    /// 'name' allocates on every call and is slower due to marshalling.
    /// </summary>
    /// <remarks>'name' allocates on every call.</remarks>
    public string Name 
    {
        get => string.IsNullOrEmpty(_name) ? (_name = name) : _name;
        set => _name = value;
    }

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

    [Conditional("UNITY_EDITOR")]
    public void Editor_MarkDirty()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
            UnityEditor.EditorUtility.SetDirty(this);
#endif
    }
}

public class RichScriptableObject <T> : RichScriptableObject
{
    [SerializeField]
    protected T _value;

    public virtual T Value { get => _value; set => _value = value; }
    
    public static implicit operator T (RichScriptableObject<T> a)
        => a.Value;
}
