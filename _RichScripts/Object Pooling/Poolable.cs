using UnityEngine;

/// <summary>
/// A basic Poolable MonoBehaviour.
/// </summary>
public class Poolable : RichMonoBehaviour, IPoolable
{
    public float lifetime = 10f;

    public GameObjectPool PoolOwner { get; set; }

    public bool InUse { get; }

    private Coroutine lifetimeCoroutine;

    public virtual void OnCreate()
    {
        //nada
    }

    public virtual void OnDepool()
    {
        gameObject.SetActive(true);
        if (lifetime > 0)
        {
            lifetimeCoroutine = StartCoroutine(
                Utility.InvokeAfterDelay(ReturnToPool, lifetime));
        }
    }

    public virtual void OnEnpool()
    {
        if (lifetimeCoroutine != null)
        {
            StopCoroutine(lifetimeCoroutine);
        }
        gameObject.SetActive(false);
    }

    public void ReturnToPool()
        => PoolOwner.Enpool(this);
}
