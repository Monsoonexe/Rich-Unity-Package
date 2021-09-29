﻿using System.Diagnostics;
using UnityEngine;

/// <summary>
/// Common base class for scriptable objects.
/// </summary>
public class RichScriptableObject : ScriptableObject
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

}

public class RichScriptableObject <T> : RichScriptableObject
{
    [SerializeField]
    protected T _value;

    public virtual T Value { get => _value; set => _value = value; }
    
    public static implicit operator T (RichScriptableObject<T> a)
        => a.Value;
}
