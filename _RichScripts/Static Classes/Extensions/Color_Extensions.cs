using UnityEngine;
using System.Runtime.CompilerServices;

public static class Color_Extensions
{
    /// <summary>
    /// Prefer WithR(float) instead.
    /// </summary>
    /// <param name="a">Vector3 source to copy</param>
    /// <param name="x">x value, if you want</param>
    /// <param name="y">y value, if you want</param>
    /// <param name="z">z value, if you want</param>
    /// <remarks>But seriously, 4 conditionals????</remarks>
    /// <returns></returns>
    public static Color With(this Color c,
        float? r = null, float? g = null, float? b = null,
        float? a = null)
        => new Color
        (
            r.HasValue ? r.Value : c.r,
            g.HasValue ? g.Value : c.g,
            b.HasValue ? b.Value : c.b,
            a.HasValue ? a.Value : c.a
        );

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color WithR(this in Color c, float r)
        => new Color(r, c.g, c.b, c.a);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color WithG(this in Color c, float g)
        => new Color(c.r, g, c.b, c.a);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color WithB(this in Color c, float b)
        => new Color(c.r, c.g, b, c.a);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static Color WithA(this in Color c, float a)
        => new Color(c.r, c.g, c.b, a);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void MakeOpaque(ref this Color c)
        => c.a = 1;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void MakeTransparent(ref this Color c)
        => c.a = 0;

    //can be called as Vector3_Extensions.With(a, ....), but that's ugly, so the 'this'
    //keyword allows you to call the method from an instance, like a.With(...);

    //following two lines are equivalent.
    //Vector3 myVec = new Vector3(myTransform.position.x, 150, myTransform.position.z);

    //Vector3 otherVec = myTransform.position.With(y: 150);
}
