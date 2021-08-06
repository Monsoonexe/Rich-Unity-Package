using UnityEngine;
using RichPackage.Tweening;

/// <summary>
/// Every x seconds, spawn a new hazard.
/// </summary>
[RequireComponent(typeof(GameObjectPool))]
public class SpawnOnTimer : RichMonoBehaviour
{
    [Header("---Settings---")]
    [SerializeField]
    private Vector2 randomTimeIntervalBounds = new Vector2(1.5f, 4.0f);

    [SerializeField]
    [Tooltip("-1 means immortal.")]
    [Min(-1)]
    private float itemLifetime = 10.0f;

    [Header("---Prefab Refs---")]
    [SerializeField]
    private Transform spawnPoint;

    private GameObjectPool hazardPool;

    private void Reset()
    {
        SetDevDescription("I spawn items from a pool at time intervals.");
        spawnPoint = GetComponent<Transform>();
    }

    protected override void Awake()
    {
        base.Awake();
        hazardPool = GetComponent<GameObjectPool>();
        InitNextSpawn();
    }

    private void InitNextSpawn()
    {
        //randomly select the next time to spawn
        var randomDelay = randomTimeIntervalBounds.RandomRange();

        //wait that much time, then call this method
        RichTweens.InvokeAfterDelay(SpawnNextHazard, randomDelay);
    }

    private void SpawnNextHazard()
    {
        //(instantiate)
        var newItem = hazardPool.Depool(
            spawnPoint.position, Quaternion.identity);

        //check if pool is exhausted.
        if (!newItem)
        {   //pool is empty, consider raising limits
            //try again later
            InitNextSpawn();//chain forever
            return;
        }

        //reclaim after some time (unless immortal)
        if(itemLifetime > 0)
            RichTweens.InvokeAfterDelay(
                () => hazardPool.Enpool(newItem), itemLifetime);

        InitNextSpawn();//chain forever
    }

}
