using UnityEngine;

/// <summary>
/// Common base class for scriptable objects.
/// </summary>
public class RichScriptableObject : ScriptableObject
{
#if UNITY_EDITOR
    [SerializeField]
    [TextArea]
#pragma warning disable 0414
    private string developerDescription = "Enter a description";
#pragma warning restore 

#endif

}

public class RichScriptableObject <T> : RichScriptableObject
{
    [SerializeField]
    private T _value;

    public virtual T Value { get => _value; set => _value = value; }
}
