using UnityEngine;

/// <summary>
/// 
/// </summary>
public static class Vector2_Extensions
{
	/// <summary>
	/// Returns a random value between x and y.
	/// </summary>
    public static float RandomRange(this Vector2 range)
        => Random.Range(range.x, range.y);
}
