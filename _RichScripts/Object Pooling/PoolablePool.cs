using System.Collections.Generic;
using UnityEngine;

namespace RichPackage.Pooling
{
    /// <summary>
    /// A Pool for GameObject prefabs that implement the IPoolable Interface.
    /// </summary>
    public class PoolablePool : RichMonoBehaviour
    {
        [Header("---Prefab Pool Resources---")]
        public GameObject objectPrefab;

        [Header("---Pool Base Settings---")]
        public bool InitOnAwake = true;

        public bool createWhenEmpty = true;

        [SerializeField, MinValue(0)]
        private int startingAmount = 6;

        [SerializeField, MinValue(0)]
        [Tooltip("less than 0 means 'no limit'.")]
        private int maxAmount = 10;
        
        [Tooltip("[Optional]")]
        public Transform poolParent = null;

        //runtime data
        public StackPool<IPoolable> Pool { get; private set; }

        private List<IPoolable> manifest;

        /// <summary>
        /// Total items this Pool tracks.
        /// </summary>
        public int PopulationCount { get => manifest.Count; }

        /// <summary>
        /// How many items are ready to be deployed.
        /// </summary>
        public int ReadyCount { get => Pool.Count; }

        /// <summary>
        /// Number of items currently depooled.
        /// </summary>
        public int InUseCount { get => manifest.Count - Pool.Count; }
        //private Queue<IPoolable> queuePool;
        
        private void OnValidate()
        {   
             //ensure prefab has an IPoolable Component on it 
            if (objectPrefab != null && objectPrefab.GetComponent<IPoolable>() == null)
            {
                Debug.LogError("[PoolablePool] Prefab {" + objectPrefab.name
                    + "} does not have an IPoolable Component", objectPrefab);
                objectPrefab = null;
            }
        }

        protected override void Awake()
        {
            base.Awake();
            manifest = new List<IPoolable>(maxAmount);
            if(InitOnAwake)
                InitPool();
        }

        protected virtual IPoolable CreatePoolable()
        {
            var newGameObj = Instantiate(objectPrefab, poolParent);
            var poolable = newGameObj.GetComponent<IPoolable>();

            //validate
            Debug.Assert(poolable != null,
                "[PoolablePool] No Poolable found on " + objectPrefab, objectPrefab);

            //init
            poolable.OnCreate();
            poolable.PoolOwner = this; //know where you came from
            manifest.Add(poolable);//track
            
            return poolable;
        }

        /// <summary>
        /// Take an item out of the pool.
        /// </summary>
        /// <returns></returns>
        public virtual IPoolable Depool() => Pool.Depool();

        public T Depool<T>() where T : class => Depool() as T;

        /// <summary>
        /// Add an item back into the pool.
        /// </summary>
        /// <param name="poolable"></param>
        public virtual void Enpool(IPoolable poolable)
        {
            Debug.Assert(poolable.PoolOwner == this,
                "[PoolablePool] Pool Error! Enpooled to non-owning Pool!", this);

            if (!Pool.Stack.Contains(poolable))//guard against multiple entries
                Pool.Enpool(poolable);
        }

        /// <summary>
        /// Create entire pool
        /// </summary>
        public virtual async void InitPool()
        {
            Pool = new StackPool<IPoolable>(maxAmount, startingAmount)
            {
                FactoryMethod = CreatePoolable,
                OnDepoolMethod = (p) => p.OnDepool(),
                OnEnpoolMethod = (p) => p.OnEnpool(),
                CreateNewWhenEmpty = createWhenEmpty,
            };
            Pool.Add(startingAmount);
        }

        /// <summary>
        /// Resize the pool.
        /// </summary>
        /// <param name="newCapacity"></param>
        public virtual void Resize(int newCapacity)
        {
            //if (!resizeable) return;
            //Debug.Assert(resizeable, "[PoolablePool] Attempting to resize a marked non-resizable pool.");

            maxAmount = Mathf.Min(newCapacity, ReadyCount);
            while (Pool.Count > maxAmount) //shrink
            {
                var poolable = Pool.Depool();//trim excess
                manifest.Remove(poolable);
            }

            manifest.TrimExcess();
            Pool.Stack.TrimExcess();
        }

        public void ReturnAllToPool()
        {
            var count = manifest.Count;
            for (var i = 0; i < count; ++i)
                Enpool(manifest[i]);
        }
    }
}
