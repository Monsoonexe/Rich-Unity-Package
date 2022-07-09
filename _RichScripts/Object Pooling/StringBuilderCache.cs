using System;
using System.Text;

namespace RichPackage.Pooling
{
    /// <summary>
    /// A thread-safe <see cref="StringBuilder"/> that can be re-used 
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

        public static void Return(StringBuilder sb)
        {
            if (sb.Capacity <= MAX_BUILDER_SIZE)
                CachedInstance = sb;
        }

        /// <summary>
        /// Get the resulting <see cref="string"/> and returns instance 
        /// to the <see cref="StringBuilderCache"/>.
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

    }
}
