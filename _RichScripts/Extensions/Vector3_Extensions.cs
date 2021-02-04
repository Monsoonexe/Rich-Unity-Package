//credit to Jason Weimann and Unity College
using UnityEngine;

/// <summary>
/// 
/// </summary>
public static class Vector3_Extensions
{
    /// <summary>
    /// Handy function to make assigning/copying vectors easier
    /// myPostion = somePosition.With(z = 1);//same, but with different z-value
    /// </summary>
    /// <param name="a">Vector3 source to copy</param>
    /// <param name="x">x value, if you want</param>
    /// <param name="y">y value, if you want</param>
    /// <param name="z">z value, if you want</param>
    /// <returns></returns>
	public static Vector3 With(this Vector3 a,
        float? x = null, float? y = null, float? z = null)
    {
        return new Vector3(
            x.HasValue ? x.Value : a.x,
            y.HasValue ? y.Value : a.y,
            z.HasValue ? z.Value : a.z);
    }
    //can be called as Vector3_Extensions.With(a, ....), but that's ugly, so the 'this'
    //keyword allows you to call the method from an instance, like a.With(...);

    //following two lines are equivalent.
    //Vector3 myVec = new Vector3(myTransform.position.x, 150, myTransform.position.z);

    //Vector3 otherVec = myTransform.position.With(y: 150);
}
