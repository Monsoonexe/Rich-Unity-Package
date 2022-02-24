using System;
using System.Collections.Generic;
using RichPackage.Pooling;
using System.Runtime.CompilerServices;

namespace RichPackage.Collections
{
    /// <summary>
    /// e.g. this.CompareTo(other);
    /// Other is the RHS of the comparison.
    /// Useful for searching without having to create a dummy key value.
    /// </summary>
    /// <param name="other">The RHS value of the comparison.</param>
    /// <returns>-1 if this precedes other, 0 if this is in the same position as other, and 1 if follows other.</returns>
    public delegate int CompareAgainst<in T> (T other);

    /// <summary>
    /// Self-balancing AVL tree. Search is Log2(n). Pools nodes.
    /// </summary>
    /// <typeparam name="T">Class or Struct that impements the <see name="IComparable"></see> interface.</typeparam>
    /// <remarks>Don't modify the IComparable pivot value while
    /// the item is in a tree. Will break BST aspect.</remarks>
    public class AVLTree<T> where T : IComparable<T>
    {
        private class AVLNode<TNode>
        {
            #region Properties

            public int height;

            public TNode data;

            public AVLNode<TNode> left = null;

            public AVLNode<TNode> right = null;

            #endregion

            public AVLNode(TNode data)
            {
                Reset();
                this.data = data;
            }

            public void Reset()
            {
                left = null;
                right = null;
                height = 0;
                data = default;
            }
        }

        #region Properties

        private static readonly ObjectPool<AVLNode<T>> nodePool
            = new ObjectPool<AVLNode<T>>
        (
            maxCount: -1, //no limit to nodes
            preInit: 32, //prebuild a pool
            factoryMethod: () => new AVLNode<T>(default), //default constructor
            enpoolMethod: (n) => n.Reset()// AVLNode<T>.ResetNode //de-init nodes
        );

        private AVLNode<T> root = null;

        private IComparer<T> dataComparer = Comparer<T>.Default;

        /// <summary>
        /// Determines how items in the tree are ordered. <br/>
        /// Default value is <see cref="Comparer{T}.Default"/>.
         /// </summary>
        public IComparer<T> DataComparer { get => dataComparer; set => dataComparer = value;} //TODO - rebuild tree

        /// <summary>
        /// Count of items in tree.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Height &lt;= 1.441 * Log2(Count)
        /// </summary>
        public int Height { get => GetHeight(root); }

        /// <summary>
        /// Quick calculation of the height. Upper bound: Height &lt;= 1.441 * Log2(Count)
        /// </summary>
        public int HeightEstimate => (int)(Math.Round(Math.Log(Count, 2) * 1.441f, MidpointRounding.ToPositiveInfinity));

        /// <summary>
        /// Get maximum value in tree. O(1) because height is cached.
        /// </summary>
        /// <exception cref="InvalidOperationException"> if tree is empty.</exception>
        public T MaxValue => GetMaxValue();

        /// <summary>
        /// Get minimum value in tree. O(1) because height is cached.
        /// </summary>
        /// <exception cref="InvalidOperationException"> if tree is empty.</exception>
        public T MinValue => GetMinValue();

        #endregion

        #region Constructors

        public AVLTree()
        {
            Count = 0;
        }

        public AVLTree(in IEnumerable<T> source)
        {
            foreach (var item in source)
                Add(item);
        }
        
        #endregion
        
        #region Insert

        /// <summary>
        /// Insert an item into the data set. O(Log2(n))
        /// </summary>
        /// <param name="data">Data to add.</param>
        public void Add(in T data)
        {
            var newNode = nodePool.Depool();
            newNode.data = data;
            RecursiveInsert(ref root, newNode);
        }

        private void RecursiveInsert(
            ref AVLNode<T> current, AVLNode<T> newNode)
        {
            if (current == null)
            {
                ++Count;
                current = newNode;
            }
            else if (dataComparer.Compare(newNode.data, current.data) <= 0) //left-bias for identical keys
                RecursiveInsert(ref current.left, newNode);
            else
                RecursiveInsert(ref current.right, newNode);
            RecalculateHeight(current);
            BalanceTree(ref current);
        }

        /// <summary>
        /// Insert an item into the data set. O(Log2(n))
        /// </summary>
        /// <param name="data">Data to add.</param>
        /// <returns>True if data does not already exist and was just added.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryAddIfNew(in T data)
            => TryAddIfNew(ref root, data);

        private bool TryAddIfNew(ref AVLNode<T> currentNode, in T data)
        {
            if (currentNode == null)
            {
                var newNode = nodePool.Depool();
                newNode.data = data;
                RecursiveInsert(ref currentNode, newNode);
                return true;
            }
            else
            {
                int compareResult = DataComparer.Compare(data, currentNode.data);
                if (compareResult < 0)
                    return TryAddIfNew(ref currentNode.left, data);
                else if (compareResult > 0)
                    return TryAddIfNew(ref currentNode.right, data);
                else
                    return false;
            }
        }

        #endregion

        /// <summary>
        /// Returns true if a node in the tree is equal to the data using the IComparable interface. O(Log2(n))
        /// </summary>
        public bool Contains(in T key) => FindNode(key) != null;

        /// <summary>
        /// Returns true if a node in the tree is equal to the data using the IComparable interface. O(Log2(n))
        /// </summary>
        public bool Contains(CompareAgainst<T> comparer) => FindNode(comparer) != null;

        #region Removal

        /// <summary>
        /// Remove node where the data is equal to the key using the IComparable interface. O(Log2(n))
        /// </summary>
        public void Remove(in T target)
        {
            --Count;//assume it was found, and backtrack if not
            Remove(ref root, target);
        }

        public void Remove(CompareAgainst<T> comparer)
        {
            --Count;//assume it was found, and backtrack if not
            Remove(ref root, comparer);
        }

        /// <summary>
        /// Remove node where the data is equal to the key using the IComparable interface. O(Log2(n))
        /// </summary>
        ///<returns>True if the node was found and removed, otherwise false.</returns>
        public bool TryRemove(in T target)
        {
            int preCount = Count;
            Remove(target);
            return Count < preCount;
        }

        public bool TryRemove(CompareAgainst<T> comparer)
        {
            int preCount = Count;
            Remove(comparer);
            return Count < preCount;
        }

        /// <summary>
        /// Empties the tree.
        /// </summary>
        public void RemoveAll()
        {
            PostOrderProcessNodes(root, (n) => nodePool.Enpool(n)); //clears nodes
            root = null;
            Count = 0;
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
        {   //arbitrarily use this method
            var found = TryInOrderSearch(predicate, out value);
            if (found)
                Remove(value);//requires another shorter search to remove and balance
            return found;
        }

        public bool TryGetRemove(CompareAgainst<T> comparer, out T value)
        {   //TODO - do in one walk instead of 2
            var node = FindNode(comparer); //find target node
            bool found = node != null;

            if (found)
            {
                value = node.data; //return value
                Remove(node.data); //find using default comparer
            }
            else
            {
                value = default;
            }
            return found;
        }

        /// <summary>
        /// Returns the first value that matches given predicate.
        /// <see cref="TryGetRemove(CompareAgainst{T}, out T)"/> to determine if the value was found.
        /// </summary>
        /// <returns>The value that was found or 'default'.</returns>
        /// <exception>KeyNotFoundException></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T GetRemove(T key)
            => GetRemove((other) => DataComparer.Compare(key, other));

        /// <summary>
        /// Returns the first value that matches given predicate.
        /// <see cref="TryGetRemove(Predicate{T}, out T)"/> to determine if the value was found.
        /// </summary>
        /// <returns>The value that was found or 'default'.</returns>
        /// <exception>KeyNotFoundException></exception>
        public T GetRemove(Predicate<T> predicate)
        {
            if (TryGetRemove(predicate, out T value))
                return value; //throw not found exception
            throw new KeyNotFoundException();
        }

        /// <summary>
        /// Returns the first value that matches given predicate.
        /// <see cref="TryGetRemove(CompareAgainst{T}, out T)"/> to determine if the value was found.
        /// </summary>
        /// <returns>The value that was found or 'default'.</returns>
        /// <exception>KeyNotFoundException></exception>
        public T GetRemove(CompareAgainst<T> comparer)
        {
            if (TryGetRemove(comparer, out T value))
                return value;
            throw new KeyNotFoundException();
        }

        private void Remove(ref AVLNode<T> current, T key)
            => Remove(ref current, (other) => DataComparer.Compare(key, other));

        private void Remove(ref AVLNode<T> current, CompareAgainst<T> comparer)
        {
            if (current == null)
            {
                ++Count;//undo wrong assumption (janky, but cheap).
                return; //not found
            }
            else
            {
                int compareResult = comparer(current.data);

                //left subtree
                if (compareResult < 0) //get and stash compare result for next else if
                    Remove(ref current.left, comparer);
                //right subtree
                else if (compareResult > 0)
                    Remove(ref current.right, comparer);
                else  //target is found!
                {   // delete this node and replace with a child?
                    //case 1: one or no children
                    if (current.left == null
                    || current.right == null)
                    {
                        AVLNode<T> temp = current; //del current
                        if (current.left != null)
                            current = current.left; //replace with left
                        else if (current.right != null)
                            current = current.right; //replace with right
                        else //has no children
                            current = null; //del self

                        nodePool.Enpool(temp); //free node
                    }
                    else //2 children
                    {
                        //find the inorder successor
                        //(the smallest item in the right subtree)
                        AVLNode<T> successor = current.right;
                        while (successor.left != null)
                            successor = successor.left;

                        //copy the inorder successor's data to the current node
                        current.data = successor.data;

                        //delete the inorder successor
                        Remove(ref current.right, (other) => dataComparer.Compare(successor.data, other)); //find node to replace using internal comparison method.
                    }

                    //had no children
                    if (current == null)
                        return; //deleted self

                    BalanceTree(ref current);
                }
            }
        }

        #endregion

        #region Min/Max Value

        /// <exception cref="InvalidOperationException"> if tree is empty.</exception>
        public T GetMaxValue()
        {
            if (root == null)
                throw new InvalidOperationException("Can't get max value from empty tree.");

            var maxNode = root;
            while(maxNode.right != null)
                maxNode = maxNode.right;
            return maxNode.data;
        }

        /// <exception cref="InvalidOperationException"> if tree is empty.</exception>
        public T PopMax()
        {
            T value = GetMaxValue();
            Remove(value);
            return value;
        }

        /// <exception cref="InvalidOperationException"> if tree is empty.</exception>
        public T GetMinValue()
        {
            if (root == null)
                throw new InvalidOperationException("Can't get min value from empty tree.");

            var minNode = root;
            while(minNode.left != null)
                minNode = minNode.left;
            return minNode.data;
        }

        /// <exception cref="InvalidOperationException"> if tree is empty.</exception>
        public T PopMin()
        {
            T value = GetMinValue();
            Remove(value);
            return value;
        }

        #endregion

        #region Searching

        /// <summary>
        /// Search against key query using binary search using <see cref="DataComparer"/>.
        /// </summary>
        /// <param name="key">Dummy value used as comparison.</param>
        /// <param name="foundItem">Holds item that matched querry.</param>
        /// <returns>True if item was found and returned.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool TryFind(T key, out T foundItem)
            => TryFind((other) => dataComparer.Compare(key, other), out foundItem);

        /// <summary>
        /// Search against key query using binary search.
        /// </summary>
        /// <param name="comparer">Comparison method.</param>
        /// <param name="foundItem">Holds item that matched querry.</param>
        /// <returns>True if item was found and returned.</returns>
        public bool TryFind(CompareAgainst<T> comparer, out T foundItem)
        {
            var foundNode = FindNode(comparer);
            var searchSuccessful = foundNode != null;
            foundItem = searchSuccessful ? foundNode.data : default;//return value
            return searchSuccessful;
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private AVLNode<T> FindNode(T key)
            => FindNode((other) => dataComparer.Compare(key, other), root); //default comparer

        /// <summary>
        /// Search against key query using binary search.
        /// </summary>
        /// <param name="comparer">Comparison method. Will select Node whose comparison == 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private AVLNode<T> FindNode(CompareAgainst<T> comparer)
            => FindNode(comparer, root); //explicit comparer

        /// <summary>
        /// Recursive binary search.
        /// </summary>
        /// <param name="target">Partially-filled record.</param>
        /// <param name="comparer">Comparison method. Will select Node whose comparison == 0.</param>
        /// <returns>Null if not found.</returns>
        private static AVLNode<T> FindNode(CompareAgainst<T> comparer,
            AVLNode<T> current)
        {
            if (current == null) return null;

            int compareResult = comparer(current.data);
            if (compareResult > 0)
                return FindNode(comparer, current.right);
            else if (compareResult < 0)
                return FindNode(comparer, current.left);
            else
                return current; // found!
        }

        /// <summary>
        /// Processes the first node that compares equal. Do NOT modify the values compared with <see cref="DataComparer"/> or you could bork the tree.
        /// </summary>
        public void ProcessItem(CompareAgainst<T> searcher, Action<T> processor)
        {
            AVLNode<T> foundNode = FindNode(searcher, root);
            if (foundNode != null)
                processor(foundNode.data);
        }

        /// <summary>
        /// Processes the first node that compares equal. Do NOT modify the values compared with <see cref="DataComparer"/> or you could bork the tree.
        /// </summary>
        public bool TryProcessItem(CompareAgainst<T> searcher, Action<T> processor)
        {
            AVLNode<T> foundNode = FindNode(searcher, root);
            bool found = foundNode != null; //return value
            if(found)
                processor(foundNode.data);
            return found;
        }

        #endregion

        #region In-Order Processing

        /// <summary>
        /// LPR
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void InOrderProcessTree(Action<T> process)
            => InOrderProcessTree(root, process);

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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
                if (TryInOrderSearch(current.left,
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PostOrderProcessTree(Action<T> process)
            => PostOrderProcessTree(root, process);
            
        /// <summary>
        /// LRP. For internal use only.
        /// </summary>
        private static void PostOrderProcessNodes(
            AVLNode<T> current, Action<AVLNode<T>> process)
        {
            if (current != null)
            {
                PostOrderProcessNodes(current.left, process);
                PostOrderProcessNodes(current.right, process);
                process(current);
            }
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void PreOrderProcessTree(Action<T> process)
            => PreOrderProcessTree(root, process);

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
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetHeight(AVLNode<T> current) 
            => current == null ? 0 : current.height;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static void RecalculateHeight(AVLNode<T> current)
        {
            int leftHeight = GetHeight(current.left);
            int rightHeight = GetHeight(current.right);
            current.height = ((leftHeight > rightHeight) ? 
                leftHeight : rightHeight) + 1; //+1 for self
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int BalanceFactor(AVLNode<T> current)
            => GetHeight(current.left) - GetHeight(current.right);

        private static void BalanceTree(ref AVLNode<T> current)
        {
            int balanceFactor = BalanceFactor(current);

            if (balanceFactor > 1)
                if (BalanceFactor(current.left) > 0)
                    current = RotateLeft(current);
                else
                    current = RotateRL(current);
            else if (balanceFactor < -1)
                if (BalanceFactor(current.right) > 0)
                    current = RotateLR(current);
                else
                    current = RotateRight(current);
        }

        /// <summary>
        /// Trim excess nodes from the node pool. 
        /// Useful if you know no more nodes will be added.
        /// </summary>
        public void TrimExcessFromNodePool() => nodePool.TrimExcess();

        #endregion

        #region Rotations

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static AVLNode<T> RotateRight(AVLNode<T> parent)
        {
            var pivot = parent.right;

            //rotate
            parent.right = pivot.left;
            pivot.left = parent;
            
            //update heights
            RecalculateHeight(parent);
            RecalculateHeight(pivot);
            
            return pivot;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static AVLNode<T> RotateLeft(AVLNode<T> parent)
        {
            var pivot = parent.left;

            //rotate
            parent.left = pivot.right;
            pivot.right = parent;
            
            //update heights
            RecalculateHeight(parent);
            RecalculateHeight(pivot);

            return pivot;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static AVLNode<T> RotateRL(AVLNode<T> parent)
        {
            var pivot = parent.left;
            parent.left = RotateRight(pivot);
            return RotateLeft(parent);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static AVLNode<T> RotateLR(AVLNode<T> parent)
        {
            var pivot = parent.right;
            parent.right = RotateLeft(pivot);
            return RotateRight(parent);
        }

        #endregion
    }
}
