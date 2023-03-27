using Sirenix.OdinInspector;

namespace RichPackage.InventorySystem.Currency
{
	[System.Serializable, ]
	public struct CurrencyAmount
	{
		[HorizontalGroup("A"), LabelWidth(64)]
		public CurrencyDefinition Currency;

        [HorizontalGroup("A"), LabelWidth(64)]
        public int Amount;

		// TODO - helper methods that check currency is correct
		// TODO - operators
		// TODO - IComparable<CurrencyAmount>

		public static implicit operator int(CurrencyAmount currencyAmount) => currencyAmount.Amount;
		public static implicit operator CurrencyDefinition(CurrencyAmount currencyAmount) => currencyAmount.Currency;
	}
}
