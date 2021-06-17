using UnityEngine;

/// <summary>
/// Useful if you have an object with multiple colliders
/// </summary>
/// <seealso cref="TriggerColliderRelay"/>
/// <seealso cref="CollisionRelay"/>
[RequireComponent(typeof(Collider))]
public abstract class AColliderRelay : RichMonoBehaviour
{
    protected Collider myCollider;

    protected override void Awake()
    {
        base.Awake();
        myCollider = GetComponent<Collider>();
    }
}
