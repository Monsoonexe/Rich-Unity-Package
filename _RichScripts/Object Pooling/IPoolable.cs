/// <summary>
/// Used by a pool. Should be a MonoBehaviour.
/// </summary>
public interface IPoolable
{
    GameObjectPool PoolOwner { get; set; }
    bool InUse { get; } // has been Depooled and is doing its thing.
    void OnCreate(); //when created
    void OnEnpool(); //into pool
    void OnDepool(); //out of pool
}

public static class IPoolable_Extensions
{
    /// <summary>
    /// Return to the Pool from which you came.
    /// </summary>
    /// <param name="p"></param>
    public static void ReturnToPool(this IPoolable p) => p.PoolOwner.Enpool(p);
}
