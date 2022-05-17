//credit: William Lau

using UnityEngine;

namespace RichPackage.TriggerVolumes
{
    /// <summary>
    /// Trigger Collider that expands its radius when something is within promixity.
    /// Note: Must use a sphere collider.
    /// </summary>
    [RequireComponent(typeof(SphereCollider))]
    public class ExpandableTriggerCollider : TriggerVolume
    {
        [Header("Expansion Settings")]
        [Min(0)]
        public float expandedRadius = 10f;

        private SphereCollider sphereCollider;
        private float radiusOrigin;

        #region Initialization

        protected override void Awake()
        {
            base.Awake();
            sphereCollider = GetComponent<SphereCollider>();
            radiusOrigin = sphereCollider.radius;
        }

        void OnEnable()
        {
            enterEvent.AddListener(Expand);
            enterEvent.AddListener(Contract);
        }

        void OnDisable()
        {
            enterEvent.RemoveListener(Expand);
            enterEvent.RemoveListener(Contract);
        }
        #endregion

        #region Expanding
        /// <summary>
        /// Increases the radius of the collider
        /// </summary>
        public void Expand()
        {
            sphereCollider.radius = expandedRadius;
        }

        /// <summary>
        /// Decreases the radius of the collider
        /// </summary>
        public void Contract()
        {
            sphereCollider.radius = radiusOrigin;
        }

        #endregion
    }
}
