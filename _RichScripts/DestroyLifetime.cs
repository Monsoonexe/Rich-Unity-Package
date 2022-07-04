using UnityEngine;
using Sirenix.OdinInspector;

namespace RichPackage
{
    /// <summary>
    /// Destroys owning <see cref="GameObject"/> after given time.
    /// </summary>
    public sealed class DestroyLifetime : RichMonoBehaviour
    {
        [Title("Settings")]
        [Min(0)]
        public float lifeTime = 10;

        protected override void Reset()
        {
            base.Reset();
            SetDevDescription("Destroys owning GameObject after given time.");
        }

        private void Start()
        {
            Destroy(gameObject, lifeTime);
        }
    }
}
