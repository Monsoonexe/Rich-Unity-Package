using System.Collections.Generic;
using System.Runtime.CompilerServices;

/// <seealso cref="StackExtensions"/>
public static class QueueExtensions
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static void EnqueueRange<T>(this Queue<T> q, IEnumerable<T> e)
    {
        foreach (T i in e)
            q.Enqueue(i);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsEmpty<T>(this Queue<T> q)
        => q.Count == 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IsNotEmpty<T>(this Queue<T> q)
        => q.Count > 0;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T DequeueOrDefault<T>(this Queue<T> q, T defaultValue = default)
        => q.IsEmpty() ? defaultValue : q.Dequeue();

    /// <summary>
    /// If possible, dequeues an item.
    /// </summary>
    /// <returns>True if an item was dequeued.</returns>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool Drop<T>(this Queue<T> q)
    {
        if (q.IsEmpty())
            return false;

        q.Dequeue();
        return true;
    }
}