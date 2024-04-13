using RichPackage.Collections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace RichPackage
{
    /// <summary>
    /// My Collection Extension collection.
    /// </summary>
    public static class Collection_Extensions
    {
        #region Collection Helpers

        #region Average

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Average<T>(this IList<T> col, Func<T, int> summer)
            => col.Sum(summer) / col.Count;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Average<T>(this IList<T> col, Func<T, uint> summer)
            => col.Sum(summer) / (uint)col.Count;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Average<T>(this IList<T> col, Func<T, long> summer)
            => col.Sum(summer) / col.Count;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong Average<T>(this IList<T> col, Func<T, ulong> summer)
            => col.Sum(summer) / (ulong)col.Count;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Average<T>(this IList<T> col, Func<T, float> summer)
            => col.Sum(summer) / col.Count;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Average<T>(this IList<T> col, Func<T, double> summer)
            => col.Sum(summer) / col.Count;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Average(this IList<byte> col)
            => col.Sum() / col.Count;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Average(this IList<short> col)
            => col.Sum() / col.Count;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Average(this IList<int> col)
            => col.Sum() / col.Count;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Average(this IList<uint> col)
            => col.Sum() / (uint)col.Count;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Average(this IList<long> col)
            => col.Sum() / col.Count;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong Average(this IList<ulong> col)
            => col.Sum() / (ulong)col.Count;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float Average(this IList<float> col)
            => col.Sum() / col.Count;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double Average(this IList<double> col)
            => col.Sum() / col.Count;

        #endregion

        #region Contains

        /// <summary>
        /// Returns 'true' if at least 1 item in array `elem.CompareTo(other) == 0`.
        /// </summary>
        public static bool Contains<T>(this IList<T> array, IComparable<T> elem)
        {
            int length = array.Count;
            for (int i = 0; i < length; ++i)
                if (elem.CompareTo(array[i]) == 0)
                    return true;
            return false;
        }

        /// <summary>
        /// Returns 'true' if at least 1 item in array `elem.CompareTo(other) == 0`.
        /// </summary>
        public static bool Contains<T>(this IList<T> array, Searcher<T> searcher)
        {
            int length = array.Count;
            for (int i = 0; i < length; ++i)
                if (searcher(array[i]) == 0)
                    return true;
            return false;
        }

        /// <summary>
        /// Returns 'true' if at least 1 item in array `query(list) == true`.
        /// </summary>
        public static bool Contains<T>(this IList<T> list, Predicate<T> query)
        {
            bool contains = false; //return value
            int count = list.Count;
            for (int i = 0; i < count; ++i)
            {
                if (query(list[i]))
                {
                    contains = true;
                    break;
                }
            }
            return contains;
        }

        #endregion

        #region IsSequentiallyEqualTo

        /// <summary>
        /// A and B are the same size and every element in A is equal to the same position in B.
        /// </summary>
        /// <returns>True if A and B are the same size and every element in A is in B</returns>
        public static bool IsSequentiallyEqualTo<T>(this IList<T> a, IList<T> b)
            where T : class
        {
            int count = a.Count;

            if (count != b.Count)
                return false;

            for (int i = 0; i < count; ++i)
                if (!a[i].Equals(b[i]))
                    return false;

            return true;
        }

        /// <summary>
        /// A and B are the same size and every element in A is equal to the same position in B.
        /// </summary>
        /// <returns>True if A and B are the same size and every element in A is in B</returns>
        public static bool IsSequentiallyEqualTo(this IList<byte> a, IList<byte> b)
        {
            int count = a.Count;

            if (count != b.Count)
                return false;

            for (int i = 0; i < count; ++i)
                if (a[i] != b[i])
                    return false;

            return true;
        }

        /// <summary>
        /// A and B are the same size and every element in A is equal to the same position in B.
        /// </summary>
        /// <returns>True if A and B are the same size and every element in A is in B</returns>
        public static bool IsSequentiallyEqualTo(this IList<int> a, IList<int> b)
        {
            int count = a.Count;

            if (count != b.Count)
                return false;

            for (int i = 0; i < count; ++i)
                if (a[i] != b[i])
                    return false;

            return true;
        }

        /// <summary>
        /// A and B are the same size and every element in A is equal to the same position in B.
        /// </summary>
        /// <returns>True if A and B are the same size and every element in A is in B</returns>
        public static bool IsSequentiallyEqualTo(this IList<char> a, IList<char> b)
        {
            int count = a.Count;

            if (count != b.Count)
                return false;

            for (int i = 0; i < count; ++i)
                if (a[i] != b[i])
                    return false;

            return true;
        }

        #endregion IsSequentiallyEqualTo

        /// <summary>
        /// Returns the first item in <paramref name="list"/> that <paramref name="query"/> returns true.
        /// </summary>
        public static T Find<T>(this IList<T> list, Predicate<T> query)
        {
            int count = list.Count;
            for (int i = 0; i < count; ++i)
                if (query(list[i]))
                    return list[i];
            return default;
        }

        /// <summary>
        /// Returns the first item in <paramref name="list"/> that <paramref name="query"/> returns true.
        /// </summary>
        public static T Find<T>(this IEnumerable<T> collection, Predicate<T> query)
        {
            foreach (T item in collection)
                if (query(item))
                    return item;
            return default;
        }

        /// <summary>
        /// Fills a new <see cref="List{T}"/> with the results of <paramref name="query"/>.
        /// </summary>
        public static List<T> FindAll<T>(this IList<T> list, Predicate<T> query)
        {
            int count = list.Count;
            var listToFill = new List<T>(count);
            for (int i = 0; i < count; ++i)
                if (query(list[i]))
                    listToFill.Add(list[i]);
            return listToFill;
        }

        /// <summary>
        /// Fills <paramref name="list"/> with the results of <paramref name="query"/>.
        /// </summary>
        public static void FindAll<T>(this IList<T> list, Predicate<T> query, List<T> listToFill)
        {
            int count = list.Count;
            for (int i = 0; i < count; ++i)
                if (query(list[i]))
                    listToFill.Add(list[i]);
        }

        /// <summary>
        /// Returns the last item in <paramref name="list"/> that <paramref name="query"/> returns true.
        /// </summary>
        public static T FindLast<T>(this IList<T> list, Predicate<T> query)
        {
            for (int i = list.Count - 1; i >= 0; --i)
                if (query(list[i]))
                    return list[i];
            return default;
        }

        /// <summary>
        /// Returns the first item in <paramref name="list"/> that <paramref name="query"/> returns true.
        /// </summary>
        public static bool TryFind<T>(this IList<T> list, Predicate<T> query,
            out T foundItem)
        {
            foundItem = default;
            bool found = false;
            int count = list.Count;
            for (int i = 0; i < count; ++i)
            {
                if (query(list[i]))
                {
                    foundItem = list[i];
                    found = true;
                    break;
                }
            }
            return found;
        }

        /// <summary>
        /// Returns List with lowest Count from List of Lists.
        /// </summary>
        public static T GetShortestList<T>(this IList<T> lists)
            where T : IList
        {
            //first shortest path
            int shortestLength = int.MaxValue;
            int shortestIndex = 0;

            int count = lists.Count;
            for (int i = 0; i < count; ++i)
            {
                int workingCount = lists[i].Count;
                if (workingCount < shortestLength)
                {
                    shortestIndex = i;
                    shortestLength = workingCount;
                }
            }
            return lists[shortestIndex]; // assume the first is shortest
        }

        /// <summary>
        /// Returns List with lowest <see cref="IList.Count"/> from List of Lists and its index.
        /// </summary>
        public static T GetShortestList<T>(this IList<T> lists,
            out int shortestIndex) where T : IList
        {
            //first shortest path
            shortestIndex = 0;
            int shortestLength = int.MaxValue;
            int count = lists.Count;
            for (int i = 0; i < count; ++i)
            {
                int workingCount = lists[i].Count;
                if (workingCount < shortestLength)
                {
                    shortestIndex = i;
                    shortestLength = workingCount;
                }
            }
            return lists[shortestIndex]; // assume the first is shortest
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IndexIsInRange(this IList col, int index)
            => index >= 0 && index < col.Count;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullOrEmpty<T>(this ICollection<T> list)
            => list == null || list.Count == 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEmpty<T>(this ICollection<T> list)
            => list.Count == 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNotEmpty<T>(this ICollection<T> list)
            => list.Count > 0;

        /// <summary>
        /// A and B are the same size and every element in A is in B (order agnostic).
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>True if A and B are the same size and every element in A is in B</returns>
        public static bool IsEquivalentTo<T>(this IList<T> a, IList<T> b)
        {
            if (a.Count != b.Count)
                return false;

            foreach (T item in a)
            {
                if (!b.Contains(item))
                    return false;
            }
            return true;
        }

        /// <summary>
        /// Returns true if every element of 'a' is in 'b'.
        /// </summary>
        /// <param name="a">check if this one is a subset</param>
        /// <param name="b">"master" set</param>
        /// <returns></returns>
        public static bool IsSubsetOf<T>(this IList<T> a, IList<T> b)
        {
            foreach (T item in a)
            {
                if (!b.Contains(item))
                    return false;
            }
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<T> Sublist<T>(this IList<T> list, int startIndex)
            => Sublist(list, startIndex, list.Count - startIndex);

        public static List<T> Sublist<T>(this IList<T> list, int startIndex, int length)
        {
            //validate
            if (startIndex < 0)
                throw new ArgumentOutOfRangeException(nameof(startIndex));

            if (startIndex > list.Count)
                throw new ArgumentOutOfRangeException(nameof(startIndex));

            if (length < 0)
                throw new ArgumentOutOfRangeException(nameof(length));

            if (startIndex > list.Count - length)
                throw new ArgumentOutOfRangeException(nameof(length));

            //early exit
            if (length == 0)
                return new List<T>(0);

            //sublist
            var sublist = new List<T>(length); //return value
            int endIndex = startIndex + length;

            for (int i = startIndex; i < endIndex; ++i)
                sublist.Add(list[i]);

            return sublist;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T ElementAtWrapped<T>(this IList<T> list, int index)
            => list[GetWrappedIndex(list, index)];

        /// <summary>
        /// Always produces a valid index if <paramref name="list"/>'s size > 0. <br/>
        /// Supports negative indexing. e.g. list[-1] gives list index.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetWrappedIndex<T>(this IList<T> list, int index)
        {
            int count = list.Count;
            int indexWrapped = (int)(index - (count * Math.Floor(
                (double)index / count)));
            return indexWrapped;
        }

        /// <summary>
        /// Element at position 0.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T First<T>(this IList<T> col) => col[0];

        /// <summary>
        /// Element at 0 or the <see langword="default"/> value if empty.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T FirstOrDefault<T>(this IList<T> col, T defaultValue = default)
            => col.Count == 0 ? defaultValue : col[0];

        /// <summary>
        /// First element in a sequence or a default value if it is empty.
        /// </summary>
        public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source,
            TSource @default = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (source is IList<TSource> list)
            {
                return FirstOrDefault(list, @default);
            }

            using (IEnumerator<TSource> enumerator = source.GetEnumerator())
            {
                if (enumerator.MoveNext())
                    return enumerator.Current;
            }

            return @default;
        }

        /// <summary>
        /// First element in a sequence or a default value if it is empty.
        /// </summary>
        public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source,
            Predicate<TSource> query, TSource @default = default)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            if (query == null)
                throw new ArgumentNullException(nameof(query));

            foreach (TSource item in source)
            {
                if (query(item))
                {
                    return item;
                }
            }

            return @default;
        }

        /// <summary>
        /// Element at position Count - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Last<T>(this IList<T> col) => col[col.Count - 1];

        /// <summary>
        /// <see cref="Last"/> or <paramref name="defaultValue"/> if <paramref name="col"/>
        /// <see cref="IsNullOrEmpty"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T LastOrDefault<T>(this IList<T> col, T defaultValue)
            => col.IsNullOrEmpty()
            ? defaultValue
            : col[col.Count - 1];

        /// <summary>
        /// A 'foreach' with a 'for' backbone
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEach<T>(this IList<T> list, Action<T> action)
        {
            int count = list.Count; //cache for less function overhead on every iteration
            for (int i = 0; i < count; ++i)
                action(list[i]);
        }

        public static IEnumerable<(T Item, int Index)> ForEachWithIndex<T>(this IEnumerable<T> src)
        {
            int i = 0;
            foreach (var v in src)
                yield return (v, i++);
        }

        public static void ForEachWithIndex<T>(this IEnumerable<T> collection, Action<T, int> action)
        {
            int i = 0;
            foreach (T item in collection)
                action(item, i++);
        }

        public static IEnumerable<(T Item, int Index)> ForeachWithIndex<T>(this IEnumerable<T> source)
        {
            int i = 0;
            foreach (T item in source)
                yield return (item, i++);
        }

        /// <summary>
        /// A 'foreach' with a 'for' backbone
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ForEachBackwards<T>(this IList<T> list, Action<T> action)
        {
            for (int i = list.Count - 1; i >= 0; --i)
                action(list[i]);
        }

        /// <summary>
        /// Count - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int LastIndex(this IList col) => col.Count - 1;

        /// <summary>
        /// Compiler-driven cast to <see cref="IEnumerable{T}"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static IEnumerable<T> GetEnumerable<T>(this IList<T> array) => array;

        public static string ToSeparatedString<T>(this IList<T> source, string separator = ",")
        {
            var sb = new StringBuilder();
            foreach (T item in source)
                sb.Append(item.ToString())
                    .Append(separator);

            // remove final separator
            sb.Remove(sb.Length - separator.Length, separator.Length);

            return sb.ToString();
        }

        //TODO - make this mirror Select from LINQ
        public static TReturn[] ToSubArray<TArray, TReturn>(this IList<TArray> array,
            Func<TArray, TReturn> expression)
            => ToSubArray(array, expression, 0, array.Count);

        public static TReturn[] ToSubArray<TArray, TReturn>(this IList<TArray> array,
            Func<TArray, TReturn> expression, int offset)
            => ToSubArray(array, expression, offset, array.Count - offset);

        public static TReturn[] ToSubArray<TArray, TReturn>(this IList<TArray> array,
            Func<TArray, TReturn> expression, int offset, int count)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (expression == null)
                throw new ArgumentNullException(nameof(expression));

            if (count < 0 || count > array.Count)
                throw new ArgumentOutOfRangeException($"{nameof(count)} " +
                    $"is out of bounds: {count} : {array.Count}.");

            if (offset + count > array.Count || offset < 0)
                throw new ArgumentOutOfRangeException($"{nameof(offset)} " +
                    $"is out of bounds: {offset} : {array.Count}.");

            var result = new TReturn[count];
            for (int i = 0; i < count; ++i)
                result[i] = expression(array[i + offset]);
            return result;
        }

        public static IEnumerator<(T Item, int Index)> WithIndex<T>(this IEnumerable<T> e)
        {
            int index = 0;

            foreach (T item in e)
                yield return (item, index++);
        }

        #region Summation

        public static int Sum<T>(this IList<T> col, Func<T, int> summer)
        {
            int sum = 0;
            for (int i = col.Count - 1; i >= 0; --i)
                sum += summer(col[i]);

            return sum;
        }

        public static uint Sum<T>(this IList<T> col, Func<T, uint> summer)
        {
            uint sum = 0;
            for (int i = col.Count - 1; i >= 0; --i)
                sum += summer(col[i]);

            return sum;
        }

        public static float Sum<T>(this IList<T> col, Func<T, float> summer)
        {
            float sum = 0;
            for (int i = col.Count - 1; i >= 0; --i)
                sum += summer(col[i]);

            return sum;
        }

        public static double Sum<T>(this IList<T> col, Func<T, double> summer)
        {
            double sum = 0;
            for (int i = col.Count - 1; i >= 0; --i)
                sum += summer(col[i]);

            return sum;
        }

        public static long Sum<T>(this IList<T> col, Func<T, long> summer)
        {
            long sum = 0;
            for (int i = col.Count - 1; i >= 0; --i)
                sum += summer(col[i]);

            return sum;
        }

        public static ulong Sum<T>(this IList<T> col, Func<T, ulong> summer)
        {
            ulong sum = 0;
            for (int i = col.Count - 1; i >= 0; --i)
                sum += summer(col[i]);

            return sum;
        }

        public static int Sum(this IList<byte> col)
        {
            int sum = 0;
            for (int i = col.Count - 1; i >= 0; --i)
                sum += col[i];
            return sum;
        }

        public static int Sum(this IList<short> col)
        {
            int sum = 0;
            for (int i = col.Count - 1; i >= 0; --i)
                sum += col[i];
            return sum;
        }

        public static int Sum(this IList<int> col)
        {
            int sum = 0;
            for (int i = col.Count - 1; i >= 0; --i)
                sum += col[i];
            return sum;
        }

        public static long Sum(this IList<long> col)
        {
            long sum = 0;
            for (int i = col.Count - 1; i >= 0; --i)
                sum += col[i];
            return sum;
        }

        public static uint Sum(this IList<uint> col)
        {
            uint sum = 0;
            for (int i = col.Count - 1; i >= 0; --i)
                sum += col[i];
            return sum;
        }

        public static ulong Sum(this IList<ulong> col)
        {
            ulong sum = 0;
            for (int i = col.Count - 1; i >= 0; --i)
                sum += col[i];
            return sum;
        }

        public static float Sum(this IList<float> col)
        {
            float sum = 0.0f;
            for (int i = col.Count - 1; i >= 0; --i)
                sum += col[i];
            return sum;
        }

        public static double Sum(this IList<double> col)
        {
            double sum = 0.0d;
            for (int i = col.Count - 1; i >= 0; --i)
                sum += col[i];
            return sum;
        }

        public static long SumLong(this IList<int> col)
        {
            long sum = 0;
            for (int i = col.Count - 1; i >= 0; --i)
                sum += col[i];
            return sum;
        }

        #endregion

        /// <summary>
        /// Swap element at index a with element at index b.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Swap<T>(this IList<T> list, int a, int b)
        {
            // perform swap
            T tmp = list[a];
            list[a] = list[b];
            list[b] = tmp;
        }

        #endregion


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool All<T>(this IList<T> list, Predicate<T> query)
        {
            int count = list.Count;
            for (int i = 0; i < count; ++i)
                if (!query(list[i]))
                    return false;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool None<T>(this IList<T> list, Predicate<T> query)
        {
            int count = list.Count;
            for (int i = 0; i < count; ++i)
                if (query(list[i]))
                    return false;
            return true;
        }
    }
}
