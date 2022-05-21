using UnityEngine;
using RichPackage.Pooling;
using RichPackage.HealthSystem;

namespace RichPackage.ProjectileSystem
{
    /// <summary>
    /// 
    /// </summary>
    [RequireComponent(typeof(Collider2D))]
    public class Projectile2D : Poolable
    {
        [Header("---Projectile---")]
        public float speed = 5.0f;
        public int damage = 10;

        [NaughtyAttributes.Tag]
        public string boundsTag = string.Empty;

        /// <summary>
        /// Entity which created this projectile
        /// </summary>
        public GameObject ProjectileOwner { get; private set; }

        private void Update()
        {
            var move = Time.deltaTime * speed * myTransform.forward;
            myTransform.position += move;
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            var other = collision.gameObject;
            if (other == ProjectileOwner) return;

            var damageable = other.GetComponent<IDamageable>();
            if(damageable != null)
            {
                damageable.TakeDamage(damage);
                ReturnToPool();
            }
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            var other = collision.gameObject;
            if (other == ProjectileOwner) //don't hit owner
            {
                return;
            }
            else //check if out of bounds
            if(!string.IsNullOrEmpty(boundsTag)
                && other.CompareTag(boundsTag))
            {   //tag matches bounds
                ReturnToPool();
            }
        }

        public void Initialize(GameObject owner)
        {
            ProjectileOwner = owner;
        }
    }
}
