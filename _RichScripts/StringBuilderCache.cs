namespace System.Text
{
    /// <summary>
    /// A thread-safe <see cref="StringBuilder"/> that can be re-used.
    /// </summary>
    public static class StringBuilderCache
    {
        internal const int MAX_BUILDER_SIZE = 512;

        [ThreadStatic]
        private static StringBuilder CachedInstance;

        public static StringBuilder Rent()
            => Rent(MAX_BUILDER_SIZE);

        public static StringBuilder Rent(int capacity)
        {
            StringBuilder cachedInstance;
            if (capacity <= MAX_BUILDER_SIZE)
            {
                cachedInstance = CachedInstance;
                if (cachedInstance != null && capacity <= cachedInstance.Capacity)
                {
                    CachedInstance = null;
                    cachedInstance.Clear();
                    return cachedInstance;
                }
            }

            return new StringBuilder(capacity);
        }

        public static PooledObject Rent(out StringBuilder builder)
        {
            return new PooledObject(builder = Rent());
        }

        public static void Return(StringBuilder sb)
        {
            if (sb.Capacity <= MAX_BUILDER_SIZE)
                CachedInstance = sb;
        }

        /// <summary>
        /// Get the resulting <see cref="string"/> and return to the <see cref="StringBuilderCache"/>.
        /// </summary>
        /// <param name="sb">A <see cref="StringBuilder"/> instance
        /// <see cref="StringBuilderCache.Rent"/>ed from
        /// <see cref="StringBuilderCache"/>.</param>
        public static string ToStringAndReturn(this StringBuilder sb)
        {
            string result = sb.ToString();
            Return(sb);
            return result;
        }

        /// <summary>
        /// Pooled object.
        /// </summary>
        public struct PooledObject : IDisposable
        {
            private readonly StringBuilder stringBuilder;

            internal PooledObject(StringBuilder stringBuilder)
            {
                this.stringBuilder = stringBuilder;
            }

            /// <summary>
            /// Disposable pattern implementation.
            /// </summary>
            void IDisposable.Dispose() => Return(stringBuilder);
        }
    }
}
