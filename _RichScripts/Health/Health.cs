using ScriptableObjectArchitecture;
using System;
using UnityEngine.Assertions;

namespace ApexOfficer
{
    [Serializable]
    public class Health
    {
        [UnityEngine.SerializeField]
        public IntVariable Amount;
        public object Owner { get; set; } // probably unnecessary
        public bool IsInjured => Amount < Amount.MaxClampValue;
        public bool IsAlive => Amount > Amount.MinClampValue;
        public bool IsFull => Amount == Amount.MaxClampValue;
        public bool IsDead => !IsAlive;
        public float Ratio => Amount / (float)Amount.MaxClampValue;
        public int Min
        {
            get => Amount.MinClampValue;
        }
        public int Max
        {
            get => Amount.MaxClampValue;
        }

        #region Events

        public event Action<Health> OnInjured;
        public event Action<Health> OnBecameInjured;
        public event Action<Health> OnDead;
        public event Action<Health> OnRecovered;
        public event Action<Health> OnRevived;

        #endregion Events

        #region Constructors

        public Health() : this(0, 100, 100) { }

        public Health(IntVariable amount)
        {
            Amount = amount;
        }

        public Health(int min, int max, int current)
        {
            Amount = IntVariable.Create(current, min, max);
        }

        #endregion Constructors

        #region Modification

        /// <returns>New amount.</returns>
        public int Modify(int amount)
        {
            // no change
            if (amount == 0)
                goto exit;

            // determine results
            bool isGaining = amount > 0;
            bool wasFullHealth = IsFull;
            bool wasDead = IsDead;
            bool wasAlive = IsAlive;

            // don't bother going negative
            if (wasDead && !isGaining)
                goto exit;

            // don't bother going over max
            if (wasFullHealth && isGaining)
                goto exit;

            // math
            Amount.Add(amount);

            // raise events
            if (isGaining)
            {
                OnRecovered?.Invoke(this);
                if (wasDead)
                    OnRevived?.Invoke(this);
            }
            else
            {
                OnInjured?.Invoke(this);
                if (wasFullHealth)
                    OnBecameInjured?.Invoke(this);
                if (IsDead)
                    OnDead?.Invoke(this);
            }

        exit:
            return Amount;
        }

        /// <summary>
        /// Sets current health value to max.
        /// </summary>
        public void SetToMax() => Modify(int.MaxValue);

        /// <summary>
        /// Sets current health to min.
        /// </summary>
        public void SetToMin() => Modify(int.MinValue);

        #endregion Modification

        public void SetBounds(int min, int max)
        {
            Assert.IsTrue(min < max);
            Assert.IsFalse(true, "impl removed");
            /*
			Min = min;
			Max = max;
			*/
        }

        public override string ToString() => $"{Amount.Value} | {Max}";

        #region Operators

        public static implicit operator int(Health health) => health.Amount;
        public static int operator +(Health health, int amount) => health.Modify(amount);
        public static int operator -(Health health, int amount) => health.Modify(-amount);

        #endregion Operators
    }
}
