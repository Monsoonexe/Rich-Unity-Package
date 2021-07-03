﻿using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

public delegate void InitPooledGameObjectMethod(GameObject poolable);
//TODO: Reclaim when empty strategy. Like bullet holes in FPS games.

/// <summary>
/// Pools a GameObject Prefab. Depool() replaces Instantiate(), and Enpool() replaces Destroy().
/// </summary>
public class GameObjectPool : RichMonoBehaviour
{
    public enum InitStrategy
    {
        EAGER = 0, //init right away
        LAZY = 1 //init as you go
    }

    [Header("---Resources---")]
    [ShowAssetPreview]
    public GameObject objectPrefab;

    [Header("---Settings---")]
    public bool initOnAwake = true;

    public bool createWhenEmpty = false;

    [SerializeField]
    protected InitStrategy initStragety = InitStrategy.EAGER;

    [SerializeField]
    private int startingAmount = 6;

    [SerializeField]
    [Tooltip("less than 0 means 'no limit'.")]
    private int maxAmount = 10;
    public int MaxAmount { get => maxAmount; }

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
        if(initOnAwake)
            InitPool();
    }

    private GameObject CreatePoolable()
    {
        if (maxAmount >= 0 && PopulationCount >= maxAmount)
        {
            Debug.Log("[" + name + "] Pool is exhausted. "
                + "Count: " + maxAmount 
                + ". Consider increasing 'maxAmount' or setting 'createWhenEmpty'."
                , this);

            return null; //at max capacity
        }

        var newGameObj = Instantiate(objectPrefab, poolParent);

        manifest.Add(newGameObj);//track
        
        return newGameObj;
    }

    public void AddItems(int amount = 1)
    {
        for(var i = amount - 1; i >= 0; --i)
            {
                var obj = CreatePoolable();
                if(obj != null)
                    Enpool(obj);
            }
    }

    /// <summary>
    /// Take an item out of the pool.
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

        depooledItem.SetActive(true);//behaves like Instantiate();

        return depooledItem;
    }

    /// <summary>
    /// Take an item out of the pool and place at world space with rotation.
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

            //maybe this is faster?
            //trans.parent = handle;
            //trans.position = Vector3.zero;
            //trans.rotation = Quaternion.identity;
        }
        return obj;
    }

    /// <summary>
    /// Take an item out of the pool and place at world space with rotation.
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
    /// Useful for delegates/events.
    /// lol. doodee pool.
    /// </summary>
    public void DoDepool() => Depool();

    /// <summary>
    /// Useful for delegates/events.
    /// lol. doodee pool.
    /// </summary>
    public void DoDepool(Transform handle) => Depool(handle);

    /// <summary>
    /// Useful for delegates/events.
    /// lol. doodee pool.
    /// </summary>
    public void DoDepool(Vector3 point) => Depool(point);
    
    public void Enpool(MonoBehaviour poolable)
        => Enpool(poolable.gameObject);

    /// <summary>
    /// Add an item back into the pool. This replaces Destroy().
    /// </summary>
    /// <param name="poolable"></param>
    public void Enpool(GameObject poolable)
    {
        Debug.Assert(poolable != null,
            "[GameObjectPool] Trying to Enpool null!", this);

        Debug.AssertFormat(manifest.Contains(poolable),
            "[GameObjectPool] This item is not included on this Pool's manifest. " +
            "This item probably belongs to another Pool. " + 
            "pendingPoolable: {0}. Pool: {1}. manifest[0] {2}.",
            poolable, gameObject, manifest[0]);

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

            pool.Push(newP);
            newP.SetActive(false);
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
