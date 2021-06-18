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

    public static Vector2 WithX(this Vector2 a, float x)
        => new Vector2(x, a.y);

    public static Vector2 WithY(this Vector2 a, float y)
        => new Vector2(a.x, y);
}
