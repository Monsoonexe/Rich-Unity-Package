using UnityEngine;

public abstract class ARandomGeneratorBase<TContainer, TValue> : RichScriptableObject
    where TContainer : AWeightedProbability<TValue>
{
    [SerializeField]
    protected TContainer[] availablePool;

    /// <summary>
    /// Peek or examine the pool. This could be on a SO, so don't go changing this.
    /// </summary>
    public TContainer[] Pool { get => availablePool; }

    /// <summary>
    /// Return a randomly drawn element from array using weights.
    /// </summary>
    /// <returns></returns>
    public virtual TValue Draw() => 
        this.GetWeightedRandomElement();

    /// <summary>
    /// Total accumulated value of all weights in pool.
    /// </summary>
    public virtual int TotalWeight { get => availablePool.GetTotalWeight(); }

    #region Editor
#if UNITY_EDITOR
    
    /// <summary>
    /// Returns a CLONE of available pool, so readonly.
    /// </summary>
    public TContainer[] AvailablePool { get => availablePool.Clone() as TContainer[]; }

#endif
    #endregion
}
