/*
TODO - replace this with a Stats system
health and mana are virtually identical, but the base class shouldn't be "health"

also, attributes like Strength traditionally work the same way

*/

using UnityEngine;
using UnityEngine.Events;
using ScriptableObjectArchitecture;
using NaughtyAttributes;
using RichPackage;

namespace RichPackage.HealthSystem
{
    /// <summary>
    /// 
    /// </summary>
    /// <remarks></remarks>
    public class HealthController : RichMonoBehaviour, 
        IDamageable, IHealable
    {
        [Header("---Resources---")]
        [SerializeField] 
        private IntReference currentHealth = new IntReference(100);

        [SerializeField] 
        private IntReference maxHealth = new IntReference(100); 

        public int MaxHealth { get => maxHealth; }
        public int CurrentHealth { get => currentHealth; }

        /// <summary>
        /// Current / max health.
        /// </summary>
        [ShowNativeProperty]
        public float HealthRatio { get => (float)currentHealth / maxHealth; }

        [ShowNativeProperty]
        public bool IsDamaged { get => currentHealth < maxHealth; }

        [ShowNativeProperty]
        public bool IsDead { get; private set; }

        #region Events

        [Foldout("---Events---")]
        [SerializeField]
        private UnityEvent healthGainedEvent = new UnityEvent();
        public UnityEvent HealthGainedEvent { get => healthGainedEvent; }

        [Foldout("---Events---")]
        [SerializeField]
        private UnityEvent healthLostEvent = new UnityEvent();
        public UnityEvent HealthLostEvent { get => healthLostEvent; }

        [Foldout("---Events---")]
        [SerializeField]
        private UnityEvent revivedEvent = new UnityEvent();
        public UnityEvent RevivedEvent { get => revivedEvent; }

        [Foldout("---Events---")]
        [SerializeField]
        private UnityEvent deadEvent = new UnityEvent();
        public UnityEvent DeadEvent { get => deadEvent; }

        #endregion

        /// <summary>
        /// Recover given health. 0 > amount means ALL health.
        /// </summary>
        /// <param name="recoverAmount">negative amount for total heal.</param>
        public void RecoverHealth(int recoverAmount = -1)
        {
            if (recoverAmount < 0) //heal all wounds
            {
                currentHealth.Value = maxHealth;
            }
            else
            {
                currentHealth.Value = RichMath.Clamp(
                    currentHealth + recoverAmount, 0, maxHealth);
            }

            healthGainedEvent.Invoke();
        }

        [Button("Recover Full Health")]
        public void RecoverFullHealth()
            => Revive(1);

        /// <summary>
        /// Make not dead and set current health to given ratio of max health.
        /// Range: 0 lt value lte 1.0f
        /// </summary>
        /// <param name="healthRatio">Range: 0 lt value lte 1.0f</param>
        public void Revive(float healthRatio = 1)
        {
            IsDead = false;
            RecoverHealth((int)(healthRatio * maxHealth));
        }
        
        /// <summary>
        /// Change amount of max health available.
        /// </summary>
        /// <param name="amount"></param>
        public void SetMaxHealth(int amount)
        {
            maxHealth.Value = amount;
            currentHealth.Value = currentHealth < maxHealth // clamp currentHealth to new max
                ? currentHealth : maxHealth;
        }

        /// <summary>
        /// Reduce damage.
        /// </summary>
        /// <param name="damageAmount">negative amount for total kill.</param>
        public void TakeDamage(int damageAmount)
        {
            if (damageAmount < 0) //insta-kill
            {
                damageAmount = currentHealth;
            }
            currentHealth.Value = RichMath.Clamp(
                currentHealth - damageAmount, 0, maxHealth);
            healthLostEvent.Invoke();

            if (currentHealth <= 0 && !IsDead)//if this is the moment of death
            {
                IsDead = true;
                deadEvent.Invoke();
            }
        }
        
        /// <summary>
        /// Will <see cref="TakeDamage"/> if <paramref name="amount"/> is below 0. 
        /// Will <see cref="RecoverHealth"/> if <paramref name="amount"/>  is above 0.
        /// Has no effect if <paramref name="amount"/> is 0.
        /// </summary>
        public void ChangeHealth(int amount)
        {
            if (amount > 0)
                RecoverHealth(amount);
            else if (amount < 0)
                TakeDamage(-amount);
        }

    #if UNITY_EDITOR

        [Button]
        private void TestTake5Damage()
            => TakeDamage(5);

    #endif
    }
}
