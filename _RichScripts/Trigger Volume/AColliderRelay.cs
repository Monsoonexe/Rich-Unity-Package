using UnityEngine;

/// <summary>
/// Useful if you have an object with multiple colliders
/// </summary>
/// <seealso cref="TriggerColliderRelay"/>
/// <seealso cref="CollisionRelay"/>

[RequireComponent(typeof(Collider))]
public abstract class AColliderRelay : RichMonoBehaviour
{
    [Header("---Prefab Refs---")]
    [SerializeField]
    protected Collider myCollider;

}
