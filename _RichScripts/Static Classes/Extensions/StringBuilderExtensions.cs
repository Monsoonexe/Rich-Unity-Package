using System.Runtime.CompilerServices;

namespace System.Text
{
    internal static class StringBuilderExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string OutputAndClear(this StringBuilder source)
        {
            string output = source.ToString();
            source.Clear();
            return output;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StringBuilder ClearOrNew(this StringBuilder source)
        {
            if (source == null)
                source = new StringBuilder(128);
            else
                source.Clear();

            return source;
        }

        /// <summary>
        /// Remove the last <paramref name="count"/> characters from the <paramref name="source"/>.
        /// </summary>
        /// <returns><paramref name="source"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static StringBuilder RemoveLast(this StringBuilder source, int count)
        {
            return source.Remove(source.Length - count, count);
        }
    }
}
