using System;
using UnityEngine;

public static class Transform_Extensions
{

	///<summary>
	///
	///</summary>
    public static float Angle(this Transform a, Transform b)
        => Vector3.Angle(a.position, b.position);

	///<summary>
	///
	///</summary>
    public static float AngleForward(this Transform a, Transform b)
        => Vector3.Angle(a.forward, b.position);

	///<summary>
	/// 0 <= Y <= 180
	///</summary>
    public static float AngleForwardDOT(this Transform a, Transform b)
    {
        var worldSpaceForwardVector = a.TransformDirection(a.forward.normalized);//what is my forward vector in world space (normalized)
        var worldSpaceTargetDirection = (b.position - a.position).normalized;//direct from me to target (normalized)

        return Vector3.Dot(worldSpaceForwardVector, worldSpaceTargetDirection);//Ax * Bx + Ay * By + Az * Bz
    }

	///<summary>
	/// -180 <= Y <= 180
	///</summary>
    public static float AngleForwardSigned(this Transform a, Transform b)
    {
        
        var targetDir = b.position - a.position;
        var forward = a.forward;
        return Vector3.SignedAngle(targetDir, forward, Vector3.up);
    }

	///<summary>
	///
	///</summary>
    public static float AngleArcTan(this Transform a, Transform b)
    {
        var targetsRelativePosition = a.InverseTransformPoint(b.position);//what is the target's position if it were in MY local space
        
        return Mathf.Atan2(targetsRelativePosition.x, targetsRelativePosition.y) * Mathf.Rad2Deg;//get arc tan, then convert to degrees
    }

    /// <summary>
    /// Perform an action on every Transform on each of its children, etc, recursively.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="newLayer"></param>
    public static void ForEachChildRecursive(this Transform obj, Action<Transform> action)
    {
        var childCount = obj.childCount;
        for (var i = childCount - 1; i >= 0; --i)
        {
            var child = obj.GetChild(i);
            ForEachTransformRecursive(child, action);
        }
    }

    /// <summary>
    /// Perform an action on this Transform and every Transform on each of its children, etc, recursively.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="newLayer"></param>
    public static void ForEachTransformRecursive(this Transform obj, Action<Transform> action)
    {
        action(obj); //perform 
        var childCount = obj.childCount;
        for (var i = childCount - 1; i >= 0; --i)
        {
            var child = obj.GetChild(i);
            ForEachTransformRecursive(child, action);
        }
    }
}
