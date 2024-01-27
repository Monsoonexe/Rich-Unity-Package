
using UnityEngine;

/// <summary>
/// 
/// </summary>
public static class Rigidbody_Extensions
{
    public static Rigidbody FreezePositionAndRotation(this Rigidbody rb)
    {
        rb.constraints = RigidbodyConstraints.FreezeAll;
        return rb;
    }

    public static Rigidbody UnFreezePositionAndRotation(this Rigidbody rb)
    {
        rb.constraints = RigidbodyConstraints.None;
        return rb;
    }
}
