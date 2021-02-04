using UnityEngine;

/// <summary>
/// 
/// </summary>
/// <typeparam name="TContainer">Weighted</typeparam>
/// <typeparam name="TValue"></typeparam>
/// <seealso cref="RandomIntGenerator"/> see for example
public abstract class ARandomGeneratorBase<TContainer, TValue> : RichScriptableObject
    where TContainer : AWeightedProbability<TValue>
{
    [SerializeField]
    protected TContainer[] availablePool = new TContainer[0]; // only YOU can prevent nullrefs

    /// <summary>
    /// Peek or examine the pool. This could be on a SO, so don't go changing this.
    /// </summary>
    public TContainer[] Pool { get => availablePool; }

    /// <summary>
    /// TV that displays results.
    /// </summary>
    protected TValue[] cachedResults; // use to reduce garbage

    /// <summary>
    /// Return a randomly drawn element from array using weights.
    /// </summary>
    /// <returns></returns>
    public virtual TValue GetNextRandom() => 
        AWeightedProbability.GetWeightedRandomElement(availablePool);

    /// <summary>
    /// Used to get a pool of objects.
    /// </summary>
    /// <param name="quantity"></param>
    /// <returns></returns>
    public virtual TValue[] GetNext(int quantity)
    {
        //get new or resize array
        if(cachedResults == null || cachedResults.Length < quantity)
            cachedResults = new TValue[quantity];

        for(var i = 0; i < quantity; ++i)
        {
            cachedResults[i] = GetNextRandom();
        }

        return cachedResults;
    }

    /// <summary>
    /// Assign a new pool of WeightedProbabilities to draw from.
    /// Note: This change will persist when exiting PlayMode. Do not pre-load data into
    /// an ARandomGen you expect to change the pool.
    /// </summary>
    /// <param name="newPool"></param>
    public void AssignNewPool(TContainer[] newPool)
        => availablePool = newPool;

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
