using UnityEngine;

/// <summary>
/// Fires event when MyCollider has collided with an Other Collider.
/// Useful for implementing a hitbox.
/// </summary>
public sealed class CollisionRelay : AColliderRelay
{
    [Header("---Events---")]
    [SerializeField]
    private CollisionUnityEvent OnEnterEvent;

    [SerializeField]
    private CollisionUnityEvent OnExitEvent;

    private void OnCollisionEnter(Collision collision)
        => OnEnterEvent.Invoke(collision);

    private void OnCollisionExit(Collision collision)
        => OnExitEvent.Invoke(collision);
}
