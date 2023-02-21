using RichPackage.Pooling;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RichPackage.Collections
{
    /// <summary>
    /// Function that searches a binary search tree in O(log n) time. <br/>
    /// <paramref name="other"/> is the RHS of the comparison.
    /// e.g. this.CompareTo(other);
    /// </summary>
    /// <param name="other">The RHS value of the comparison.</param>
    /// <returns>-1 if this precedes <paramref name="other"/>, 0 if this is in the same position as <paramref name="other"/>, 
    /// and 1 if follows <paramref name="other"/>.</returns>
    public delegate int Searcher<in T>(T other);

    /// <summary>
    /// Self-balancing AVL tree. Search is Log2(n). Pools nodes.
    /// </summary>
    /// <typeparam name="T">Class or Struct that impements the <see name="IComparable"></see> interface.</typeparam>
    /// <remarks>Don't modify the IComparable pivot value while
    /// the item is in a tree. Will break BST aspect.</remarks>
    public class AVLTree<T> : ICollection<T>
    {
        private class Node<TNode>
        {
            #region Properties

            public int height;

            public TNode data;

            public Node<TNode> left = null;

            public Node<TNode> right = null;

            #endregion Properties

            public Node(TNode data)
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

        private StackPool<Node<T>> nodePool;

        private Node<T> root = null;

        private IComparer<T> dataComparer = Comparer<T>.Default;

        /// <summary>
        /// Lazily-init'd stack used for enumerating the tree.
        /// </summary>
        private WeakReference<Stack<Node<T>>> enumerationStack;

        /// <summary>
        /// Determines how items in the tree are ordered. <br/>
        /// Default value is <see cref="Comparer{T}.Default"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="searcher"/> is null.</exception>
        public IComparer<T> DataComparer
        {
            get => dataComparer;
            set
            {
                if (value == null)
                    throw new ArgumentNullException(nameof(value));

                dataComparer = value;
                if (Count > 1)
                    FixViolatedTreeProperty();
            }
        }

        /// <summary>
        /// If true, nodes are drawn from a pool instead of dynamically allocated, and only allocated as needed.
        /// If false, nodes are always dynamically allocated. <br/>
        /// Default value is false.
        /// Consider setting this to true if you have lots of trees adding/removing frequently. <br/>
        /// If you are using a pool, you can also set the <see cref="NodePoolSize"/> property.
        /// </summary>
        public bool PoolInternalNodes
        {
            get => nodePool != null;
            set
            {
                if (value)
                {
                    if (nodePool == null)
                    {
                        nodePool = new StackPool<Node<T>>()
                        {
                            FactoryMethod = () => new Node<T>(default), //default constructor
                            OnEnpoolMethod = (n) => n.Reset(),// AVLNode<T>.ResetNode //de-init nodes
                        };
                    }
                }
                else
                {
                    nodePool = null;
                }
            }
        }

        public int NodePoolSize
        {
            get
            {
                int size = 0;

                if (PoolInternalNodes)
                {
                    size = nodePool.Count;
                }

                return size;
            }
            set
            {
                PoolInternalNodes = true;
                nodePool.MaxCount = value;
            }
        }

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
        public int HeightEstimate => (int)Math.Round(Math.Log(Count, 2) * 1.441f, MidpointRounding.AwayFromZero);

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

        #endregion Properties

        #region Constructors

        public AVLTree()
        {
            Count = 0;
        }

        public AVLTree(in IEnumerable<T> source) : this()
        {
            foreach (T item in source)
                Add(item);
        }

        #endregion Constructors

        #region Insert

        /// <summary>
        /// Insert an item into the data set. O(Log2(n))
        /// </summary>
        /// <param name="data">Data to add.</param>
        public void Add(T data)
        {
            if (data == null)
                throw new ArgumentNullException(nameof(data));

            Node<T> newNode = PoolInternalNodes ? nodePool.Depool() : new Node<T>(data);
            newNode.data = data;
            RecursiveInsert(ref root, newNode);
        }

        private void RecursiveInsert(
            ref Node<T> current, Node<T> newNode)
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

        private bool TryAddIfNew(ref Node<T> currentNode, in T data)
        {
            if (currentNode == null)
            {
                Node<T> newNode = PoolInternalNodes ? nodePool.Depool() : new Node<T>(data);
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

        #endregion Insert

        /// <summary>
        /// Returns true if a node in the tree is equal to the data using the IComparable interface. O(Log2(n))
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(T key) => FindNode(key) != null;

        /// <summary>
        /// Returns true if a node in the tree is equal to the data using the IComparable interface. O(Log2(n))
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(Searcher<T> searcher) => FindNode(searcher) != null;

        #region Removal

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear() => RemoveAll(); //alias

        /// <summary>
        /// Remove node where the data is equal to the key using the IComparable interface. O(Log2(n))
        /// </summary>
        public void Remove(in T target)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            --Count;//assume it was found, and backtrack if not
            Remove(ref root, target);
        }

        public void Remove(Searcher<T> searcher)
        {
            --Count;//assume it was found, and backtrack if not
            Remove(ref root, searcher);
        }

        /// <summary>
        /// Remove node where the data is equal to the key using the IComparable interface. O(Log2(n))
        /// </summary>
        ///<returns>True if the node was found and removed, otherwise false.</returns>
        public bool TryRemove(in T target)
        {
            if (target == null)
                throw new ArgumentNullException(nameof(target));

            int preCount = Count;
            Remove(target);
            return Count < preCount;
        }

        public bool TryRemove(Searcher<T> searcher)
        {
            int preCount = Count;
            Remove(searcher);
            return Count < preCount;
        }

        /// <summary>
        /// Empties the tree.
        /// </summary>
        public void RemoveAll()
        {
            if (PoolInternalNodes)
                PostOrderProcessNodes(root, (n) => nodePool.Enpool(n)); // clears nodes

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
            bool found = TryInOrderSearch(predicate, out value);
            if (found)
                Remove(value);//requires another shorter search to remove and balance
            return found;
        }

        public bool TryGetRemove(Searcher<T> searcher, out T value)
        {   //TODO - do in one walk instead of 2
            Node<T> node = FindNode(searcher); //find target node
            bool found = node != null;

            if (found)
            {
                value = node.data; //return value
                Remove(ref root, searcher); //find using default searcher
            }
            else
            {
                value = default;
            }
            return found;
        }

        /// <summary>
        /// Returns the first value that matches given predicate.
        /// <see cref="TryGetRemove(Searcher{T}, out T)"/> to determine if the value was found.
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
        /// <see cref="TryGetRemove(Searcher{T}, out T)"/> to determine if the value was found.
        /// </summary>
        /// <returns>The value that was found or 'default'.</returns>
        /// <exception>KeyNotFoundException></exception>
        public T GetRemove(Searcher<T> searcher)
        {
            if (TryGetRemove(searcher, out T value))
                return value;
            throw new KeyNotFoundException();
        }

        private void Remove(ref Node<T> current, T key)
            => Remove(ref current, (other) => DataComparer.Compare(key, other));

        private void Remove(ref Node<T> current, Searcher<T> searcher)
        {
            if (current == null)
            {
                ++Count;//undo wrong assumption (janky, but cheap).
                return; //not found
            }
            else
            {
                int compareResult = searcher(current.data);

                //left subtree
                if (compareResult < 0) //get and stash compare result for next else if
                    Remove(ref current.left, searcher);
                //right subtree
                else if (compareResult > 0)
                    Remove(ref current.right, searcher);
                else  //target is found!
                {   // delete this node and replace with a child?
                    //case 1: one or no children
                    if (current.left == null
                    || current.right == null)
                    {
                        Node<T> temp = current; //del current
                        if (current.left != null)
                            current = current.left; //replace with left
                        else if (current.right != null)
                            current = current.right; //replace with right
                        else //has no children
                            current = null; //del self

                        //free node
                        if (PoolInternalNodes)
                            nodePool.Enpool(temp);
                        else
                            temp = null;
                    }
                    else //2 children
                    {
                        //find the inorder successor
                        //(the smallest item in the right subtree)
                        Node<T> successor = current.right;
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

        private void RemoveNode(ref Node<T> current, Node<T> targetNode)
        {
            int compareResult = dataComparer.Compare(targetNode.data, current.data);

            //left subtree
            if (compareResult < 0 || (compareResult == 0 && current != targetNode)) //get and stash compare result for next else if
                RemoveNode(ref current.left, targetNode);
            //right subtree
            else if (compareResult > 0)
                RemoveNode(ref current.right, targetNode);
            else  //target is found!
            {
                // delete this node and replace with a child?
                //case 1: one or no children
                if (current.left == null
                || current.right == null)
                {
                    Node<T> temp = current; //del current
                    if (current.left != null)
                        current = current.left; //replace with left
                    else if (current.right != null)
                        current = current.right; //replace with right
                    else //has no children
                        current = null; //del self

                    //free node
                    if (PoolInternalNodes)
                        nodePool.Enpool(temp);
                    else
                        temp = null;
                }
                else //2 children
                {
                    //find the inorder successor
                    //(the smallest item in the right subtree)
                    Node<T> successor = current.right;
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

        #endregion

        #region Min/Max Value

        /// <exception cref="InvalidOperationException"> if tree is empty.</exception>
        public T GetMaxValue()
        {
            if (root == null)
                throw new InvalidOperationException("Can't get max value from empty tree.");

            Node<T> maxNode = root;
            while (maxNode.right != null)
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

            Node<T> minNode = root;
            while (minNode.left != null)
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
        /// <param name="searcher">Comparison method.</param>
        /// <param name="foundItem">Holds item that matched querry.</param>
        /// <returns>True if item was found and returned.</returns>
        public bool TryFind(Searcher<T> searcher, out T foundItem)
        {
            Node<T> foundNode = FindNode(searcher);
            bool searchSuccessful = foundNode != null;
            foundItem = searchSuccessful ? foundNode.data : default;//return value
            return searchSuccessful;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Node<T> FindNode(T key)
            => FindNode((other) => dataComparer.Compare(key, other), root); //default comparer

        /// <summary>
        /// Search against key query using binary search.
        /// </summary>
        /// <param name="searcher">Comparison method. Will select Node whose comparison == 0.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private Node<T> FindNode(Searcher<T> searcher)
            => FindNode(searcher, root); //explicit searcher

        /// <summary>
        /// Recursive binary search.
        /// </summary>
        /// <param name="target">Partially-filled record.</param>
        /// <param name="searcher">Comparison method. Will select Node whose comparison == 0.</param>
        /// <returns>Null if not found.</returns>
        private static Node<T> FindNode(Searcher<T> searcher,
            Node<T> current)
        {
            if (current == null)
                return null;

            int compareResult = searcher(current.data);
            if (compareResult > 0)
                return FindNode(searcher, current.right);
            else if (compareResult < 0)
                return FindNode(searcher, current.left);
            else
                return current; // found!
        }

        /// <summary>
        /// Processes the first node that compares equal. Do NOT modify the values compared with <see cref="DataComparer"/> or you could bork the tree.
        /// </summary>
        public void ProcessItem(Searcher<T> searcher, Action<T> processor)
        {
            Node<T> foundNode = FindNode(searcher, root);
            if (foundNode != null)
                processor(foundNode.data);
        }

        /// <summary>
        /// Processes the first node that compares equal. Do NOT modify the values compared with <see cref="DataComparer"/> or you could bork the tree.
        /// </summary>
        public bool TryProcessItem(Searcher<T> searcher, Action<T> processor)
        {
            Node<T> foundNode = FindNode(searcher, root);
            bool found = foundNode != null; //return value
            if (found)
                processor(foundNode.data);
            return found;
        }

        /// <summary>
        /// Processes the first node that compares equal. <br/>
        /// Do NOT modify the values compared with <see cref="DataComparer"/> or you could bork the tree. <br/>
        /// If you do violate the tree property, you must call <see cref="FixViolatedTreeProperty"/> or else your searching behavior is undefined.
        /// </summary>
        public void ProcessItemRef(Searcher<T> searcher, ActionRef<T> processor)
        {
            Node<T> foundNode = FindNode(searcher, root);
            if (foundNode != null)
                processor(ref foundNode.data);
        }

        /// <summary>
        /// Processes the first node that compares equal. Do NOT modify the values compared with <see cref="DataComparer"/> or you could bork the tree.
        /// </summary>
        public bool TryProcessItemRef(Searcher<T> searcher, ActionRef<T> processor)
        {
            Node<T> foundNode = FindNode(searcher, root);
            bool found = foundNode != null; //return value
            if (found)
                processor(ref foundNode.data);
            return found;
        }

        #endregion Searching

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
            Node<T> current, Action<T> process)
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
            Node<T> current, Predicate<T> predicate,
            ref T value)
        {
            if (current != null)
            {
                // check left
                if (TryInOrderSearch(current.left,
                    predicate, ref value))
                {
                    return true;
                }

                // check this
                if (predicate(current.data))
                {
                    value = current.data;
                    return true;
                }

                // check right
                if (TryInOrderSearch(current.right,
                    predicate, ref value))
                {
                    return true;
                }
            }
            return false;
        }

        public IEnumerable<T> EnumerateInOrder()
        {
            if (root == null)
                yield break;

            Stack<Node<T>> stack = GetEnumerationStack();
            Node<T> current = root;

            while (current != null || stack.Count > 0)
            {
                while (current != null)
                {
                    stack.Push(current);
                    current = current.left;
                }

                current = stack.Pop();
                yield return current.data;
                current = current.right;
            }
        }

        #endregion In-Order Processing

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
            Node<T> current, Action<Node<T>> process)
        {
            if (current != null)
            {
                PostOrderProcessNodes(current.left, process);
                PostOrderProcessNodes(current.right, process);
                process(current);
            }
        }

        private static void PostOrderProcessTree(
            Node<T> current, Action<T> process)
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
            Node<T> current, Predicate<T> predicate,
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

        public IEnumerable<T> EnumeratePostOrder()
        {
            if (root == null)
                yield break;

            Stack<Node<T>> stack = GetEnumerationStack();
            Node<T> current = root;
            Node<T> lastVisited = null;

            while (current != null || stack.Count > 0)
            {
                while (current != null)
                {
                    stack.Push(current);
                    current = current.left;
                }

                current = stack.Peek();
                if (current.right == null || current.right == lastVisited)
                {
                    yield return current.data;
                    lastVisited = current;
                    stack.Pop();
                    current = null;
                }
                else
                {
                    current = current.right;
                }
            }
        }

        #endregion Post-Order Processing

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
            Node<T> current, Action<T> process)
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
            Node<T> current, Predicate<T> predicate,
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

        public IEnumerable<T> EnumeratePreOrder()
        {
            if (root == null)
                yield break;

            Stack<Node<T>> stack = GetEnumerationStack();
            stack.Push(root);

            while (stack.Count > 0)
            {
                Node<T> current = stack.Pop();
                yield return current.data;

                if (current.right != null)
                    stack.Push(current.right);

                if (current.left != null)
                    stack.Push(current.left);
            }
        }

        #endregion Pre-Order Processing

        #region Utility

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int GetHeight(Node<T> current)
            => current == null ? 0 : current.height;

        private static void RecalculateHeight(Node<T> current)
        {
            int leftHeight = GetHeight(current.left);
            int rightHeight = GetHeight(current.right);
            current.height = ((leftHeight > rightHeight) ?
                leftHeight : rightHeight) + 1; //+1 for self
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int BalanceFactor(Node<T> current)
            => GetHeight(current.left) - GetHeight(current.right);

        private static void BalanceTree(ref Node<T> current)
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
        /// If the tree was violated and needs to be reset, like if 
        /// a node's pivot property was modified while in the tree, this method
        /// will fix the violated tree property (nodes stored in an order).
        /// </summary>
        public void FixViolatedTreeProperty()
        {
            int index = 0;
            T[] items = new T[Count];

            while (Count-- > 0)
                items[index++] = GetRemove(root.data);

            while (index > 0)
                Add(items[--index]);
        }

        /// <summary>
        /// Gets a pooled stack for enumerating the tree.
        /// </summary>
        private Stack<Node<T>> GetEnumerationStack()
        {
            // lazy init the weak reference
            enumerationStack = enumerationStack ?? new WeakReference<Stack<Node<T>>>(null);

            // use an existing stack or get a new one
            Stack<Node<T>> stack; // return value

            // if need a new one, create it and set the weak reference
            if (!enumerationStack.TryGetTarget(out stack))
            {
                stack = new Stack<Node<T>>(Count);
                enumerationStack.SetTarget(stack);
            }

            return stack;
        }

        public void TrimExcessPool()
        {
            if (PoolInternalNodes)
                nodePool.Stack.TrimExcess();
        }

        #endregion Utility

        #region Rotations

        private static Node<T> RotateRight(Node<T> parent)
        {
            Node<T> pivot = parent.right;

            //rotate
            parent.right = pivot.left;
            pivot.left = parent;

            //update heights
            RecalculateHeight(parent);
            RecalculateHeight(pivot);

            return pivot;
        }

        private static Node<T> RotateLeft(Node<T> parent)
        {
            Node<T> pivot = parent.left;

            //rotate
            parent.left = pivot.right;
            pivot.right = parent;

            //update heights
            RecalculateHeight(parent);
            RecalculateHeight(pivot);

            return pivot;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Node<T> RotateRL(Node<T> parent)
        {
            Node<T> pivot = parent.left;
            parent.left = RotateRight(pivot);
            return RotateLeft(parent);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static Node<T> RotateLR(Node<T> parent)
        {
            Node<T> pivot = parent.right;
            parent.right = RotateLeft(pivot);
            return RotateRight(parent);
        }

        #endregion

        #region ICollection

        bool ICollection<T>.IsReadOnly { get => false; }

        public void CopyTo(T[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(arrayIndex));

            if (array.Length - arrayIndex < Count)
                throw new ArgumentException("The number of elements in the source AVLTree<T> is greater than the available space from " + nameof(arrayIndex) + " to the end of the destination array.");

            // add items in order
            foreach (T item in this)
                array[arrayIndex++] = item;
        }

        public bool Remove(T item) => TryRemove(item);

        #region IEnumerable

        IEnumerator<T> IEnumerable<T>.GetEnumerator() => EnumerateInOrder().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => EnumerateInOrder().GetEnumerator();

        #endregion IEnumerable

        #endregion ICollection
    }
}
