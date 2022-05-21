using RichPackage.InventorySystem;

namespace RichPackage.WeightedProbabilities
{
	/// <summary>
	/// Weighted ItemStack.
	/// </summary>
	/// <seealso cref="RandomItemStackGenerator"/>
	[System.Serializable]
	public class WeightedItemStack : AWeightedProbability<ItemStack>
	{
		//exists

		#region Constructors
		
		public WeightedItemStack(int weight, ItemStack stack)
			: base(weight, stack)
		{

		}
		
		#endregion
	}
}
