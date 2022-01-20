using System;
using System.Collections.Generic;

public class ClassPool<T>
    where T : class, new()
{
    private Stack<T> pool;

    /// <summary>
    /// Can override this to provide a custom initialization for the pooled object.
    /// </summary>
    public Func<T> factory = () => new T(); //call default constructor

    public int Count => pool.Count;

    public int MaxCount { get; set; } = -1;

    public ClassPool(int maxCount = -1)
    {
        MaxCount = maxCount;
        int amount = MaxCount >= 0 ? MaxCount : 16;
        pool = new Stack<T>(amount);
    }

    /// <summary>
    /// Will always return an intance of T.
    /// </summary>
    public T Depool()
    {
        T depooledItem = null;

        if (pool.Count > 0)
            depooledItem = pool.Pop();
        else
            depooledItem = factory(); // create a new one

        return depooledItem;
    }

    /// <summary>
    /// Will only enpool if the pool is not full.
    /// </summary>
    public void Enpool(T item)
    {
        if(MaxCount < 0 || pool.Count < MaxCount)
            pool.Push(item);
    }
}
