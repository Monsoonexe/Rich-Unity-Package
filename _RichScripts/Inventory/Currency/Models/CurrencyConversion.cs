using System;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Runtime.CompilerServices;

namespace RichPackage.InventorySystem.Currency
{
    /// <summary>
    /// Defines a conversion between two currencies.
    /// </summary>
    public sealed class CurrencyConversion : ScriptableObject
    {
        [SerializeField, LabelText(nameof(factor))]
        private float factor = 1.0f;

        [SerializeField, LabelText(nameof(currencyA))]
        private CurrencyDefinition currencyA;

        [SerializeField, LabelText(nameof(currencyB))]
        private CurrencyDefinition currencyB;

        [SerializeField, LabelText(nameof(useInAutoConversion))]
        private bool useInAutoConversion = false;

        public bool UseInAutoConversion => useInAutoConversion;
        public CurrencyDefinition CurrencyB => currencyB;
        public CurrencyDefinition CurrencyA => currencyA;

        public float Factor => factor;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Includes(CurrencyDefinition query)
        {
            return currencyA == query || currencyB == query;
        }

        /// <remarks>Lossy.</remarks> Conversions of currencies with discrete values is inherintly lossy.
        /// <exception cref="ArgumentException"></exception>
        public CurrencyAmount Convert(CurrencyAmount from, CurrencyDefinition to)
        {
            // validate
            if (from.Currency == null)
                throw new ArgumentException(nameof(from.Currency));
            if (to == null)
                throw new ArgumentException(nameof(to));
            if (!ReferenceEquals(from.Currency, currencyA)
                || !ReferenceEquals(from.Currency, currencyB))
                throw new ArgumentException($"{this} does not handle a conversion between {from.Currency} and {to}");

            // redundancy check
            if (from == 0 || ReferenceEquals(from.Currency, to))
            {
                return from;
            }

            // apply conversion factor
            float convertedAmount = currencyA == to
                ? from.Amount * factor
                : from.Amount / factor;

            int final = (int)Math.Round(convertedAmount, MidpointRounding.AwayFromZero);
            return new CurrencyAmount(to, final);
        }

        /// <remarks>Lossy.</remarks> Conversions of currencies with discrete values is inherintly lossy.
        public CurrencyAmount Convert(ref CurrencyAmount from, CurrencyDefinition to)
        {
            // validate
            if (from.Currency == null)
                throw new ArgumentException(nameof(from.Currency));
            if (to == null)
                throw new ArgumentException(nameof(to));
            if (!ReferenceEquals(from.Currency, currencyA)
                || !ReferenceEquals(from.Currency, currencyB))
                throw new ArgumentException($"{this} does not handle a conversion between {from.Currency} and {to}");

            // redundancy check
            if (from == 0 || ReferenceEquals(from.Currency, to))
            {
                return from;
            }

            bool mul = currencyA == to;
            int convertedAmount;
            int takeAmount;

            // to or from operation
            if (mul)
            {
                Assertions.Assert.IsTrue(factor >= 1, "Expected positive conversion ratio");

                // apply conversion factor
                convertedAmount = (int)(from.Amount * factor);
                takeAmount = from.Amount;
            }
            else
            {
                // apply conversion factor
                // TODO - test this
                convertedAmount = (int)(from.Amount / factor);
                takeAmount = from.Amount - (int)Math.Round(from.Amount % factor, MidpointRounding.AwayFromZero);
            }

            from.Amount -= takeAmount;
            return new CurrencyAmount(to, (int)convertedAmount);
        }
    }
}
