using UnityEngine;
using UnityEngine.Events;

namespace Explore
{
    public class HealthController : RichMonoBehaviour, IDamageable, IHealable
    {
        public const string SPACE_SLASH_SPACE = " / ";

        [SerializeField] private int currentHealth = 100;
        [SerializeField] private int maxHealth = 100;
        [SerializeField] private bool isDead = false;

        public int Max { get => maxHealth; }
        public int Current { get => currentHealth; }
        /// <summary>
        /// Current / max health.
        /// </summary>
        public float Ratio { get => (float)currentHealth / maxHealth; }
        public bool IsDamaged { get => currentHealth < maxHealth; }
        public bool IsDead { get => isDead; }

        public readonly UnityEvent damagedEvent = new UnityEvent();
        public readonly UnityEvent deadEvent = new UnityEvent();
        public readonly UnityEvent revivedEvent = new UnityEvent();
        public readonly UnityEvent healedEvent = new UnityEvent();

        /// <summary>
        /// Recover given health. 0 > amount means ALL health.
        /// </summary>
        /// <param name="recoverAmount">negative amount for total heal.</param>
        public void RecoverHealth(int recoverAmount = -1)
        {
            if (recoverAmount < 0) //heal all wounds
            {
                currentHealth = maxHealth;
            }
            else
            {
                currentHealth = Mathf.Clamp(currentHealth + recoverAmount, 0, maxHealth);
            }
            healedEvent.Invoke();
        }

        /// <summary>
        /// Make not dead and set current health to given ratio of max health.
        /// Range: 0 lt value lte 1.0f
        /// </summary>
        /// <param name="healthRatio">Range: 0 lt value lte 1.0f</param>
        public void Revive(float healthRatio = 1.0f)
        {
            isDead = false;
            RecoverHealth((int) (healthRatio * maxHealth));
        }

        /// <summary>
        /// Change amount of max health available.
        /// </summary>
        /// <param name="amount"></param>
        public void SetMaxHealth(int amount)
        {
            maxHealth = amount;
            currentHealth = currentHealth < maxHealth // clamp currentHealth to new max
                ? currentHealth : maxHealth;
        }

        /// <summary>
        /// Reduce damage.
        /// </summary>
        /// <param name="damageAmount">negative amount for total kill.</param>
        public void TakeDamage(int damageAmount)
        {
            if(damageAmount < 0)
            {
                damageAmount = currentHealth;
            }
            currentHealth = Mathf.Clamp(currentHealth - damageAmount, 0, maxHealth);
            damagedEvent.Invoke();
            
            if(currentHealth <= 0 && !isDead)//if this is the moment of death
            {
                isDead = true;
                deadEvent.Invoke();
            }
        }

        /// <summary>
        /// [current] / [max]
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return currentHealth.ToString() + SPACE_SLASH_SPACE + maxHealth.ToString();
        }

        /* 
        public void TakeDamage(DamagePacket damagePacket, Entity damageSource)
        {
            var damageAmount = damagePacket.DamageAmount;
            if (damageAmount < 0)
            {
                damageAmount = currentHealth;
            }
            currentHealth = Mathf.Clamp(currentHealth - damageAmount, 0, maxHealth);
            damagedEvent.Invoke();

            if (currentHealth <= 0 && !isDead)//if this is the moment of death
            {
                isDead = true;
                deadEvent.Invoke();
            }
        }
        */
    }
}
