using System;
using System.Text;

namespace RichPackage.Pooling
{
    /// <summary>
    /// A thread-safe <see cref="StringBuilder"/> that can be re-used 
    /// </summary>
    public static class StringBuilderCache
    {
        internal const int MAX_BUILDER_SIZE = 360;

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
                }
            }
			else
			{
                cachedInstance = new StringBuilder(capacity);
            }

            return cachedInstance;
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
        public static string GetStringAndReturn(this StringBuilder sb)
        {
            string result = sb.ToString();
            Return(sb);
            return result;
        }

    }
}
