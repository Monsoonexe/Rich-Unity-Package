using System.Collections.Generic;
using UnityEngine;

public delegate void InitPoolableMethod(IPoolable poolable);

/// <summary>
/// Base Class for ObjectPool System
/// </summary>
public abstract class GameObjectPool : RichMonoBehaviour
{
    public enum InitStrategy
    {
        EAGER = 0, //init right away
        LAZY = 1 //init as you go
    }

    [Header("---Prefab Pool Resources---")]
    public GameObject objectPrefab;

    [Header("---Pool Base Settings---")]
    public bool InitOnAwake = true;

    [SerializeField]
    protected InitStrategy initStragety = InitStrategy.EAGER;

    [SerializeField]
    private int startingAmount = 6;

    public int Count { get => pool.Count; }

    /// <summary>
    /// [Optional] Something special to be performed on new entries.
    /// </summary>
    public InitPoolableMethod InitPoolableMethod = (p) => p = null;//no-op so never null

    //runtime data
    private Stack<IPoolable> pool;

    protected override void Awake()
    {
        base.Awake();
        if(InitOnAwake)
            InitPool();
    }

    public virtual IPoolable CreatePoolable()
    {
        var newGameObj = Instantiate(objectPrefab);
        var poolable = newGameObj.GetComponent<IPoolable>();

        //validate
        Debug.Assert(poolable != null,
            "[GameObjectPool] No Poolable found on " + objectPrefab, objectPrefab);

        poolable.OnCreate();
        poolable.PoolOwner = this; //know where you came from

        if (initStragety == InitStrategy.EAGER)
            InitPoolableMethod(poolable);

        return poolable;
    }

    /// <summary>
    /// Take an item out of the pool.
    /// </summary>
    /// <returns></returns>
    public virtual IPoolable Depool()
    {
        IPoolable depooledItem = null;

        Debug.Assert(pool.Count != 0, "[GameObjectPool] Pool is empty.", this);

        if(pool.Count != 0)
        {
            depooledItem = pool.Pop();
            if (initStragety == InitStrategy.LAZY)
                InitPoolableMethod(depooledItem);

            depooledItem.OnDepool();
        }
        
        return depooledItem;
    }

    /// <summary>
    /// Add an item back into the pool.
    /// </summary>
    /// <param name="poolable"></param>
    /// <returns>Whether item was sucessfully enpooled</returns>
    public virtual bool Enpool(IPoolable poolable)
    {
        pool.Push(poolable);
        poolable.PoolOwner = this;//claim
        poolable.OnEnpool();
        return true;
    }

    /// <summary>
    /// Create entire pool
    /// </summary>
    public virtual void InitPool()
    {
        pool = new Stack<IPoolable>(startingAmount);

        //preload pool
        for(var i = startingAmount; i > 0; --i)
            pool.Push(CreatePoolable());
    }

    /// <summary>
    /// Resize the pool.
    /// </summary>
    /// <param name="newCapacity"></param>
    public virtual void Resize(int newCapacity)
    {
        //if (!resizeable) return;
        //Debug.Assert(resizeable, "[GameObjectPool] Attempting to resize a marked non-resizable pool.");

        var smallerCapacity = Mathf.Min(newCapacity, pool.Count);
        while (pool.Count > smallerCapacity) //shrink
            pool.Pop();//trim excess

        pool.TrimExcess();
    }
}
