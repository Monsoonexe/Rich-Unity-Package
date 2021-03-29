using UnityEngine;

public class HardpointController : RichMonoBehaviour
{
    [Header("---Stats---")]
    [SerializeField] private float shotDelay = 1.0f;

    private float nextShootTime = 0.0f;

    /// <summary>
    /// Time until next Shot can be fired.
    /// </summary>
    public float CooldownTime { get
        {
            var cooldown = nextShootTime - Time.time;
            return cooldown < 0 ? 0 : cooldown;
        } }

    [Header("---Scene Refs---")]
    [SerializeField] private Transform[] projectileSpawnPoints;

    public Transform[] SpawnPoints { get => projectileSpawnPoints; }

    [Header("---Audio---")]
    [SerializeField] private AudioClip bangSound;

    private int projectileSpawnIndex = 0;

    /// <summary>
    /// Cached value.
    /// </summary>
    private int projSpawnPointCount = 0; // ++performance : --memory

    private AudioOptions laserSoundOptions = new AudioOptions // must explicitly initialize
    { loop = false, pitchShift = true, priority = 127, volume = 1.0f };

    //member components
    private ILaunchable launcher;

    protected override void Awake()
    {
        base.Awake();
        launcher = GetComponent<ILaunchable>();
        projSpawnPointCount = projectileSpawnPoints.Length;
    }

    protected virtual bool CanShoot()
    {
        return Time.time > nextShootTime;
    }

    public void FireWeapon()
    {
        if (CanShoot())
        {
            nextShootTime = Time.time + shotDelay;
            launcher.Launch(GetNextSpawnPoint()); // do the thing
            AudioManager.PlaySFX(bangSound, laserSoundOptions); // can include options
        }
    }

    /// <summary>
    /// Get the next point at which to spawn a projectile.
    /// </summary>
    /// <returns></returns>
    public Transform GetNextSpawnPoint()
    {
        projectileSpawnIndex = projectileSpawnIndex >= projSpawnPointCount ? 
            0 : projectileSpawnIndex;

        return projectileSpawnPoints[projectileSpawnIndex++];
    }
}
