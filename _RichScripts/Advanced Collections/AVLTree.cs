using System;

class AVLTree<T> where T : IComparable
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

    AVLNode<T> root;

    #endregion

    #region Constructors

    public AVLTree()
    {
        //nada
    }

    #endregion

    /// <summary>
    /// 
    /// </summary>
    /// <param name="data"></param>
    public void Add(T data)
    {
        AVLNode<T> newItem = new AVLNode<T>(data);
        if (root == null)
        {
            root = newItem;
        }
        else
        {
            root = RecursiveInsert(root, newItem);
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="current"></param>
    /// <returns></returns>
    private static AVLNode<T> BalanceTree(AVLNode<T> current)
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
        return current;
    }

    /// <summary>
    /// Remove node with target data.
    /// </summary>
    /// <param name="target"></param>
    public void Delete(T target)
    {
        root = Delete(root, target);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="current"></param>
    /// <param name="target"></param>
    /// <returns></returns>
    private static AVLNode<T> Delete(AVLNode<T> current, T target)
    {
        AVLNode<T> parent;

        if (current == null) { return null; }

        else
        {
            //left subtree
            if (target.CompareTo(current.data) < 0)
            {
                current.left = Delete(current.left, target);
                if (BalanceFactor(current) == -2)//here
                {
                    if (BalanceFactor(current.right) <= 0)
                    {
                        current = RotateRR(current);
                    }
                    else
                    {
                        current = RotateRL(current);
                    }
                }
            }

            //right subtree
            else if (target.CompareTo(current.data) > 0)
            {
                current.right = Delete(current.right, target);
                if (BalanceFactor(current) == 2)
                {
                    if (BalanceFactor(current.left) >= 0)
                    {
                        current = RotateLL(current);
                    }
                    else
                    {
                        current = RotateLR(current);
                    }
                }
            }
            //if target is found
            else
            {
                if (current.right != null)
                {
                    //delete its inorder successor
                    parent = current.right;
                    while (parent.left != null)
                    {
                        parent = parent.left;
                    }
                    current.data = parent.data;
                    current.right = Delete(current.right, parent.data);
                    if (BalanceFactor(current) == 2)//rebalancing
                    {
                        if (BalanceFactor(current.left) >= 0)
                        {
                            current = RotateLL(current);
                        }
                        else { current = RotateLR(current); }
                    }
                }
                else
                {   //if current.left != null
                    return current.left;
                }
            }
        }
        return current;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="key"></param>
    public bool Find(T key)
    {
        return Find(key, root).data.CompareTo(key) == 0;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="target"></param>
    /// <param name="current"></param>
    /// <returns></returns>
    private static AVLNode<T> Find(T target, AVLNode<T> current)
    {

        if (target.CompareTo(current.data) < 0)
        {
            if (target.CompareTo(current.data) == 0)
            {
                return current;
            }
            else
                return Find(target, current.left);
        }
        else
        {
            if (target.CompareTo(current.data) == 0)
            {
                return current;
            }
            else
                return Find(target, current.right);
        }

    }

    /// <summary>
    /// LPR
    /// </summary>
    public void InOrderProcessTree(Action<T> process)
    {
        if (root == null)
        {
            return;
        }

        InOrderProcessTree(root, process);
    }

    /// <summary>
    /// LPR
    /// </summary>
    /// <param name="current"></param>
    /// <param name="process"></param>
    private static void InOrderProcessTree(AVLNode<T> current, Action<T> process)
    {
        if (current != null)
        {
            InOrderProcessTree(current.left, process);
            process(current.data);
            InOrderProcessTree(current.right, process);
        }
    }

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

    private static void PostOrderProcessTree(AVLNode<T> current, Action<T> process)
    {
        if (current != null)
        {
            PostOrderProcessTree(current.left, process);
            PostOrderProcessTree(current.right, process);
            process(current.data);
        }
    }

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
    /// <param name="current"></param>
    /// <param name="process"></param>
    private static void PreOrderProcessTree(AVLNode<T> current, Action<T> process)
    {
        if (current != null)
        {
            process(current.data);
            PreOrderProcessTree(current.left, process);
            PreOrderProcessTree(current.right, process);
        }
    }

    /// <summary>
    /// Return element that is bigger.
    /// </summary>
    /// <param name="l"></param>
    /// <param name="r"></param>
    /// <returns></returns>
    private static U Max<U>(U l, U r) where U : IComparable
    {
        return l.CompareTo(r) > 0 ? l : r;
    }

    private static int GetHeight(AVLNode<T> current)
    {
        var height = 0;

        if (current != null)
        {
            var l = GetHeight(current.left);
            var r = GetHeight(current.right);
            var m = Max(l, r);
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
    /// <param name="newNode"></param>
    /// <returns></returns>
    private static AVLNode<T> RecursiveInsert(AVLNode<T> current, AVLNode<T> newNode)
    {
        if (current == null)
        {
            current = newNode;
            return current;
        }

        else if (newNode.data.CompareTo(current.data) < 0)
        {
            current.left = RecursiveInsert(current.left, newNode);
            current = BalanceTree(current);
        }

        else if (newNode.data.CompareTo(current.data) > 0)
        {
            current.right = RecursiveInsert(current.right, newNode);
            current = BalanceTree(current);
        }

        return current;
    }

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
    
}
