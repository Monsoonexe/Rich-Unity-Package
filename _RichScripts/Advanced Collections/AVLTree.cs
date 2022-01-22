using System;
using System.Collections.Generic;

/*  TODO - pool Nodes in stack to reduce garbage / fragmentation
 *  FixMe - TryFindRemove() costs 2 traversals, one for find, one to delete
 *  fixme - AddIfNew() costs 2 traversals
 */

/// <summary>
/// Target {compare} Item.If value >= 1, then testItem is greater than other. <br/>
/// If value == 0, then testItem is equal to other. <br/>
/// If value <= -1, then testItem is less than other. <br/>
/// </summary>
public delegate int Comparer(T testItem);

/// <summary>
/// Self-balancing AVL tree. Search is Log2(n).
/// Creates garbage on remove.
/// </summary>
/// <typeparam name="T">Class or Struct that impements the <see name="IComparable"/see> interface.</typeparam>
/// <remarks>Don't modify the IComparable pivot value while
/// the item is in a tree. Will break BST aspect.
/// No duplicates allowed!</remarks>
public class AVLTree<T> where T : IComparable
{
    class AVLNode<TNode> where TNode : IComparable
    {
        #region Properties

        public TNode data;

        public AVLNode<TNode> left;

        public AVLNode<TNode> right;

        #endregion

        #region Constructors

        public AVLNode(TNode data)
        {
            this.data = data;
        }

        #endregion
    }

    #region Properties

    private AVLNode<T> root;

    //private Class

    /// <summary>
    /// Count of items in tree.
    /// </summary>
    public int Count { get; private set; }

    /// <summary>
    /// Height <= 1.441 * Log2(Count)
    /// </summary>
    public int Height { get => GetHeight(root); }

    /// <summary>
    /// Quick calculation of the height. Upper bound: Height <= 1.441 * Log2(Count)
    /// </summary>
    public int HeightEstimate => Math.Log2(Count) * 1.441f;

    #endregion

    #region Constructors

    public AVLTree()
    {
        Count = 0;
    }

    public AVLTree(in IList<T> source)
    {
        var count = source.Count;
        for (var i = 0; i < count; ++i)
            Add(source[i]);
    }

    public AVLTree(in IEnumerable<T> source)
    {
        foreach (var item in source)
            Add(item);
    }

    public AVLTree(in ICollection<T> source)
    {
        foreach (var item in source)
            Add(item);
    }

    #endregion

    /// <summary>
    /// Insert an item into the data set. O(Log2(n))
    /// </summary>
    /// <param name="data"></param>
    public void Add(in T data)
    {
        RecursiveInsert(ref root, new AVLNode<T>(data));
    }

    public void AddIfNew(in T data)
    {   //2 traversals unless optimized
        if (!Contains(data))
            Add(data);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    public bool Contains(in T key)
    {
        var foundNode = FindNode(key, root);
        return foundNode != null
            && foundNode.data.CompareTo(key) == 0;
    }

    /// <summary>
    /// Remove node with target data.
    /// </summary>
    /// <param name="target"></param>
    public void Remove(in T target)
    {
        --Count;//assume it was found, and backtrack if not
        Remove(ref root, target);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="current"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    private void Remove(ref AVLNode<T> current, in T target)
    {
        int compareResult = 0;
        if (current == null)
        {
            ++Count;//undo wrong assumption (janky, but cheap).
            return; //not found
        }
        else
        {   //left subtree
            if ((compareResult = target.CompareTo(current.data)) < 0) //get and stash compare result for next else if
            {
                Remove(ref current.left, target);
                if (BalanceFactor(current) == -2)//here
                {
                    if (BalanceFactor(current.right) <= 0)
                        current = RotateRR(current);
                    else
                        current = RotateRL(current);
                }
            }
            //right subtree
            else if (compareResult > 0)
            {
                Remove(ref current.right, target);
                if (BalanceFactor(current) == 2)
                {
                    if (BalanceFactor(current.left) >= 0)
                        current = RotateLL(current);
                    else
                        current = RotateLR(current);
                }
            }
            else  //target is found!
            {   //adjust tree
                if (current.right != null)
                {
                    //delete its inorder successor
                    AVLNode<T> parent = current.right;
                    while (parent.left != null)
                        parent = parent.left;
                    current.data = parent.data;

                    //rebalancing
                    Remove(ref current.right, current.data);
                    if (BalanceFactor(current) == 2)
                    {
                        if (BalanceFactor(current.left) >= 0)
                            current = RotateLL(current);
                        else
                            current = RotateLR(current);
                    }
                }
                else //if current.right == null
                {
                    current = current.left;
                    return;
                }
            }
        }
    }

    /// <summary>
    /// Returns the item that matches given predicate,
    /// or 'default'. If {Type} is a class, 'value' can be a
    /// partially-complete record.
    /// </summary>
    /// <param name="predicate">Condition.</param>
    /// <param name="value">Condition.</param>
    /// <returns>True if item was found and removed.</returns>
    public bool TryGetRemove(Predicate<T> predicate, 
        out T value)
    {
        var found = TryInOrderSearch(//arbitrarily use this method
            predicate, out value);
        if (found)
            Remove(ref root, value);//requires another shorter search
        return found;
    }

    /// <summary>
    /// Returns the item that matches given predicate,
    /// or 'default'. If {Type} is a class, 'value' can be a
    /// partially-complete record.
    /// </summary>
    /// <param name="comparer">Target {comp} Item.</param>
    /// <param name="value">Condition.</param>
    /// <returns>True if item was found and removed.</returns>
    public bool TryGetRemove(Comparer comparer,
        out T value)
    {
        value = default;
        //costs 2 traversals unless I make a special find-remove recursive function
        var foundNode = FindNode(comparer, root);
        var nodeWasFound = foundNode != null; //cache for return
        if (nodeWasFound)
        {
            value = foundNode.data;
            Remove(ref root, value);//requires another search
        }
        return nodeWasFound;
    }

    /// <summary>
    /// Search against key query using binary search.
    /// </summary>
    /// <param name="key">Dummy value used as comparison.</param>
    /// <param name="foundItem">Holds item that matched querry.</param>
    /// <returns>True if item was found and returned.</returns>
    public bool TryFind(in T key, out T foundItem)
    {
        foundItem = default;
        var foundNode = FindNode(key, root);
        var searchSuccessful = foundNode != null;
        if (searchSuccessful)
            foundItem = foundNode.data;

        return searchSuccessful;
    }

    /// <summary>
    /// Returns the item that compared equal or 'default'. 
    /// If {Type} is a class, 'value' can be a
    /// partially-complete record.
    /// </summary>
    /// <param name="comparer">Target {comp} Item.</param>
    public bool TryFind(Comparer compareMethod, 
        out T value)
    {
        value = default;
        var foundNode = FindNode(compareMethod, root);
        bool searchSuccessful = foundNode != null;
        if (searchSuccessful)
            value = foundNode.data;
        return searchSuccessful;
    }

    /// <summary>
    /// Recuresive binary search based on how item 
    /// compares in a given method.
    /// </summary>
    /// <returns>Null if not found.</returns>
    private static AVLNode<T> FindNode(Comparer compareMethod,
        AVLNode<T> current)
    {
        if (current == null) return null; //not found

        var compareResult = compareMethod(current.data);
        if (compareResult > 0)
            return FindNode(compareMethod, current.right);
        else if (compareResult < 0)
            return FindNode(compareMethod, current.left);
        else
            return current; // found!
    }

    private AVLNode<T> FindNode(in T target)
        => FindNode(target, root);

    /// <summary>
    /// Recursive binary search.
    /// </summary>
    /// <param name="target">Partially-filled record.</param>
    /// <returns>Null if not found.</returns>
    private static AVLNode<T> FindNode(in T target, 
        AVLNode<T> current)
    {
        if (current == null) return null;

        var compareResult = target.CompareTo(current.data);
        if (compareResult > 0)
            return FindNode(target, current.right); 
        else if (compareResult < 0)
            return FindNode(target, current.left);
        else
            return current; // found!
    }

    private void RecursiveInsert(
        ref AVLNode<T> current, AVLNode<T> newNode)
    {
        int compareResult = 0;
        if (current == null)
        {
            ++Count;
            current = newNode;
        }
        else if ((compareResult = newNode.data.CompareTo(current.data)) < 0)//stash result and check if less than 0
        {
            RecursiveInsert(ref current.left, newNode);
            BalanceTree(ref current);
        }
        else if (compareResult > 0)
        {
            RecursiveInsert(ref current.right, newNode);
            BalanceTree(ref current);
        }
    }

    #region In-Order Processing

    /// <summary>
    /// LPR
    /// </summary>
    public void InOrderProcessTree(Action<T> process)
    {
        if (root == null) return;
        InOrderProcessTree(root, process);
    }

    /// <summary>
    /// LPR
    /// </summary>
    /// <param name="current"></param>
    /// <param name="process"></param>
    private static void InOrderProcessTree(
        AVLNode<T> current, Action<T> process)
    {
        if (current != null)
        {
            InOrderProcessTree(current.left, process);
            process(current.data);
            InOrderProcessTree(current.right, process);
        }
    }

    /// <summary>
    /// Returns first item where predicate returns true.
    /// </summary>
    public bool TryInOrderSearch(
        Predicate<T> predicate, out T value)
    {
        value = default;
        return TryInOrderSearch(root, 
            predicate, ref value);
    }

    private static bool TryInOrderSearch(
        AVLNode<T> current, Predicate<T> predicate,
        ref T value)
    {
        if (current != null)
        {
            //check left
            if(TryInOrderSearch(current.left, 
                predicate, ref value))
                return true;

            //check this
            if (predicate(current.data))
            {
                value = current.data;
                return true;
            }

            //check right
            if (TryInOrderSearch(current.right, 
                predicate, ref value))
                return true;
        }
        return false;
    }

    #endregion

    #region Post-Order Processing

    /// <summary>
    /// LRP
    /// </summary>
    public void PostOrderProcessTree(Action<T> process)
    {
        if (root == null)
        {
            return;
        }

        PostOrderProcessTree(root, process);
    }

    private static void PostOrderProcessTree(
        AVLNode<T> current, Action<T> process)
    {
        if (current != null)
        {
            PostOrderProcessTree(current.left, process);
            PostOrderProcessTree(current.right, process);
            process(current.data);
        }
    }

    /// <summary>
    /// Returns first item where predicate returns true.
    /// </summary>
    public bool TryPostOrderSearch(
        Predicate<T> predicate, out T value)
    {
        value = default;
        return TryPostOrderSearch(root, 
            predicate, ref value);
    }

    private static bool TryPostOrderSearch(
        AVLNode<T> current, Predicate<T> predicate,
        ref T value)
    {
        if (current != null)
        {
            //check left
            if (TryPostOrderSearch(current.left,
                predicate, ref value))
                return true;

            //check right
            if (TryPostOrderSearch(current.right,
                predicate, ref value))
                return true;

            //check this
            if (predicate(current.data))
            {
                value = current.data;
                return true;
            }
        }
        return false;
    }

    #endregion

    #region Pre-Order Processing

    /// <summary>
    /// PLR
    /// </summary>
    public void PreOrderProcessTree(Action<T> process)
    {
        if (root == null)
        {
            return;
        }

        PreOrderProcessTree(root, process);
    }

    /// <summary>
    /// PLR
    /// </summary>
    private static void PreOrderProcessTree(
        AVLNode<T> current, Action<T> process)
    {
        if (current != null)
        {
            process(current.data);
            PreOrderProcessTree(current.left, process);
            PreOrderProcessTree(current.right, process);
        }
    }

    /// <summary>
    /// Returns first item where predicate returns true.
    /// </summary>
    public bool TryPreOrderSearch(
        Predicate<T> predicate, out T value)
    {
        value = default;
        return TryPreOrderSearch(root,
            predicate, ref value);
    }

    private static bool TryPreOrderSearch(
        AVLNode<T> current, Predicate<T> predicate,
        ref T value)
    {
        if (current != null)
        {
            //check this
            if (predicate(current.data))
            {
                value = current.data;
                return true;
            }

            //check left
            if (TryPreOrderSearch(current.left,
                predicate, ref value))
                return true;

            //check right
            if (TryPreOrderSearch(current.right,
                predicate, ref value))
                return true;
        }
        return false;
    }

    #endregion

    #region Utility

    private static int GetHeight(AVLNode<T> current)
    {
        int height = 0;

        if (current != null)
        {
            int l = GetHeight(current.left);
            int r = GetHeight(current.right);
            int m = l.CompareTo(r) > 0 ? l : r;//max
            height = m + 1;
        }

        return height;
    }

    private static int BalanceFactor(AVLNode<T> current)
    {
        int l = GetHeight(current.left);
        int r = GetHeight(current.right);
        int balanceFactor = l - r;

        return balanceFactor;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="current"></param>
    /// <returns></returns>
    private static void BalanceTree(ref AVLNode<T> current)
    {
        int balanceFactor = BalanceFactor(current);

        if (balanceFactor > 1)
        {
            if (BalanceFactor(current.left) > 0)
            {
                current = RotateLL(current);
            }
            else
            {
                current = RotateLR(current);
            }
        }
        else if (balanceFactor < -1)
        {
            if (BalanceFactor(current.right) > 0)
            {
                current = RotateRL(current);
            }
            else
            {
                current = RotateRR(current);
            }
        }
    }

    #endregion

    #region Rotations

    private static AVLNode<T> RotateRR(AVLNode<T> parent)
    {
        var pivot = parent.right;
        parent.right = pivot.left;
        pivot.left = parent;
        return pivot;
    }

    private static AVLNode<T> RotateLL(AVLNode<T> parent)
    {
        var pivot = parent.left;
        parent.left = pivot.right;
        pivot.right = parent;
        return pivot;
    }

    private static AVLNode<T> RotateLR(AVLNode<T> parent)
    {
        var pivot = parent.left;
        parent.left = RotateRR(pivot);
        return RotateLL(parent);
    }

    private static AVLNode<T> RotateRL(AVLNode<T> parent)
    {
        var pivot = parent.right;
        parent.right = RotateLL(pivot);
        return RotateRR(parent);
    }

    #endregion
}
