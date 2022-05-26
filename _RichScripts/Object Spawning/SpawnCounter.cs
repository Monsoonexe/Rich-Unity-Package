using UnityEngine;
using ScriptableObjectArchitecture;
using Sirenix.OdinInspector;
using RichPackage.Pooling;

namespace RichPackage.Spawning
{
    /// <summary>
    /// Always keeps given number of items spawned. Pairs well with LayoutGroups.
    /// </summary>
    /// <seealso cref="UICounter"/>
    [RequireComponent(typeof(GameObjectPool))]
    public class SpawnCounter : RichMonoBehaviour
    {
        [Header("---Resources---")]
        [SerializeField]
        private IntReference targetData = new IntReference();

        //member Components
        private GameObjectPool objectPool;

        public int Count
        {
            get => targetData.Value;
            set
            {
                targetData.Value = value;
                if (targetData.UseConstant) //manually update if not a ref
                    UpdateSpawnCount();
            }
        }

        protected override void Awake()
        {
            base.Awake();
            objectPool = GetComponent<GameObjectPool>();
            objectPool.InitPool();
        }

        private void OnEnable()
        {
            targetData.AddListener(UpdateSpawnCount);
            UpdateSpawnCount();
        }

        private void OnDisable()
        {
            targetData.RemoveListener(UpdateSpawnCount);
        }

        [Button, DisableInEditorMode]
        public void UpdateSpawnCount()
        {
            //need to create more
            while (targetData.Value > objectPool.InUseCount)
            {
                var item = objectPool.Depool();//(spawn)
                if (item == null) break;//pool is exhausted
            }

            //have too many
            while (targetData.Value < objectPool.InUseCount)
            {
                //local function to avoid lambda
                bool IsGameObjectActive(GameObject obj)
                    => obj.activeSelf == true;

                //look for an active item
                var itemInUse = objectPool.Manifest.Find(IsGameObjectActive);
                if (itemInUse == null) break;//break if not found
                objectPool.Enpool(itemInUse);//return to pool (despawn)
            }
        }
    }
}
