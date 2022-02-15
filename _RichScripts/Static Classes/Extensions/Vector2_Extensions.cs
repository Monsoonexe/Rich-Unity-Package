using UnityEngine;
using System.Runtime.CompilerServices;

/// <summary>
/// Custom helper mehtods for Vector2s.
/// </summary>
public static class Vector2_Extensions
{
    /// <summary>
    /// Returns a random value between x [inclusive] and y [inclusive].
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float RandomRange(this Vector2 range)
        => Random.Range(range.x, range.y);

    /// <Summmary>
    /// Calculates |x - y|.
    /// </Summmary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static float Range(this Vector2 a)
        => RichMath.AbsoluteValue(a.x - a.y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 WithX(this Vector2 a, float x)
        => new Vector2(x, a.y);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Vector2 WithY(this Vector2 a, float y)
        => new Vector2(a.x, y);
}
