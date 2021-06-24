using System.Collections.Generic;
using UnityEngine;

public delegate void InitPooledGameObjectMethod(GameObject poolable);

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

    [Tooltip("[Optional]")]
    public Transform poolParent = null;

    /// <summary>
    /// [Optional] Something special to be performed on new entries.
    /// </summary>
    public InitPooledGameObjectMethod InitPoolableMethod = (p) => p = null;//no-op so never null

    //runtime data
    private Stack<GameObject> pool; //stack has better locality than queue
    private List<GameObject> manifest;

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

    protected override void Awake()
    {
        base.Awake();
        manifest = new List<GameObject>(maxAmount);
        if(InitOnAwake)
            InitPool();
    }

    protected GameObject CreatePoolable()
    {
        if (maxAmount >= 0 && PopulationCount >= maxAmount)
            return null; //at max capacity

        var newGameObj = Instantiate(objectPrefab, poolParent);

        manifest.Add(newGameObj);//track
        
        return newGameObj;
    }

    /// <summary>
    /// Take an item out of the pool.
    /// Note: does NOT call SetActive().
    /// </summary>
    /// <returns>Newly de-pool object.</returns>
    public GameObject Depool()
    {
        GameObject depooledItem = null;

        if (pool.Count > 0)
            depooledItem = pool.Pop();
        else if(createWhenEmpty)
            depooledItem = CreatePoolable();
        
        if(depooledItem != null && initStragety == InitStrategy.LAZY)
        {
            InitPoolableMethod(depooledItem);
        }

        return depooledItem;
    }

    /// <summary>
    /// Take an item out of the pool and place at world space with rotation.
    /// Note: does NOT call SetActive(). Does NOT change parentage.
    /// </summary>
    /// <returns>Newly de-pool object.</returns>
    public GameObject Depool(Transform handle)
    {
        var obj = Depool();
        if(obj != null)
        {
            var trans = obj.GetComponent<Transform>();
            trans.position = handle.position;
            trans.rotation = handle.rotation;
            trans.parent = handle;
        }
        return obj;
    }

    /// <summary>
    /// Take an item out of the pool and place at world space with rotation.
    /// Note: does NOT call SetActive().
    /// </summary>
    /// <returns>Newly de-pool object.</returns>
    public GameObject Depool(Transform handle, bool setParent)
    {
        GameObject obj = null;
        if(setParent)
            obj = Depool(handle);
        else
            obj = Depool(handle.position, handle.rotation);
        return obj;
    }

    /// <summary>
    /// Take an item out of the pool and place at world space.
    /// Note: does NOT call SetActive().
    /// </summary>
    /// <returns>Newly de-pool object.</returns>
    public GameObject Depool(Vector3 position)
    {
        var obj = Depool();
        if(obj != null)
        {
            var trans = obj.GetComponent<Transform>();
            trans.position = position;
        }
        return obj;
    }

    /// <summary>
    /// Take an item out of the pool and set at world space with orientation.
    /// Note: does NOT call SetActive().
    /// </summary>
    /// <returns>Newly de-pool object.</returns>
    public GameObject Depool(Vector3 position, Quaternion rotation)
    {
        var obj = Depool();
        if(obj != null)
        {
            var trans = obj.GetComponent<Transform>();
            trans.position = position;
            trans.rotation = rotation;
        }
        return obj;
    }

    /// <summary>
    /// Take an item out of the pool and GetComponent{T} on it.
    /// Note: does NOT call SetActive().
    /// </summary>
    /// <returns>Newly de-pool object or null if the Component wasn't found.</returns>
    public T Depool<T>() where T : Component
    {
        T component = null;
        var obj = Depool();
        if(obj)
            component = obj.GetComponent<T>();
        return component;
    }

    /// <summary>
    /// Add an item back into the pool.
    /// </summary>
    /// <param name="poolable"></param>
    public void Enpool(GameObject poolable)
    {
        Debug.Assert(poolable != null,
            "[GameObjectPool] Trying to Enpool null!", this);

        if (!pool.Contains(poolable))//guard against multiple entries
        {
            pool.Push(poolable);
            poolable.SetActive(false);
        }
    }

    /// <summary>
    /// Create entire pool
    /// </summary>
    public void InitPool()
    {
        maxAmount = maxAmount < startingAmount ? startingAmount : maxAmount;
        pool = new Stack<GameObject>(maxAmount);

        //preload pool
        for (var i = startingAmount; i > 0; --i)
        {
            var newP = CreatePoolable();

            if (initStragety == InitStrategy.EAGER)
                InitPoolableMethod(newP);

            pool.Push(poolable);
            poolable.SetActive(false);
        }
    }

    /// <summary>
    /// Resize the pool.
    /// </summary>
    /// <param name="newCapacity"></param>
    public void Resize(int newCapacity)
    {
        maxAmount = RichMath.Min(newCapacity, ReadyCount);
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
