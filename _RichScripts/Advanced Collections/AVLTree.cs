using System;
using System.Collections.Generic;
using RichPackage.Pooling;
using System.Runtime.CompilerServices;

namespace RichPackage.Collections
{
    /// <summary>
    /// e.g. this.CompareTo(other); Other is the RHS value of the comparison. 
    /// Useful for searching without having to create a dummy key value.
    /// </summary>
    /// <param name="other">The RHS value of the comparison.</param>
    /// <returns>-1 if this is less than other, 0 if this is equal to other, and 1 if this is greater than other</returns>
    public delegate int Comparer<in T> (T other);

    /// <summary>
    /// Self-balancing AVL tree. Search is Log2(n). Pools nodes.
    /// </summary>
    /// <typeparam name="T">Class or Struct that impements the <see name="IComparable"></see> interface.</typeparam>
    /// <remarks>Don't modify the IComparable pivot value while
    /// the item is in a tree. Will break BST aspect.</remarks>
    public class AVLTree<T> where T : IComparable
    {
        private class AVLNode<TNode> where TNode : IComparable
        {
            #region Properties

            public TNode data;

            public AVLNode<TNode> left = null;

            public AVLNode<TNode> right = null;

            #endregion

            public AVLNode(TNode data)
            {
                this.data = data;
            }

            public void Reset()
            {
                left = null;
                right = null;
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

        //private Class

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
        public int HeightEstimate => (int)(Math.Round(Math.Log(2, Count) * 1.441f, MidpointRounding.AwayFromZero));

        /// <summary>
        /// Get maximum value in tree. O(height). It is your responsibility to not call this on an empty tree.
        /// </summary>
        public T MaxValue => GetMaxValue();

        /// <summary>
        /// Get minimum value in tree. O(height). It is your responsibility to not call this on an empty tree.
        /// </summary>
        public T MinValue => GetMinValue();

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
            int compareResult = 0;
            if (current == null)
            {
                ++Count;
                current = newNode;
            }
            else if ((compareResult = newNode.data.CompareTo(current.data)) <= 0)//stash result and check if less than 0
            {
                RecursiveInsert(ref current.left, newNode);
                BalanceTree(ref current);
            }
            else
            {
                RecursiveInsert(ref current.right, newNode);
                BalanceTree(ref current);
            }
        }

        /// <summary>
        /// Insert an item into the data set. O(Log2(n))
        /// </summary>
        /// <param name="data">Data to add.</param>
        /// <returns>True if data does not already exist and was just added.</returns>
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
                int compareResult = data.CompareTo(currentNode.data);
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
        public bool Contains(Comparer<T> comparer) => FindNode(comparer) != null;

        #region Removal

        /// <summary>
        /// Remove node where the data is equal to the key using the IComparable interface. O(Log2(n))
        /// </summary>
        public void Remove(in T target)
        {
            --Count;//assume it was found, and backtrack if not
            Remove(ref root, target);
        }

        public void Remove(Comparer<T> comparer)
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

        public bool TryRemove(Comparer<T> comparer)
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

        public bool TryGetRemove(Comparer<T> comparer, out T value)
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
        /// <see cref="TryGetRemove(Comparer{T}, out T)"/> to determine if the value was found.
        /// </summary>
        /// <returns>The value that was found or 'default'.</returns>
        /// <exception>KeyNotFoundException></exception>
        public T GetRemove(Comparer<T> comparer)
        {
            if (TryGetRemove(comparer, out T value))
                return value;
            throw new KeyNotFoundException();
        }

        private void Remove(ref AVLNode<T> current, T target)
            => Remove(ref current, (other) => target.CompareTo(other));

        private void Remove(ref AVLNode<T> current, Comparer<T> comparer)
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
                        Remove(ref current.right, (other) => successor.data.CompareTo(other)); //find node to replace using internal comparison method.
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

        public T GetMaxValue()
        {
            if (root == null) //throw new EmptyTreeException();
                return default;

            var maxNode = root;
            while(maxNode.right != null)
                maxNode = maxNode.right;
            return maxNode.data;
        }

        public T GetMinValue()
        {
            if (root == null) //throw new EmptyTreeException();
                return default;

            var minNode = root;
            while(minNode.left != null)
                minNode = minNode.left;
            return minNode.data;
        }

        #endregion

        #region Searching

        /// <summary>
        /// Search against key query using binary search using the default comparer: <see name="IComparable.CompareTo({T})"></see>.
        /// </summary>
        /// <param name="key">Dummy value used as comparison.</param>
        /// <param name="foundItem">Holds item that matched querry.</param>
        /// <returns>True if item was found and returned.</returns>
        public bool TryFind(T key, out T foundItem)
            => TryFind((other) => key.CompareTo(other), out foundItem);

        /// <summary>
        /// Search against key query using binary search.
        /// </summary>
        /// <param name="comparer">Comparison method.</param>
        /// <param name="foundItem">Holds item that matched querry.</param>
        /// <returns>True if item was found and returned.</returns>
        public bool TryFind(Comparer<T> comparer, out T foundItem)
        {
            var foundNode = FindNode(comparer);
            var searchSuccessful = foundNode != null;
            foundItem = searchSuccessful ? foundNode.data : default;//return value
            return searchSuccessful;
        }
        
        private AVLNode<T> FindNode(T target)
            => FindNode((other) => target.CompareTo(other), root); //default comparer

        private AVLNode<T> FindNode(Comparer<T> comparer)
            => FindNode(comparer, root); //explicit comparer

        /// <summary>
        /// Recursive binary search.
        /// </summary>
        /// <param name="target">Partially-filled record.</param>
        /// <returns>Null if not found.</returns>
        private static AVLNode<T> FindNode(Comparer<T> comparer,
            AVLNode<T> current)
        {
            if (current == null) return null;

            var compareResult = comparer(current.data);
            if (compareResult > 0)
                return FindNode(comparer, current.right);
            else if (compareResult < 0)
                return FindNode(comparer, current.left);
            else
                return current; // found!
        }

        /// <summary>
        /// Processes the first node that compares equal. Do NOT modify the comparison values of <see name="CompareTo()"/>, or you could bork the tree.
        /// </summary>
        public void ProcessItem(Comparer<T> searcher, Action<T> processor)
        {
            AVLNode<T> foundNode = FindNode(searcher, root);
            if (foundNode != null)
                processor(foundNode.data);
        }

        /// <summary>
        /// Processes the first node that compares equal. Do NOT modify the comparison values of <see name="CompareTo()"/>, or you could bork the tree.
        /// </summary>
        public bool TryProcessItem(Comparer<T> searcher, Action<T> processor)
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
            int height = 0; //return value

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

        #endregion

        #region Rotations

        private static AVLNode<T> RotateRight(AVLNode<T> parent)
        {
            var pivot = parent.right;
            parent.right = pivot.left;
            pivot.left = parent;
            return pivot;
        }

        private static AVLNode<T> RotateLeft(AVLNode<T> parent)
        {
            var pivot = parent.left;
            parent.left = pivot.right;
            pivot.right = parent;
            return pivot;
        }

        private static AVLNode<T> RotateRL(AVLNode<T> parent)
        {
            var pivot = parent.left;
            parent.left = RotateRight(pivot);
            return RotateLeft(parent);
        }

        private static AVLNode<T> RotateLR(AVLNode<T> parent)
        {
            var pivot = parent.right;
            parent.right = RotateLeft(pivot);
            return RotateRight(parent);
        }

        #endregion
    }
}
