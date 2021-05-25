using UnityEngine;

[RequireComponent(typeof(GameObjectPool))]
public class Launcher : RichMonoBehaviour, ILaunchable
{
    /// <summary>
    /// Pool of projectiles.
    /// </summary>
    protected GameObjectPool projectilePool;

    protected override void Awake()
    {
        base.Awake();
        projectilePool = GetComponent<GameObjectPool>();
    }

    protected virtual void Start()
    {
        projectilePool.InitPool();
    }

    public virtual void Launch(Transform spawnPoint)
    {
        //get obj from pool
        var projectile = projectilePool.Depool<RichMonoBehaviour>();

        if (!projectile)//pool empty
        {
            Debug.Log("Pool Empty");
            return;
        }

        //orient towards target
        projectile.transform.position = spawnPoint.position;
        projectile.transform.rotation = spawnPoint.rotation;
    }
}
