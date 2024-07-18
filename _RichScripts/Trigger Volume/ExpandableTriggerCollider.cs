//credit: William Lau

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
            triggerVolume.OnEnter.AddListener(Expand);
            triggerVolume.OnEnter.AddListener(Contract);
        }

        void OnDisable()
        {
            triggerVolume.OnEnter.RemoveListener(Expand);
            triggerVolume.OnEnter.RemoveListener(Contract);
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
