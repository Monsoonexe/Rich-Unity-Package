
using UnityEngine;

/// <summary>
/// 
/// </summary>
public static class Rigidbody_Extensions
{
    public static Rigidbody FreezePosition(this Rigidbody rb)
    {
        // add the freeze position mask
        rb.constraints |= RigidbodyConstraints.FreezePosition;
        return rb;
    }

    public static Rigidbody UnFreezePosition(this Rigidbody rb)
    {
        // remove the freeze position mask
        rb.constraints &= ~RigidbodyConstraints.FreezePosition;
        return rb;
    }
    
    public static Rigidbody FreezeRotation(this Rigidbody rb)
    {
        // add the freeze position mask
        rb.constraints |= RigidbodyConstraints.FreezeRotation;
        return rb;
    }

    public static Rigidbody UnFreezeRotation(this Rigidbody rb)
    {
        // remove the freeze position mask
        rb.constraints &= ~RigidbodyConstraints.FreezeRotation;
        return rb;
    }

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
