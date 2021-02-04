using System;
using UnityEngine;

/// <summary>
/// A line defined by endpoints PointA and PointB.
/// </summary>
[Serializable]
public struct TransformLine
{
    public Transform PointA;
    public Transform PointB;
	
	#region Constructors
	
	public TransformLine(Transform a, Transform b)
    {
        PointA = a;
        PointB = b;
    }
	
	#endregion

    /// <summary>
    /// Shortcut to world position.
    /// </summary>
    /// <param name="a"></param>
    public static implicit operator Line3(TransformLine a) 
        => new Line3(a.PointA.position, a.PointB.position);
}
