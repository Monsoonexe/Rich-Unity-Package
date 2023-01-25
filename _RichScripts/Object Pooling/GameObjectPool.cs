using RichPackage.Assertions;
using Sirenix.OdinInspector;
using Sirenix.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine;

//TODO: Reclaim when empty strategy. Like bullet holes in FPS games.

namespace RichPackage.Pooling
{
    public delegate void GameObjectMethod(GameObject poolable);

    /// <summary>
    /// Pools a GameObject Prefab. Depool() replaces Instantiate(), and Enpool() replaces Destroy().
    /// </summary>
    public sealed class GameObjectPool : RichMonoBehaviour
    {
        [Title("Resources")]
        [PreviewField, Required, AssetsOnly]
        public GameObject objectPrefab;

        [Header("---Settings---")]
        [Tooltip("If true, this pool will initialize itself on Start(). " +
            "If false, you must call " + nameof(Init) + "() manually.")]
        public bool initOnStart = true;
        
        [Tooltip("Should this pool enpool any existing children when init'g?")]
        public bool enpoolChildrenOnInit = false;

        [Min(0)]
        public int startingAmount = 6;

        [Tooltip("Less than 0 means 'no limit'.")]
        [Min(-1)]
        public int maxAmount = 10;

        [Tooltip("[Optional] The parent of all pooled objects.")]
        public Transform poolParent = null;

        /// <summary>
        /// [Optional] Something special to be performed when new items are created.
        /// </summary>
        public GameObjectMethod InitPoolableMethod;// = (p) => p = null;//no-op so delegate never null

        /// <summary>
        /// [Optional] What should be done when item is Depooled?
        /// By default calls SetActive(true);
        /// </summary>
        public GameObjectMethod OnDepoolMethod = (p) => p.SetActive(true);

        /// <summary>
        /// [Optional] What should be done when item is Enpooled?
        /// By default calls SetActive(false);
        /// </summary>
        public GameObjectMethod OnEnpoolMethod = (p) => p.SetActive(false);

        // runtime data
        private Stack<GameObject> pool = new Stack<GameObject>(); //stack has better locality than queue
        private List<GameObject> manifest;

        /// <summary>
        /// Every GameObject Managed by this pool, non- and active alike.
        /// </summary>
        public IList<GameObject> Manifest { get => manifest; }

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

        /// <summary>
        /// <see langword="true"/> after <see cref="Init"/> has been completed,
        /// otherwise <see langword="false"/>.
        /// </summary>
        /// <remarks>Enables idempotency of <see cref="Init"/>.</remarks>
        private bool isInitialized;

        #region Unity Messages

        protected override void Reset()
        {
            base.Reset();
            SetDevDescription("I replace Instantiate with a pool of objects. " +
                "I'm doing my part to reduce garbage generation.");
            poolParent = transform;
        }

        private void Start()
        {
            if (initOnStart)
                Init();
        }

        #endregion Unity Messages

        public void AddItems(int amount = 1)
        {
            for (int i = amount - 1; i >= 0; --i)
            {
                GameObject obj = CreatePoolable();
                if (obj != null)
                    Enpool(obj);
            }
        }

        #region En/Depool

        /// <summary>
        /// Take an item out of the pool.
        /// </summary>
        /// <returns>Newly de-pool object.</returns>
        public GameObject Depool()
        {
            GameObject depooledItem;

            if (pool.Count > 0)
                depooledItem = pool.Pop();
            else
                depooledItem = CreatePoolable(); //or not

            if (depooledItem != null)
                OnDepoolMethod?.Invoke(depooledItem);//by default SetsActive(true), like Instantiate

            return depooledItem;
        }

        /// <summary>
        /// Take an item out of the pool and place at world space with rotation.
        /// </summary>
        /// <returns>Newly de-pool object.</returns>
        public GameObject Depool(Transform handle)
        {
            GameObject obj = Depool();
            if (obj != null)
            {
                Transform trans = obj.transform;
                trans.SetPositionAndRotation(handle.position, handle.rotation);
                trans.parent = handle;
                
                // what about: set parent, zero locals?
            }
            return obj;
        }

        /// <summary>
        /// Take an item out of the pool and place at world space with rotation.
        /// </summary>
        /// <returns>Newly de-pool object.</returns>
        public GameObject Depool(Transform handle, bool setParent)
        {
            GameObject obj = setParent ? Depool(handle) : Depool(handle.position, handle.rotation);
            return obj;
        }

        /// <summary>
        /// Take an item out of the pool and place at world space.
        /// </summary>
        /// <returns>Newly de-pool object.</returns>
        public GameObject Depool(in Vector3 position)
        {
            GameObject obj = Depool();
            if (obj != null)
            {
                obj.transform.position = position;
            }
            return obj;
        }

        /// <summary>
        /// Take an item out of the pool and set at world space with orientation.
        /// </summary>
        /// <returns>Newly de-pool object.</returns>
        public GameObject Depool(in Vector3 position, in Quaternion rotation)
        {
            GameObject obj = Depool();
            if (obj != null)
            {
                obj.transform.SetPositionAndRotation(position, rotation);
            }
            return obj;
        }

        /// <summary>
        /// Take an item out of the pool and GetComponent{T} on it.
        /// </summary>
        /// <returns>Newly de-pool object or null if the <seealso cref="Component"/> wasn't found.</returns>
        public T Depool<T>() where T : Component
        {
            T component = null;
            GameObject obj = Depool();
            if (obj)
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
        public void DoDepool(in Vector3 point) => Depool(point);

        public void Enpool(MonoBehaviour poolable)
            => Enpool(poolable.gameObject);

        /// <summary>
        /// Add an item back into the pool. This replaces Destroy().
        /// </summary>
        public void Enpool(GameObject poolable)
        {
            if (poolable == null)
                throw new Exception("[GameObjectPool] Trying to Enpool null!");

            Debug.AssertFormat(manifest.Contains(poolable),
                "[GameObjectPool] This item is not included on this Pool's manifest. " +
                "This item probably belongs to another Pool. " +
                "pendingPoolable: {0}. Pool: {1}. manifest[0] {2}.",
                poolable, gameObject, manifest[0]);

            if (!pool.Contains(poolable))//guard against multiple entries
            {
                pool.Push(poolable);
                OnEnpoolMethod?.Invoke(poolable);//by default sets inactive.
            }
        }

        #endregion En/Depool

        #region Initialization

        private GameObject CreatePoolable()
        {
            GameObject newGameObject = null;//return value
            if (maxAmount >= 0 && PopulationCount >= maxAmount)
            {
                Debug.Log("[" + name + "] Pool is exhausted. "
                    + "Count: " + ConstStrings.GetCachedString(maxAmount)
                    + ". Consider increasing 'maxAmount' or setting 'createWhenEmpty'."
                    , this);
            }
            else
            {
                newGameObject = Instantiate(objectPrefab, poolParent);
                manifest.Add(newGameObject);//track
            }

            return newGameObject;
        }

        /// <summary>
        /// Create entire pool
        /// </summary>
        public void Init()
        {
            // prevent double-init'g
            if (isInitialized)
                return;

            // validate
            objectPrefab.ShouldNotBeNull();

            // work
            int poolSize = RichMath.Max(startingAmount, maxAmount);
            manifest = new List<GameObject>(poolSize);
            pool = new Stack<GameObject>(poolSize);

            //load children?
            if (enpoolChildrenOnInit && poolParent != null)
            {
                foreach (Transform child in poolParent)
                    InitPoolable(child.gameObject);
            }

            //preload pool
            for (int i = startingAmount - pool.Count; i > 0; --i)
                InitPoolable(CreatePoolable());

            //state
            isInitialized = true;
        }

        public void InitPool(int startingAmount, int maxAmount)
        {
            this.startingAmount = startingAmount;
            this.maxAmount = maxAmount;
            Init();
        }

        private void InitPoolable(GameObject obj)
        {
            InitPoolableMethod?.Invoke(obj);

            pool.Push(obj);
            OnEnpoolMethod?.Invoke(obj);//SetActive(false) by default
        }

        #endregion Initialization

        /// <summary>
        /// Resize the pool.
        /// </summary>
        /// <param name="newCapacity"></param>
        public void Resize(int newCapacity)
        {
            maxAmount = RichMath.Min(newCapacity, ReadyCount);
            while (pool.Count > maxAmount) //shrink
            {
                GameObject poolable = pool.Pop();//trim excess
                manifest.Remove(poolable);
                Destroy(poolable);
            }

            manifest.TrimExcess();
            pool.TrimExcess();
        }

        public void ReturnAllToPool()
        {
            // iterate backwards to preserve stack order
            for (int i = manifest.Count - 1; i >= 0; --i)
                Enpool(manifest[i]);
        }
    }
}
