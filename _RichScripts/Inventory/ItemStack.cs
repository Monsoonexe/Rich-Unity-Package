using System;
using UnityEngine;

/// <summary>
/// Serializeable Item Stack. Formerlly called "ItemAmount'. Contains runtime data.
/// This should probably be a struct.
/// </summary>
/// <seealso cref="ItemStack_Ext"/>
/// <seealso cref="ItemStack"/>
/// <seealso cref="BaseItemSlot"/>
/// <remarks></remarks>
[Serializable]
public struct ItemStack// value type
{
    [SerializeField]
    private Item _item;

    public Item Item { get => _item; set =>_item = value; }

    [SerializeField]
    private int _amount;

    public int Amount
    {
        get => _amount;
        set
        {
            _amount = Mathf.Clamp(value, 0, MaxAmount);//max value
            if (_amount == 0) Item = null; // disown Item
        }
    }

    public int MaxAmount { get => Item == null ? int.MaxValue : Item.MaximumStacks; }

    /// <summary>
    /// Bullets in the gun, or something.
    /// </summary>
    [Tooltip("Bullets in the gun, or something.")]
    public float SubAmount;

    /// <summary>
    /// Returns False if there is more space in stack.
    /// </summary>
    public bool IsFull => _item != null && _amount >= _item.MaximumStacks;

    public bool IsEmpty => _amount == 0;

    #region Constructors

    /// <summary>
    /// It is acceptable to pass 'null' here
    /// </summary>
    /// <param name="item"></param>
    public ItemStack(Item item = null)
    {
        _item = item;
        _amount = 0;
        SubAmount = 0;
    }

    public ItemStack(Item item, int amount)
    {
        _item = item;
        _amount = amount;
        SubAmount = 0;
    }

    public ItemStack(Item item, int amount, float subAmount)
    {
        _item = item;
        _amount = amount;
        SubAmount = subAmount;
    }

    /// <summary>
    /// Copy Constructor
    /// </summary>
    /// <param name="stack"></param>
    public ItemStack(ItemStack stack)
    {
        this._item = stack._item;
        this._amount = stack._amount;
        this.SubAmount = stack.SubAmount;
    }

    #endregion

    /// <summary>
    /// Take from given Stack and add to own Stack.
    /// </summary>
    /// <remarks>Can be called stack.AddToStack(ref amount)</remarks>
    public void AddToStack(ref ItemStack stack)
    {
        if (_item == null)
        {
            Item = stack.Item;//in case empty stack
            SubAmount = stack.SubAmount; //this may not be 100% accurate behaviour
        }
        Debug.Assert(_item == stack._item,//validate
            "[ItemStack] ItemStacks of different Items being added.");

        var maxStackLimit = MaxAmount;//cache value
        var newAmount = _amount + stack.Amount;
        var excessAmount = newAmount - maxStackLimit;

        if (excessAmount > 0) // amount exceeds stack limits
        {
            Amount = maxStackLimit;//max out stack
        }
        else
        {
            Amount = newAmount;
            excessAmount = 0; // it all fits
        }
        stack.Amount = excessAmount; //return remaining amount
    }

    public void RemoveFromStack(int amount, bool greedy = true)
        => RemoveFromStack(ref amount, greedy);

    /// <summary>
    /// Remove given amount from stack and return remaining amount.
    /// If need to track amount, use SplitStack.
    /// </summary>
    /// <param name="currentStack">From Stack</param>
    /// <param name="removeAmount">Amount to remove</param>
    /// <param name="greedy">Greedy will take any/all up to amount, 
    /// stingy won't take any if can't take all (bully vs clerk)</param>
    /// <remarks>Can be called stack.RemoveFromStack(ref amount)</remarks>
    public void RemoveFromStack(ref int removeAmount, bool greedy = true)
    {
        var amountRemaining = _amount - removeAmount;

        if (amountRemaining >= 0)//had enough money to pay toll
        {
            Amount = amountRemaining;
            removeAmount = 0;
        }
        else//not enough stock for requested removal
        {
            if (greedy)//take as much as able to give.
            { //give me what you have now and we will track the remainder
                Amount = 0;
                removeAmount = -amountRemaining; //negate
            }//if NOT greedy, don't take any if can't afford
            //a shop won't sell you a $3 coffee if you only have $2 --transaction will not happen
        }
        //return this;
    }

    /// <summary>
    /// Create a new ItemStack by taking "newStackAmount" from this stack (stingy).
    /// </summary>
    /// <param name="stack"></param>
    /// <param name="newStackAmount"></param>
    /// <returns>Returns a stack of 0 Amount if newStackAmount > stack.Amount</returns>
    public ItemStack SplitStack(int newStackAmount)
    {
        if (_amount < newStackAmount) // not enough
            return new ItemStack(_item, 0);
        else
        {
            _amount -= newStackAmount;
            return new ItemStack(_item, newStackAmount);
        }
    }

    /// <summary>
    /// Take 'amount' from 'this' stack and give to 'destination' stack (stingy).
    /// </summary>
    /// <param name="stack"></param>
    /// <param name="newStackAmount"></param>
    /// <returns></returns>
    public bool SplitStack(ref ItemStack destination, int newStackAmount)
    {
        var stackPossible = _item == destination._item && _amount >= newStackAmount;
        if (stackPossible)
        {
            _amount -= newStackAmount;
            destination._amount += newStackAmount;
        }
        return stackPossible;
    }

    public override string ToString()
    {
        return 
            (Item != null ? Item.ItemName : "empty")
            +  " " + _amount.ToString();
    }

    //public static ItemStack operator +(ItemStack a, ItemStack b)
    //{
    //    var item = a._item == b._item ? a._item : null;
    //    var amount = item == null ? 0 : a._amount + b._amount;
    //    var sub = a.SubAmount == b.SubAmount ? a.SubAmount : 0;
    //    return new ItemStack(item, amount, sub);
    //}

    //public static ItemStack operator -(ItemStack a, ItemStack b)
    //{
    //    var item = a._item == b._item ? a._item : null;
    //    var amount = item == null ? 0 : a._amount - b._amount;
    //    var sub = a.SubAmount == b.SubAmount ? a.SubAmount : 0;
    //    return new ItemStack(item, amount, sub);
    //}

    public static implicit operator Item(ItemStack a) => a.Item; //explicit conversion
    public static implicit operator int (ItemStack a) => a.Amount;
}
