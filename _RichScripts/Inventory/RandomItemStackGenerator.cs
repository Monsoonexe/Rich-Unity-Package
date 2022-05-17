using UnityEngine;

namespace RichPackage.InventorySystem
{
	/// <summary>
	/// 
	/// </summary>
	[CreateAssetMenu(fileName = "RandomItemStackGen",
		menuName = "ScriptableObjects/Random Generators/Item Stack")]
	public class RandomItemStackGenerator : ARandomGeneratorBase
		<WeightedItemStack, ItemStack>
	{
		//exists
	}
}
