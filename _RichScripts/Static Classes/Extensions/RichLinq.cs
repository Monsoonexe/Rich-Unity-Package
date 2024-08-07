/* TODO - can't the Sorted calls be replaced by "OrderBy"?
 * 
 */

using System.Collections.Generic;

namespace System.Linq
{
    public static class RichLinq
    {
        public static List<T> Sorted<T>(this IEnumerable<T> src, Comparison<T> comparison)
        {
            return Sorted(src, new List<T>(), comparison);
        }

        public static List<T> Sorted<T>(this IEnumerable<T> src, List<T> list, Comparison<T> comparison)
        {
            list.AddRange(src);
            list.Sort(comparison);
            return list;
        }

        public static List<T> Sorted<T>(this IEnumerable<T> src, List<T> list, IComparer<T> sorter)
        {
            list.AddRange(src);
            list.Sort(sorter);
            return list;
        }

        public static List<T> Sorted<T>(this IEnumerable<T> src, List<T> list)
        {
            list.AddRange(src);
            list.Sort();
            return list;
        }

        public static List<T> Sorted<T>(this IEnumerable<T> src)
            => Sorted(src, new List<T>());

        public static List<T> Sorted<T>(this List<T> list, IComparer<T> comparer)
        {
            list.Sort(comparer);
            return list;
        }

        public static int Remove<T>(this List<T> list, Predicate<T> predicate)
        {
            int initialCount = list.Count;

            // early exit
            if (initialCount == 0)
                return 0;

            // check and remove
            for (int i = initialCount - 1; i >= 0; i--)
            {
                if (predicate(list[i]))
                {
                    list.RemoveAt(i);
                }
            }

            return initialCount - list.Count;
        }
    }
}
