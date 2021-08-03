using UnityEngine;

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
    private bool shouldOverrideDirection = true;

    [SerializeField]
    private Vector3 directionOverride = new Vector3(1, 0, 0);

    [SerializeField]
    [Tooltip("-1 means immortal.")]
    [Min(-1)]
    private float hazardLifetime = 10.0f;

    [Header("---Prefab Refs---")]
    [SerializeField]
    private Transform spawnPoint;

    private GameObjectPool hazardPool;

    private void Reset()
    {
        SetDevDescription("I spawn items from a pool at time intervals.");
    }

    private void Awake()
    {
        hazardPool = GetComponent<GameObjectPool>();
        InitNextSpawn();
    }

    private void InitNextSpawn()
    {
        //randomly select the next time to spawn
        var randomDelay = randomTimeIntervalBounds.RandomRange();

        //wait that much time, then call this method
        ApexTweens.InvokeAfterDelay(SpawnNextHazard, randomDelay);
    }

    private void SpawnNextHazard()
    {
        //(instantiate)
        var newHazard = hazardPool.Depool(
            spawnPoint.position, Quaternion.identity);

        //check if pool is exhausted.
        if (!newHazard)
        {   //pool is empty, consider raising limits
            //try again later
            InitNextSpawn();//chain forever
            return;
        }

        if(shouldOverrideDirection)
        {
            var hazard = newHazard.GetComponent<ProjectileHazard>();
            if (hazard != null)
                hazard.moveVector = directionOverride;
        }

        //reclaim after some time (unless immortal)
        if(hazardLifetime > 0)
            ApexTweens.InvokeAfterDelay(
                () => hazardPool.Enpool(newHazard), hazardLifetime);

        InitNextSpawn();//chain forever
    }

}
