using UnityEngine;
using RichPackage.WeightedProbabilities;
using RichPackage.Decks;

namespace RichPackage.InventorySystem
{
	/// <summary>
	/// 
	/// </summary>
	[CreateAssetMenu(fileName = "RandomItemStackGen",
		menuName = "ScriptableObjects/Random Generators/Item Stack")]
	public class RandomItemStackGenerator : CardGenerator
		<WeightedItemStack, ItemStack>
	{
		//exists
	}
}
