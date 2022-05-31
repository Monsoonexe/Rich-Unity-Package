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

        public static StringBuilder Rent(int capacity = 64)
        {
            if (capacity <= MAX_BUILDER_SIZE)
            {
                StringBuilder cachedInstance = CachedInstance;
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
            {
                CachedInstance = sb;
            }
        }

        public static string GetStringAndRelease(StringBuilder sb)
        {
            string result = sb.ToString();
            Return(sb);
            return result;
        }
    }
}
