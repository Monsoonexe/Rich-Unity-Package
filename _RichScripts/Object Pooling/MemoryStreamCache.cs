namespace System.IO
{
    public static class MemoryStreamCache
    {
        internal const int MAX_STREAM_SIZE = 2048;

        [ThreadStatic]
        private static MemoryStream CachedInstance;

        public static MemoryStream Rent() => Rent(MAX_STREAM_SIZE);

        public static MemoryStream Rent(int capacity)
        {
            MemoryStream cachedInstance;
            if (capacity <= MAX_STREAM_SIZE)
            {
                cachedInstance = CachedInstance;
                if (cachedInstance != null)
                {
                    CachedInstance = null;
                    return cachedInstance;
                }
            }

            return new MemoryStream(capacity);
        }

        public static PooledObject Rent(out MemoryStream memoryStream)
        {
            return new PooledObject(memoryStream = Rent());
        }

        public static void Return(MemoryStream ms)
        {
            int capacity = ms.Capacity;
            if (capacity <= MAX_STREAM_SIZE
                && (CachedInstance == null || CachedInstance.Capacity < capacity))
            {
                CachedInstance = ms;
                ms.Position = 0;
            }
        }

        /// <summary>
        /// Pooled object.
        /// </summary>
        public struct PooledObject : IDisposable
        {
            private readonly MemoryStream memoryStream;

            internal PooledObject(MemoryStream memoryStream)
            {
                this.memoryStream = memoryStream;
            }

            /// <summary>
            /// Disposable pattern implementation.
            /// </summary>
            void IDisposable.Dispose() => Return(memoryStream);
        }
    }
}