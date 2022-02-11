﻿using System;
using System.Collections.Generic;
using RichPackage.Pooling;

/*  TODO - allow for applying custom comparison function rather than relying on CompareTo.
 *  this would allow for things like arrays to be pooled natively without a wrapper class.
 *  FixMe - TryFindRemove() costs 2 traversals, one for find, one to delete
 *  fixme - AddIfNew() costs 2 traversals
 */

namespace RichPackage.Collections
{
    /// <summary>
    /// Self-balancing AVL tree. Search is Log2(n). Pools nodes.
    /// </summary>
    /// <typeparam name="T">Class or Struct that impements the <see name="IComparable"/see> interface.</typeparam>
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

        /// <summary>
        /// Insert an item into the data set. O(Log2(n))
        /// </summary>
        /// <param name="data">Data to add.</param>
        /// <returns>True if data does not already exist and was just added.</returns>
        public bool TryAddIfNew(in T data)
            => TryAddIfNew(ref root, data);

        /// <summary>
        /// Returns true if a node in the tree is equal to the data using the IComparable interface. O(Log2(n))
        /// </summary>
        public bool Contains(in T key)
        {
            var foundNode = FindNode(key, root);
            return foundNode != null
                && foundNode.data.CompareTo(key) == 0;
        }

        /// <summary>
        /// Remove node where the data is equal to the key using the IComparable interface. O(Log2(n))
        public void Remove(in T target)
        {
            --Count;//assume it was found, and backtrack if not
            Remove(ref root, target);
        }

        /// <summary>
        /// Empties the tree.
        /// </summary>
        public void RemoveAll()
        {
            while (root != null)
                Remove(ref root, root.data);
            Count = 0;
        }

        private void Remove(ref AVLNode<T> current, in T target)
        {
            if (current == null)
            {
                ++Count;//undo wrong assumption (janky, but cheap).
                return; //not found
            }
            else
            {
                int compareResult = target.CompareTo(current.data);

                //left subtree
                if (compareResult < 0) //get and stash compare result for next else if
                    Remove(ref current.left, target);
                //right subtree
                else if (compareResult > 0)
                    Remove(ref current.right, target);
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
                        Remove(ref current.right, successor.data);
                    }

                    //had no children
                    if (current == null)
                        return; //deleted self

                    BalanceTree(ref current);
                }
            }
        }

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
            {
                --Count; // assume it was found, and backtrack if not
                Remove(ref root, value);//requires another shorter search
            }
            return found;
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        private static void BalanceTree(ref AVLNode<T> current)
        {
            int balanceFactor = BalanceFactor(current);

            if (balanceFactor > 1)
                if (BalanceFactor(current.left) > 0)
                    current = RotateLL(current);
                else
                    current = RotateLR(current);
            else if (balanceFactor < -1)
                if (BalanceFactor(current.right) > 0)
                    current = RotateRL(current);
                else
                    current = RotateRR(current);
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
}
