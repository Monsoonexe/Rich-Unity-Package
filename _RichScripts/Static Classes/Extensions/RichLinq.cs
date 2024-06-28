using System;
using System.Collections.Generic;

namespace RichPackage.Linq
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
    }
}
