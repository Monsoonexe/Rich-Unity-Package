using UnityEngine;
using UnityEngine.UI;
using ScriptableObjectArchitecture;
using NaughtyAttributes;

/// <summary>
/// Always keeps given number of items spawned.
/// </summary>
[RequireComponent(typeof(GameObjectPool))]
public class SpawnCounter : RichMonoBehaviour
{
    [Header("---Resources---")]
    [SerializeField]
    private IntReference targetData = new IntReference();

    //member Components
    private GameObjectPool objectPool;
    
    protected override void Awake()
    {
        base.Awake();
        objectPool = GetComponent<GameObjectPool>();
        objectPool.poolParent = myTransform;
    }

    private void OnEnable()
    {
        targetData.AddListener(UpdateSpawnCount);
    }
    private void OnDisable()
    {
        targetData.RemoveListener(UpdateSpawnCount);
    }

    [Button(null, EButtonEnableMode.Playmode)]
    public void UpdateSpawnCount()
    {
        //need to create more
        while(targetData.Value > objectPool.InUseCount)
        {
            var item = objectPool.Depool();//(spawn)
            if (item == null) break;//pool is exhausted
        }

        //have too many
        while(targetData.Value < objectPool.InUseCount)
        {
            var itemInUse = objectPool.Manifest.Find(
                (obj) => obj.activeSelf == true);//look for an active item
            if (itemInUse == null) break;//break if 0
            objectPool.Enpool(itemInUse);//return to pool (despawn)
        }
    }
}
