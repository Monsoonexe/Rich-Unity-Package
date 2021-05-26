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
