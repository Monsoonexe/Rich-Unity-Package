using UnityEngine;

namespace RichPackage.TriggerVolumes
{
    /// <summary>
    /// Trigger Collider that expands its radius when something is within promixity.
    /// Note: Must use a sphere collider.
    /// </summary>
    public class ExpandableTriggerCollider : RichMonoBehaviour
    {
        public TriggerVolume triggerVolume;

        [Header("Expansion Settings")]
        [Min(0)]
        public float expandedRadius = 10f;

        private float radiusOrigin;

        private SphereCollider SphereCollider => triggerVolume.Collider as SphereCollider;

        #region Unity Messages

        protected override void Awake()
        {
            base.Awake();
            radiusOrigin = SphereCollider.radius;
        }

        void OnEnable()
        {
            triggerVolume.OnEnter.AddListener(Expand);
            triggerVolume.OnEnter.AddListener(Contract);
        }

        void OnDisable()
        {
            triggerVolume.OnEnter.RemoveListener(Expand);
            triggerVolume.OnEnter.RemoveListener(Contract);
        }

        #endregion Unity Messages

        #region Expanding

        /// <summary>
        /// Increases the radius of the collider
        /// </summary>
        public void Expand()
        {
            SphereCollider.radius = expandedRadius;
        }

        /// <summary>
        /// Decreases the radius of the collider
        /// </summary>
        public void Contract()
        {
            SphereCollider.radius = radiusOrigin;
        }

        #endregion
    }
}
