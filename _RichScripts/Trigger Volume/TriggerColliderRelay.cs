using UnityEngine;

/// <summary>
/// Fires event when MyCollider has collided with an Other Collider.
/// Useful for implementing a hitbox.
/// </summary>
public sealed class TriggerColliderRelay : AColliderRelay
{
    [Header("---Events---")]
    [SerializeField]
    private TriggerCollisionUnityEvent OnEnterEvent;

    [SerializeField]
    private TriggerCollisionUnityEvent OnExitEvent;

    private void OnTriggerEnter(Collider other)
        => OnEnterEvent.Invoke(myCollider, other);

    private void OnTriggerExit(Collider other)
        => OnExitEvent.Invoke(myCollider, other);
}
