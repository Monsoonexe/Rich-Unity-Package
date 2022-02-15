using UnityEngine;
using System.Runtime.CompilerServices;


/// <summary>
/// Functions I think are cool that Unity should totally add to the Behaviour API.
/// </summary>
public static class Behaviour_Extensions
{
    /// <summary>
    /// Shortcut for a.enabled = true;
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SetEnabled(this Behaviour a) => a.enabled = true;

    /// <summary>
    /// Shortcut for a.enabled = false;
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SetDisabled(this Behaviour a) => a.enabled = false;

    /// <summary>
    /// Shortcut for a.enabled = !a.enabled;
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void SetEnabledToggle(this Behaviour a) => a.enabled = !a.enabled;
}
