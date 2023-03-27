using UnityEngine;
using System;
using Sirenix.OdinInspector;

namespace RichPackage.InventorySystem.Currency
{
    [Serializable]
    public class CurrencyConversion
    {
        [field: SerializeField, LabelText(nameof(Factor))]
        public float Factor { get; private set; } = 1.0f;

        [field: SerializeField, LabelText(nameof(Currency))]
        public CurrencyDefinition Currency { get; private set; }

        [field: SerializeField, LabelText(nameof(UseInAutoConversion))]
        public bool UseInAutoConversion { get; private set; } = false;
    }
}
