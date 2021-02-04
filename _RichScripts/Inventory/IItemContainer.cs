using System.Collections.Generic;

/// <summary>
/// Add/Remove things from a collection of items.
/// </summary>
/// <seealso cref="Inventory"/>
/// <seealso cref="InventoryUIDisplayer"/>
public interface IItemContainer
{
    int CapacityLimit { get; }
    string ContainerName { get; }
    int StackCount { get; }
    List<ItemStack> Stock { get; }

    void AddItem(ref ItemStack stack);
    void AddToStackAtIndex(ref ItemStack item, int index);
    bool CanAddItem(Item item, int amount);
    bool CanAddItem(ItemStack stack);
    void ChangeCapactity(int newCapacity);
    bool HasAtLeast(Item item, int amount);
    //bool HasAtLeast(ItemStack stack); //TODO - would be nice to have
    int ItemCount(string itemID);
    int ItemCount(Item item);
    void RemoveAll();
    /// <summary>
    /// 
    /// </summary>
    /// <param name="item"></param>
    /// <param name="amount"></param>
    /// <param name="greedy">Greedy vs stingy, think clerk vs bully. Both want your 
    /// money, but the clerk will only take it if you have enough of it. The bully
    /// just takes what you have up to that limit.</param>
    /// <returns></returns>
    ItemStack RemoveItem(Item item, int amount, bool greedy = false);
    /// <summary>
    /// 
    /// </summary>
    /// <param name="item"></param>
    /// <param name="amount"></param>
    /// <param name="index"></param>
    /// <param name="greedy">Greedy vs stingy, think clerk vs bully. Both want your 
    /// money, but the clerk will only take it if you have enough of it. The bully
    /// just takes what you have up to that limit.</param>
    /// <returns></returns>
    ItemStack RemoveFromStackAtIndex(int amount, int index, bool greedy = false);

    //ItemStack RemoveItem(ItemStack stack, bool greedy = false); //TODO would be nice to have
}
