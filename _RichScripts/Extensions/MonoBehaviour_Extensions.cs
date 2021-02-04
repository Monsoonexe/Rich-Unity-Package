using UnityEngine;

/// <summary>
/// Functions I think are cool that Unity should totally add to the MonoBehaviour API.
/// </summary>
public static class MonoBehaviour_Extensions
{
    /// <summary>
    /// Shortcut for a.enabled = true;
    /// </summary>
    /// <param name="a"></param>
    public static void SetEnabled(this MonoBehaviour a) => a.enabled = true;

    /// <summary>
    /// Shortcut for a.enabled = false;
    /// </summary>
    /// <param name="a"></param>
    public static void SetDisabled(this MonoBehaviour a) => a.enabled = false;


    /// <summary>
    /// Shortcut for a.enabled = true;
    /// </summary>
    /// <param name="a"></param>
    public static void SetEnabled(this AudioSource a) => a.enabled = true;

    /// <summary>
    /// Shortcut for a.enabled = false;
    /// </summary>
    /// <param name="a"></param>
    public static void SetDisabled(this AudioSource a) => a.enabled = false;
}
