using System.Runtime.CompilerServices;
using System.Threading;

namespace RichPackage.Threading
{
    /// <summary>
    /// Thread-safe counter.
    /// </summary>
    internal struct AtomicCounter
    {
        private int counter;

        public AtomicCounter(int value)
        {
            counter = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set(int x)
        {
            Interlocked.Exchange(ref counter, x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Add(int x)
        {
            return Interlocked.Add(ref counter, x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Sub(int x)
        {
            return Interlocked.Add(ref counter, -x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Increment()
        {
            return Interlocked.Increment(ref counter);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Decrement()
        {
            return Interlocked.Decrement(ref counter);
        }

        /// <summary>
        /// Thread-safe read.
        /// </summary>
        public int Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Interlocked.Add(ref counter, 0);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => Interlocked.Exchange(ref counter, value);
        }

        /// <summary>
        /// Non-thread-safe read.
        /// </summary>
        public int UnsafeValue => counter;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator int(AtomicCounter a) => a.Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static AtomicCounter operator --(AtomicCounter a)
        {
            Interlocked.Decrement(ref a.counter);
            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static AtomicCounter operator ++(AtomicCounter a)
        {
            Interlocked.Increment(ref a.counter);
            return a;
        }
    }
}
