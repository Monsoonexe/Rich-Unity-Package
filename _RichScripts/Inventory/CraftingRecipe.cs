using UnityEngine;

/// <summary>
/// 
/// </summary>
/// <remarks>TODO - make materials and results an array, since they are not resizable 
/// once created.</remarks>
[CreateAssetMenu(fileName = "_CraftRecipe",
    menuName = "ScriptableObjects/Crafting Recipe")]
public class CraftingRecipe : RichScriptableObject
{   //please update Constructors when adding properties!
    [SerializeField]
	private ItemStack[] materials;
    public ItemStack[] Materials { get => materials; }

    [SerializeField]
	private ItemStack[] results;
    public ItemStack[] Results { get => results; }

    /// <summary>
    /// GetPrint the primary result, or rather results[0]; 
    /// </summary>
    /// <remarks>useful because you can't index a ReadOnlyColletion</remarks>
    public ItemStack MainResult { get => results[0]; }

    /// <summary>
    /// GetPrint the primary result, or rather results[0]; 
    /// </summary>
    /// <remarks>useful because you can't index a ReadOnlyColletion,
    /// and avoids making a copy of the ItemStack.</remarks>
    public Item MainResultItem { get => results[0].Item; }

    private void AddResults(IItemContainer itemContainer)
    {   //already checked and has enough space
        var len = results.Length;
        for (var i = 0; i < len; ++i)
        {
            var itemStack = results[i];//get copy for ref
            itemContainer.AddItem(ref itemStack);
        }
    }

    public bool CanCraft(IItemContainer itemContainer)
		=> HasMaterials(itemContainer) && HasSpace(itemContainer);

    public void Craft(IItemContainer itemContainer)
    {
        if (CanCraft(itemContainer))
        {
            RemoveMaterials(itemContainer);
            AddResults(itemContainer);
        }
    }

    /// <summary>
    /// Returns an ItemStack of the first Result from 
    /// </summary>
    /// <param name="itemContainer"></param>
    /// <returns></returns>
    public ItemStack CraftSingle(IItemContainer itemContainer)
    {   //ItemStacks are structs so they don't generate garbage with 'new'
        var resultStack = new ItemStack(null, 0, 0);//return value
        if (CanCraft(itemContainer))
        {
            RemoveMaterials(itemContainer);
            resultStack = MainResult;
        }
        return resultStack;
    }

    public bool HasMaterials(IItemContainer itemContainer)
	{
        var matCount = materials.Length;
        for(var i = 0; i < matCount; ++i)
		{
            var itemAmount = materials[i];//for double use

            if (!itemContainer.HasAtLeast(itemAmount.Item, itemAmount.Amount))
			{
				Debug.LogWarning("You don't have the required materials.");
				return false;
			}
		}
		return true;
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="itemContainer"></param>
    /// <returns>TODO - Doesn't check if can add ALL Results 
    /// (1 spot left with 3 unique, new results will throw false positive)</returns>
	public bool HasSpace(IItemContainer itemContainer)
	{
        var resCount = results.Length;
        var hasSpace = true;//return value
        for(var i = 0; i < resCount; ++i)
		{
            if (!itemContainer.CanAddItem(results[i]))
			{
				Debug.LogWarning("Your inventory is full.");
                hasSpace = false;
                break;
			}
		}
		return hasSpace;
	}

    public int HowManyCanBeCrafted(IItemContainer itemContainer)
    {   //scan each ingredient and take minimum value
        var totalCraftableAmount = int.MaxValue;//return value
        var matCount = materials.Length;
        for (var i = 0; i < matCount; ++i)
        {
            var ingredient = materials[i];
            Debug.Assert(ingredient.Amount > 0, "[CraftingRecipe] Invalid amount: " +
                ingredient.Amount); //validate

            var amtInInventory = itemContainer.ItemCount(ingredient);
            var thisCraftableAmount = amtInInventory / ingredient.Amount;//integer divide
            totalCraftableAmount = (thisCraftableAmount < totalCraftableAmount) //find min
                ? thisCraftableAmount : totalCraftableAmount;
        }

        return totalCraftableAmount;
    }

	private void RemoveMaterials(IItemContainer itemContainer)
	{   //already checked and requirements are indeed met.
        var matCount = materials.Length;
		for (var i = 0; i < matCount; ++i)
        {   //TODO ItemStack RemoveFromStackAtIndex(ItemStack stack, bool greedy = false); 
            var itemStack = materials[i];//cache for function arg
            itemContainer.RemoveItem(itemStack.Item, itemStack.Amount, false);//remove exactly this
		}
	}

    #region Constructors

    /// <summary>
    /// A constructor method because UnityObjects don't let you use a real one.
    /// Note: COPIES given arrays for safety.
    /// </summary>
    /// <param name="mats"></param>
    /// <param name="rults"></param>
    /// <returns></returns>
    public static CraftingRecipe Construct(ItemStack[] mats, ItemStack[] rults)
    {
        var newObj = CreateInstance<CraftingRecipe>();//create
        //create copies of arrays for safety
        newObj.materials = (ItemStack[])mats.Clone();//construct
        newObj.results = (ItemStack[])rults.Clone();

        return newObj;//return ref
    }

    #endregion

}
