using System.Collections.Generic;
using UnityEngine;

public delegate void InitPoolableMethod(IPoolable poolable);

/// <summary>
/// Base Class for ObjectPool System
/// </summary>
public class GameObjectPool : RichMonoBehaviour
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

    public bool createWhenEmpty = true;

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
    //private Queue<IPoolable> queuePool;

    protected override void Awake()
    {
        base.Awake();
        if(InitOnAwake)
            InitPool();
    }

    private void OnValidate()
    {
        if (objectPrefab != null)
        {   //ensure prefab has an IPoolable Component on it 
            if (objectPrefab.GetComponent<IPoolable>() == null)
            {
                Debug.LogError("[GameObjectPool] Prefab {" + objectPrefab.name
                    + "} does not have an IPoolable Component", objectPrefab);
                objectPrefab = null;
            }
        }
    }

    public virtual IPoolable CreatePoolable()
    {
        var newGameObj = Instantiate(objectPrefab, myTransform);
        var poolable = newGameObj.GetComponent<IPoolable>();

        //validate
        Debug.Assert(poolable != null,
            "[GameObjectPool] No Poolable found on " + objectPrefab, objectPrefab);

        poolable.OnCreate();
        poolable.PoolOwner = this; //know where you came from
        
        return poolable;
    }

    /// <summary>
    /// Take an item out of the pool.
    /// </summary>
    /// <returns></returns>
    public virtual IPoolable Depool()
    {
        IPoolable depooledItem = null;

        if (pool.Count > 0)
            depooledItem = pool.Pop();
        else if(createWhenEmpty)
            depooledItem = CreatePoolable();
        
        if(depooledItem != null)
        {
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
        {
            var newP = CreatePoolable();

            if (initStragety == InitStrategy.EAGER)
                    InitPoolableMethod(newP);

            pool.Push(newP);
        }
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
