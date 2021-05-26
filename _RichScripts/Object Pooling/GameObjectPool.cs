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

    [SerializeField]
    [Tooltip("less than 0 means 'no limit'.")]
    private int maxAmount = 10;

    /// <summary>
    /// [Optional] Something special to be performed on new entries.
    /// </summary>
    public InitPoolableMethod InitPoolableMethod = (p) => p = null;//no-op so never null

    //runtime data
    private Stack<IPoolable> pool;
    private List<IPoolable> manifest;

    /// <summary>
    /// Total items this Pool tracks.
    /// </summary>
    public int PopulationCount { get => manifest.Count; }

    /// <summary>
    /// How many items are ready to be deployed.
    /// </summary>
    public int ReadyCount { get => pool.Count; }

    /// <summary>
    /// Number of items currently depooled.
    /// </summary>
    public int InUseCount { get => manifest.Count - pool.Count; }
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

    protected virtual IPoolable CreatePoolable()
    {
        if (maxAmount >= 0 && PopulationCount >= maxAmount)
            return null; //at max capacity

        var newGameObj = Instantiate(objectPrefab, myTransform);
        var poolable = newGameObj.GetComponent<IPoolable>();

        //validate
        Debug.Assert(poolable != null,
            "[GameObjectPool] No Poolable found on " + objectPrefab, objectPrefab);

        poolable.OnCreate();
        poolable.PoolOwner = this; //know where you came from
        manifest.Add(poolable);//track
        
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

    public T Depool<T>() where T : class => Depool() as T;

    /// <summary>
    /// Add an item back into the pool.
    /// </summary>
    /// <param name="poolable"></param>
    /// <returns>Whether item was sucessfully enpooled</returns>
    public virtual void Enpool(IPoolable poolable)
    {
        Debug.Assert(poolable.PoolOwner == this,
            "[GameObjectPool] Pool Error! Enpooled to non-owning Pool!", this);

        if (!pool.Contains(poolable))//guard against multiple entries
        {
            pool.Push(poolable);
            poolable.OnEnpool();
        }
    }

    /// <summary>
    /// Create entire pool
    /// </summary>
    public virtual void InitPool()
    {
        maxAmount = maxAmount < startingAmount ? startingAmount : maxAmount;
        pool = new Stack<IPoolable>(maxAmount);

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

        maxAmount = Mathf.Min(newCapacity, ReadyCount);
        while (pool.Count > maxAmount) //shrink
        {
            var poolable = pool.Pop();//trim excess
            manifest.Remove(poolable);
        }

        manifest.TrimExcess();
        pool.TrimExcess();
    }

    public void ReturnAllToPool()
    {
        var count = manifest.Count;
        for (var i = 0; i < count; ++i)
            Enpool(manifest[i]);
    }

}
