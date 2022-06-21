using System.Collections.Generic;

namespace RichPackage
{
    /// <summary>
    /// 
    /// </summary>
    public static class EnumerableExtensions
    {
        /// <summary>
        /// Convert <paramref name="source"/> to a <see cref="HashSet{T}"/>.
        /// </summary>
        // Ref: https://stackoverflow.com/questions/34858338/how-to-elegantly-convert-ienumerablet-to-hashsett-at-runtime-without-knowing
        public static HashSet<T> ToHashSet<T>(this IEnumerable<T> source)
        {
            return new HashSet<T>(source);
        }
    }
}
