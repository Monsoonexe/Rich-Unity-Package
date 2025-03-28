using RichPackage.GuardClauses;
using RichPackage.PrimitivePointers;
using System.Collections.Generic;

namespace RichPackage.InventorySystem.Currency
{
    public class Wallet
    {
        private readonly Dictionary<CurrencyDefinition, IntObject> items = new Dictionary<CurrencyDefinition, IntObject>();

        #region Constructors

        public Wallet() { }

        public Wallet(IEnumerable<CurrencyDefinition> currencies)
        {
            foreach (CurrencyDefinition c in currencies)
            {
                items.Add(c, new IntObject(0));
            }
        }

        #endregion Constructors

        private IntObject GetOrAdd(CurrencyDefinition key)
        {
            if (!items.TryGetValue(key, out IntObject value))
            {
                items.Add(key, value = new IntObject(0));
            }

            return value;
        }

        public void Add(CurrencyAmount amount)
        {
            IntObject value = GetOrAdd(amount.Currency);
            int sum = amount.Amount + value.Value;
            value.Value = sum.Clamp(0, sum);
        }

        public void Sub(CurrencyAmount amount)
        {
            Add(new CurrencyAmount(amount.Currency, -amount.Amount));
        }

        public bool Spend(CurrencyAmount cost)
        {
            GuardAgainst.ArgumentIsNull(cost.Currency, nameof(cost.Currency));
            GuardAgainst.IsNegative(cost.Amount, nameof(cost.Amount));

            int owned = this[cost.Currency];
            if (cost > owned)
            {
                return false;
            }
            else
            {
                Sub(cost);
                return true;
            }
        }

        public int GetAmount(CurrencyDefinition key)
        {
            GuardAgainst.ArgumentIsNull(key, nameof(key));

            // only allocate if we need to
            return items.TryGetValue(key, out IntObject value)
                ? value.Value
                : 0;
        }

        public IEnumerable<CurrencyDefinition> Currencies => items.Keys;

        public IEnumerable<CurrencyAmount> Amounts
        {
            get
            {
                foreach (KeyValuePair<CurrencyDefinition, IntObject> kvp in items)
                {
                    yield return new CurrencyAmount(kvp.Key, kvp.Value);
                }
            }
        }

        public IEnumerator<CurrencyAmount> GetEnumerator() => Amounts.GetEnumerator();

        public int this[CurrencyDefinition key]
        {
            get => GetAmount(key);
            set => GetOrAdd(key).Value = value;
        }

        public static Wallet operator +(Wallet wallet, CurrencyAmount value)
        {
            wallet.Add(value);
            return wallet;
        }

        public static Wallet operator -(Wallet wallet, CurrencyAmount amount)
        {
            wallet.Sub(amount);
            return wallet;
        }
    }
}
