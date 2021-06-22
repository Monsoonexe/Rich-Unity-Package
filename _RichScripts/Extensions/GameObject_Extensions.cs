using System;
using UnityEngine;

/// <summary>
/// 
/// </summary>
public static class GameObject_Extensions
{
    /// <summary>
    /// Shortcut for a.enabled = true;
    /// </summary>
    /// <param name="a"></param>
    public static void SetActiveTrue(this GameObject a)
        => a.SetActive(true);

    /// <summary>
    /// Shortcut for a.enabled = false;
    /// </summary>
    /// <param name="a"></param>
    public static void SetActiveFalse(this GameObject a)
        => a.SetActive(false);

    /// <summary>
    /// Shortcut for a.enabled = false;
    /// </summary>
    /// <param name="a"></param>
    public static void SetActiveToggle(this GameObject a)
        => a.SetActive(!a.activeSelf);

    /// <summary>
    /// Shortcut for Destroy(gameObject);
    /// </summary>
    /// <param name="a"></param>
    public static void Destroy(this GameObject a)
        => Destroy(a);

    /// <summary>
    /// Perform an action on every Transform on each of its children, etc, recursively.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="newLayer"></param>
    public static void ForEachChildRecursive(this GameObject obj, Action<Transform> action)
    {
        var transform = obj.GetComponent<Transform>();
        var childCount = transform.childCount;
        for (var i = childCount - 1; i >= 0; --i)
        {
            var child = transform.GetChild(i);
            child.ForEachTransformRecursive(action);
        }
    }
}
