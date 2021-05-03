using UnityEngine;

/// <summary>
/// Common base class for scriptable objects.
/// </summary>
public class RichScriptableObject : ScriptableObject
{
#if UNITY_EDITOR
    [SerializeField]
    [TextArea]
#pragma warning disable IDE0044 // Add readonly modifier
    private string developerDescription = "Enter a description";
#pragma warning restore IDE0044 // Add readonly modifier

#endif

}
