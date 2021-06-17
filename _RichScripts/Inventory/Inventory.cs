using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A collection of items with amounts.
/// </summary>
/// <seealso cref="ItemChest"/>
/// <seealso cref="InventoryUIController"/>
public class Inventory : RichMonoBehaviour, IItemContainer // behaviour
{
    private const string DEFAULT_STASH_NAME = "Item Stash";
    [SerializeField]
    private string inventoryName = DEFAULT_STASH_NAME;
    public string ContainerName { get => inventoryName; }

    /// <summary>
    /// List of all Items in stock. Can be ScriptableObject
    /// data or Runtime data, like an ATool.
    /// </summary>
    [Tooltip("List of all Items in stock. Can be ScriptableObject " +
        "data or Runtime data, like an ATool.")]
	[SerializeField] protected List<ItemStack> stock = new List<ItemStack>();

    /// <summary>
    /// All items  this Inventory has.
    /// </summary>
    public List<ItemStack> Stock { get => stock; }

    [Tooltip("Maximum number of items stacks that will fit in this Inventory.")]
    [SerializeField]
    private int capacityLimit = 16;
       
    /// <summary>
    /// Maximum number of ItemStacks that will fit in this Inventory. 
    /// </summary>
    public virtual int CapacityLimit { get => capacityLimit; set => ChangeCapactity(value); }
    
    /// <summary>
    /// Take from source stack and give to inventory stacks. 
    /// This operation potentially modifies given stack, hence the 'ref'.
    /// </summary>
    /// <param name="stack"></param>
    public virtual void AddItem(ref ItemStack stack)
    {
        Debug.Assert(stack.Item != null, 
            "[Inventory] null item being added", this);

        Debug.Assert(stack.Amount > 0, 
            "[Inventory] amount <= 0, ", this);

        var i = 0;//index
        //add to existing stacks first
        while (stack.Amount > 0 && i < stock.Count)
        {
            var stockItem = stock[i];//COPY of element from array
            if (stockItem.Item == stack.Item)//match with like stacks
            {
                stockItem.AddToStack(ref stack);
                stock[i] = stockItem;//re-assign because it's a struct
                Debug.Assert(stack.Amount >= 0, "[Inventory] amount < 0", this);
            }
            ++i;//increment loop counter
        }

        i = 0; // reset index
        //add remaining to empty stacks next
        while (stack.Amount > 0 && i < stock.Count)
        {
            var stockItem = stock[i];//COPY of element from array
            if (stockItem.Item == null)//add to empty stacks
            {
                stockItem.AddToStack(ref stack);
                stock[i] = stockItem;//re-assign because it's a struct
                Debug.Assert(stack.Amount >= 0, "[Inventory] amount < 0", this);
            }
            ++i;//increment loop counter
        }

        //add the even more remaining as extra slots until capacity limit reached.
        while (stack.Amount > 0 && stock.Count < CapacityLimit)//still more remaining, add new item stack
        {//under normal circumstances, won't loop. But maybe it's a super-sized stack
            var newStack = new ItemStack(stack.Item, 0, stack.SubAmount);
            newStack.AddToStack(ref stack);
            stock.Add(newStack);
        }

    }

    /// <summary>
    /// No overflow, no swap, add stack to given matching stack at index.
    /// UI usually uses this because it has the concept of indices as well.
    /// </summary>
    /// <param name="stack"></param>
    /// <param name="index"></param>
    public void AddToStackAtIndex(ref ItemStack stack, int index)
    {
        if(index >= capacityLimit || index < 0)
        {
            throw new System.IndexOutOfRangeException("[Inventory] " + index 
                + " / " + capacityLimit);
        }
        else
        {
            while (index >= stock.Count)//pad with empty slots
                stock.Add(new ItemStack(null));

            var stockItem = stock[index]; //get COPY
            if (stockItem.Item == null //empty
                || stockItem.Item == stack.Item)//same type items
            {
                stockItem.AddToStack(ref stack);
                stock[index] = stockItem;//re-assign because it's a struct
                Debug.Assert(stack.Amount >= 0, "[Inventory] amount < 0", this);
            }
            else
            {
                Debug.AssertFormat(false,
                    "[Inventory] Items do not match: {0}, {1} ", 
                    stockItem.Item, stack.Item);
            }
        }
    }

    /// <summary>
    /// Returns true if the entire amount can be added to Stock.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    public virtual bool CanAddItem(Item item, int amount)
    {   //subtract available space from amount. if availableSpace > amount, amount will be negative
        amount -= (capacityLimit - stock.Count) * item.MaximumStacks; //check empty space

        if (amount > 0)
        {//inventory mostly full, try to add stack to similar stacks.
            foreach (var stack in stock)
            {
                if (stack.Item == item)//try to stack like items
                {
                    var spaceRemaining = stack.Item.MaximumStacks - stack.Amount;
                    amount -= spaceRemaining > 0 ? spaceRemaining : 0;
                    if (amount <= 0) break;//early exit
                }
            }
        }

        return amount <= 0;
    }

    /// <summary>
    /// Returns true if the entire amount can be added to Stock.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    public virtual bool CanAddItem(ItemStack stack)
        => CanAddItem(stack, stack); // lol implicit conversions.

    /// <summary>
    /// Remove up to (greedy) or all (!greedy) amount from inventory. negative to remove all.
    /// Best practice is to remove one max stack at a time.
    /// </summary>
    /// <param name="item">Item to remove. negative number implies "all".</param>
    /// <param name="amount"></param>
    /// <param name="greedy">If request 10, but only have 9, take em all anyways.</param>
    /// <returns>If greedy, true if a single item removed. If not greedy, ALL items removed.</returns>
    public virtual ItemStack RemoveItem(Item item, int amount, bool greedy = false)
    {
        var amountToRemove = amount; //cache starting value
        if(amount < 0)//remove all if negative
        {
            greedy = true;
            amount = int.MaxValue;
        }

        if(greedy)
        {
            var itemIndex = 0;//while loop iterator
            var itemCount = stock.Count;//cache for loop
            while (amount > 0 && itemIndex < itemCount)
            {
                //var stockItem = stock[itemIndex];
                if (stock[itemIndex].Item == item) // only remove from similar items
                {
                    //yes, this is intentional. getting a struct from a List<struct>
                    //returns a COPY of that element.
                    var temp = stock[itemIndex];//struct
                    temp.RemoveFromStack(ref amount, greedy);//if amount > 0, more needs to be removed
                    stock[itemIndex] = temp;//reassign because struct
                    Debug.Assert(amount >= 0, "[Inventory] amount negative amount! " +
                        "Check your logic, fool!!!!", this);
                }
                ++itemIndex;
            }
        }
        else
        {   //if stingy, can't take any if can't take full amount
            if (amount <= ItemCount(item))
            {
                for(var i = 0; i < stock.Count; ++i)
                {
                    if(stock[i].Item == item)
                    {
                        //yes, this is intentional. getting a struct from a List<struct>
                        //returns a COPY of that element.
                        var tempStack = stock[i];
                        tempStack.RemoveFromStack(ref amount, greedy);
                        stock[i] = tempStack;
                        Debug.Assert(amount >= 0, "[Inventory] amount negative amount! " +
                            "Check your logic, fool!!!!", this);
                        if (amount == 0) break;
                    }
                }
            }
            //else don't have enough stock to cover amount
        }

        //RemoveEmptyItems();//REMOVE empty ItemValues
        return new ItemStack(item, amountToRemove - amount);
    }

    /// <summary>
    /// UI will probably need this, as UI has a concept of indices as well.
    /// </summary>
    /// <param name="item"></param>
    /// <param name="amount"></param>
    /// <param name="index"></param>
    /// <param name="greedy"></param>
    /// <returns>The stack of items removed. Can be discarded.</returns>
    public virtual ItemStack RemoveFromStackAtIndex(int amount, 
        int index, bool greedy = false)
    {
        var temp = stock[index];//struct
        var tempItem = temp.Item; //cache for many uses and changes
        var tempSubAmount = temp.SubAmount;
        var amountToRemove = amount;//cache starting value

        Debug.Assert(tempItem != null,
            "[Inventory] Removing from slot where Item is null: "
            + index.ToString(), this);

        temp.RemoveFromStack(ref amount, greedy);//if amount > 0, more needs to be removed
        stock[index] = temp;//reassign because struct

        //RemoveEmptyItems();//REMOVE empty ItemValues
        return new ItemStack(tempItem, amountToRemove - amount, tempSubAmount);
    }

    /// <summary>
    /// Completely remove all Items from this Inventory. Lossy.
    /// </summary>
    public virtual void RemoveAll()
    {
        stock.Clear(); //TODO more elegantly just reduce stock amount instead of garbage
    }

    /// <summary>
    /// Remove items from inventory who have amounts of 0;
    /// Not really needed, as UI has empty slots mixed with non-empty slots.
    /// </summary>
    /// <remarks>Should probably be called after removing items.</remarks>
    protected virtual void RemoveEmptyItems()
    {
        var itemCount = stock.Count;
        for (var i = 0; i < itemCount; ++i)
        {
            if (stock[i].Amount == 0)
            {
                stock.RemoveAt(i);
                --i;//check this same index on next pass.
                --itemCount;//reel in bounds
            }
        }
    }

    /// <summary>
    /// How many separate stacks are in inventory.
    /// </summary>
    /// <returns></returns>
    public virtual int StackCount => stock.Count;

    /// <summary>
    /// Returns total amount of items.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public virtual int ItemCount(string itemID)
    {   //O(n) complexity. Can be reduced by sorting list, or one major stack divided up
        var amount = 0;//return value
        foreach (var stack in stock)
            if (stack.Item.ID == itemID)
                amount += stack.Amount;
        return amount;
    }

    /// <summary>
    /// Returns total amount of items.
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    public virtual int ItemCount(Item item)
    {   //O(n) complexity. Can be reduced by sorting list, or one major stack divided up
        var amount = 0;//return value
        foreach (var stack in stock)
            if (stack.Item == item)
                amount += stack.Amount;
        return amount;
    }

    /// <summary>
    /// Change the totaly amount of Items this Inventory can hold.
    /// </summary>
    /// <param name="newCapacity"></param>
    public virtual void ChangeCapactity(int newCapacity)
    {
        if(newCapacity < stock.Count)
        {
            Debug.LogError("[Inventory] Capacity change " +
            "will result in loss of inventory items. Operation not completed.", this);
        }
        else
        {
            capacityLimit = newCapacity;
        }
    }

    /// <summary>
    /// Returns 'true' if the inventory contains at least the given amount of Items.
    /// Less iterations that 'ItemCount() lessThan amount' 
    /// </summary>
    /// <param name="item"></param>
    /// <param name="amount"></param>
    /// <returns></returns>
    public bool HasAtLeast(Item item, int amount)
    {
        foreach (var stack in stock)
            if (stack.Item == item)
            {
                amount -= stack.Amount;
                if (amount <= 0)
                    return true;
            }
        return false;
    }

    public ItemStack this [int index] => stock[index];
}
