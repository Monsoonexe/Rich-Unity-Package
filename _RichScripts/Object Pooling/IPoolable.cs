/// <summary>
/// Used by a pool. Should be a MonoBehaviour.
/// </summary>
public interface IPoolable
{
    GameObjectPool PoolOwner { get; set; }
    void OnCreate();
    void OnEnpool();
    void OnDepool();
}

public static class IPoolable_Extensions
{
    public static void ReturnToPool(this IPoolable p) => p.PoolOwner.Enpool(p);
}
