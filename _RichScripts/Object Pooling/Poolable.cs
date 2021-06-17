using UnityEngine;
using NaughtyAttributes;

/// <summary>
/// A basic Poolable MonoBehaviour.
/// </summary>
public class Poolable : RichMonoBehaviour, IPoolable
{
    [MinValue(0)]
    public float lifetime = 10f;

    public GameObjectPool PoolOwner { get; set; }

    [ShowNativeProperty]
    public bool InUse { get; private set;}

    private Coroutine lifetimeCoroutine;

    public virtual void OnCreate()
    {
        //nada
    }

    public virtual void OnDepool()
    {
        InUse = true;
        gameObject.SetActive(true);
        if (lifetime > 0)
        {
            lifetimeCoroutine = StartCoroutine(
                Utility.InvokeAfterDelay(ReturnToPool, lifetime));
        }
    }

    public virtual void OnEnpool()
    {
        InUse = false;
        if (lifetimeCoroutine != null)
        {
            StopCoroutine(lifetimeCoroutine);
        }
        gameObject.SetActive(false);
    }

    [Button("ReturnToPool()", EButtonEnableMode.Playmode)]
    public void ReturnToPool() => PoolOwner.Enpool(this);
}
