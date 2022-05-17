using UnityEngine;

namespace RichPackage.TriggerVolumes
{
    /// <summary>
    /// Useful if you have an object with multiple colliders
    /// </summary>
    /// <seealso cref="TriggerColliderRelay"/>
    /// <seealso cref="CollisionRelay"/>
    [RequireComponent(typeof(Collider))]
    public abstract class AColliderRelay : RichMonoBehaviour
    {
        //member Components
        protected Collider myCollider;

        protected override void Awake()
        {
            base.Awake();
            myCollider = GetComponent<Collider>();
        }
    }
}
