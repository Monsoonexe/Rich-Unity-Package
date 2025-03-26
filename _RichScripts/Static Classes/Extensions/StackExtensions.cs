using System.Runtime.CompilerServices;

namespace System.Collections.Generic
{
    /// <seealso cref="QueueExtensions"/>
    public static class StackExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void PushRange<T>(this Stack<T> s, IEnumerable<T> e)
        {
            foreach (T i in e)
                s.Push(i);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEmpty<T>(this Stack<T> s)
            => s.Count == 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNotEmpty<T>(this Stack<T> s)
            => s.Count > 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T PopOrDefault<T>(this Stack<T> s, T defaultValue = default)
            => s.IsEmpty() ? defaultValue : s.Pop();

        /// <summary>
        /// Tries to pop an item from the stack.
        /// </summary>
        /// <returns>True if an item was popped.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Drop<T>(this Stack<T> s)
        {
            if (s.Count == 0)
                return false;

            s.Pop();
            return true;
        }
    }
}
