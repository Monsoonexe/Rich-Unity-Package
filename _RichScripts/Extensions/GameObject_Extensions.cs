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
    public static void SetActiveTrue(this GameObject a) => a.SetActive(true);

    /// <summary>
    /// Shortcut for a.enabled = false;
    /// </summary>
    /// <param name="a"></param>
    public static void SetActiveFalse(this GameObject a) => a.SetActive(false);
}
