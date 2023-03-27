using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace RichPackage.InventorySystem.Currency
{
    [Serializable]
	public partial class CurrencyDefinition : RichScriptableObject, IEquatable<CurrencyDefinition>
    {
        [field: SerializeField, LabelText(nameof(Id))]
        public UniqueID Id { get; private set; }

        [field: SerializeField, LabelText(nameof(SingleName), true)]
        public string SingleName { get; private set; } = "Enter an item name";

        [field: SerializeField, LabelText(nameof(PluralName), true)]
        public string PluralName { get; private set; } = "Enter an item name";

        [field: SerializeField, LabelText(nameof(Description), true)]
        public string Description { get; private set; } = "";

        [field: SerializeField, LabelText(nameof(Icon), true)]
        public Sprite Icon { get; private set; }

        [field: SerializeField, LabelText(nameof(StringFormat), true),
            Tooltip("How should the string be shown?\n{0} = The amount\n{1} = The currency name")]
        public string StringFormat { get; private set; } = "{0} {3}";

        [field: SerializeField, LabelText(nameof(CurrencyConversions), true)]
        public CurrencyConversion[] CurrencyConversions { get; private set; } = Array.Empty<CurrencyConversion>();

        /// <summary>
        /// True if we can we get 0.1 gold (fraction), when false only ints are allowed.
        /// </summary>
        [field: SerializeField, LabelText(nameof(AllowFractions), true)]
        public bool AllowFractions { get; private set; } = true;

        /// <summary>
        /// Useful when you want to "cap" a currency.
        /// For example in your game contains copper, silver and gold. When copper reaches 100 it can be converted to 1 silver.
        /// </summary>
        [field: SerializeField, LabelText(nameof(AutoConvertOnMax), true), Tooltip("Useful when you want to \"cap\" a currency.\r\nFor example in your game contains copper, silver and gold. When copper reaches 100 it can be converted to 1 silver.")]
        public bool AutoConvertOnMax { get; private set; } = false;

        [field: SerializeField, LabelText(nameof(AutoConvertOnMaxAmount), true),
            ShowIf(nameof(AutoConvertOnMax))]
        public float AutoConvertOnMaxAmount { get; private set; } = 100f;

        [field: SerializeField, LabelText(nameof(AutoConvertOnMaxCurrency), true),
            ShowIf(nameof(AutoConvertOnMax))]
        public CurrencyDefinition AutoConvertOnMaxCurrency { get; private set; }

        [field: SerializeField, LabelText(nameof(AutoConvertFractions), true)]
        public bool AutoConvertFractions { get; private set; } = true;

        [field: SerializeField, LabelText(nameof(AutoConvertFractionsToCurrency), true),
            ShowIf(nameof(AutoConvertFractions))]
        public CurrencyDefinition AutoConvertFractionsToCurrency { get; private set; }

        private void Reset()
        {
            Id = UniqueID.New;
        }

        /// <summary>
        /// Convert this currency to the amount given ID.
        /// </summary>
        public float ConvertTo(float amount, CurrencyDefinition currency)
        {
            foreach (var conversion in CurrencyConversions)
            {
                if (conversion.Currency == currency)
                {
                    return amount * conversion.Factor;
                }
            }

            Debug.LogWarning("Conversion not possible no conversion found with currencyID " + currency.Id, this);
            return 0.0f;
        }

        public string ToString(float amount, float minAmount, float maxAmount, string overrideFormat = "")
        {
            try
            {
                // wat?
                return string.Format(overrideFormat == "" ? StringFormat : overrideFormat, amount, minAmount, maxAmount, amount >= -1.0f - float.Epsilon && amount <= 1.0f + float.Epsilon ? SingleName : PluralName);
            }
            catch (Exception ex)
            {
                return $"(Formatting not valid: {ex.GetType().Name})";
            }
        }

        public override string ToString() => PluralName;

        public bool Equals(CurrencyDefinition other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;
            return this.Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as CurrencyDefinition);
        }

        public override int GetHashCode() => Id.GetHashCode();

        public static bool operator ==(CurrencyDefinition a, CurrencyDefinition b) => a.Equals(b);

        public static bool operator !=(CurrencyDefinition a, CurrencyDefinition b) => !(a == b);
    }
}
