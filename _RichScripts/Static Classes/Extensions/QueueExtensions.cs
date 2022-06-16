using System.Runtime.CompilerServices;
using System.Collections.Generic;

namespace RichPackage
{
    /// <seealso cref="StackExtensions"/>
    internal static class QueueExtensions
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
        public static T DequeueOrDefault(this Queue<T> q, T defaultValue = default)
            => q.IsEmpty() ? defaultValue : q.Dequeue();
    }
}
