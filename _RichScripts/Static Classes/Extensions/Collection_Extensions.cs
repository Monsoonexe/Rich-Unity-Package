//TODO - remove dependency on UnityEngine

using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RichPackage.Collections;
using RichPackage.Assertions;

//clarifications
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

namespace RichPackage
{
    /// <summary>
    /// My Collection Extension collection.
    /// </summary>
    public static class Collection_Extensions
    {
        #region Collection Helpers

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AddIfNew<T>(this List<T> list, T item)
        {
            bool isNew;
            if (isNew = !list.Contains(item))
                list.Add(item);
            return isNew;
        }

        /// <summary>
        /// Pop <paramref name="count"/> items off of <paramref name="src"/>
        /// and add them to <paramref name="dest"/>.
        /// </summary>
        /// <param name="src">List to remove items from.</param>
        /// <param name="dest">List to add items to.</param>
        /// <param name="count">Number of items to drain. &lt;0 implies 'drain all'.</param>
        /// <returns>Actual number of items added to <paramref name="dest"/>.</returns>
        public static int DrainInto<T>(this List<T> src, List<T> dest, int count)
        {
            //validate
            if (src == null)
                throw new ArgumentNullException(nameof(src));
            if (dest == null)
                throw new ArgumentNullException(nameof(dest));

            //flag to drain all
            if (count < 0 || count > src.Count)
                count = src.Count;

            //ensure capacity
            if (dest.Capacity < Math.Min(count, src.Count))
                dest.Capacity = count;
            int itemsAdded = count; //return value

            //work
            while (count-- > 0)
                dest.Add(src.GetRemoveLast());
            return itemsAdded;
        }

        /// <summary>
        /// Pop <paramref name="count"/> items off of <paramref name="src"/>
        /// and add them to <paramref name="dest"/>.
        /// </summary>
        /// <param name="src">List to remove items from.</param>
        /// <param name="dest">List to add items to.</param>
        /// <param name="count">Number of items to drain. &lt;0 implies 'drain all'.</param>
        /// <returns>Actual number of items added to <paramref name="dest"/>.</returns>
        public static int DrainInto<T>(this List<T> src, Queue<T> dest, int count)
        {
            //validate
            if (src == null)
                throw new ArgumentNullException(nameof(src));
            if (dest == null)
                throw new ArgumentNullException(nameof(dest));

            //flag to drain all
            if (count < 0 || count > src.Count)
                count = src.Count;
            int itemsAdded = count; //return value

            //work
            while (count-- > 0)
                dest.Enqueue(src.GetRemoveLast());
            return itemsAdded;
        }

        /// <summary>
        /// Pop <paramref name="count"/> items off of <paramref name="src"/>
        /// and add them to <paramref name="dest"/>.
        /// </summary>
        /// <param name="src">List to remove items from.</param>
        /// <param name="dest">List to add items to.</param>
        /// <param name="count">Number of items to drain. &lt;0 implies 'drain all'.</param>
        /// <returns>Actual number of items added to <paramref name="dest"/>.</returns>
        public static int DrainInto<T>(this List<T> src, Stack<T> dest, int count)
        {
            //validate
            if (src == null)
                throw new ArgumentNullException(nameof(src));
            if (dest == null)
                throw new ArgumentNullException(nameof(dest));

            //flag to drain all
            if (count < 0 || count > src.Count)
                count = src.Count;
            int itemsAdded = count; //return value

            //work
            while (count-- > 0)
                dest.Push(src.GetRemoveLast());
            return itemsAdded;
        }

        /// <summary>
        /// Returns 'true' if the dictionary contains the given key, which indicates the value was removed.
        /// Otherwise, returns false. Will not throw exception if key does not exist.
        /// </summary>
        public static bool Remove<TKey, UValue>(this Dictionary<TKey, UValue> dic, 
            TKey key, out UValue value)
        {
            var found = dic.TryGetValue(key, out value);
            if(found) dic.Remove(key);
            return found;
        }
        
        #region Average
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Average<T>(this IList<T> col, Func<T, int> summer)
            => col.Sum(summer) / col.Count;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static uint Average<T>(this IList<T> col, Func<T, uint> summer)
            => col.Sum(summer) / (uint) col.Count;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long Average<T>(this IList<T> col, Func<T, long> summer)
            => col.Sum(summer) / col.Count;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong Average<T>(this IList<T> col, Func<T, ulong> summer)
            => col.Sum(summer) / (ulong) col.Count;

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
            var length = array.Count;
            for (var i = 0; i < length; ++i)
                if (elem.CompareTo(array[i]) == 0)
                    return true;
            return false;
        }
        
        /// <summary>
        /// Returns 'true' if at least 1 item in array `elem.CompareTo(other) == 0`.
        /// </summary>
        public static bool Contains<T>(this IList<T> array, Searcher<T> searcher)
        {
            var length = array.Count;
            for (var i = 0; i < length; ++i)
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
            for(int i = 0; i < count; ++i)
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any<T>(this IList<T> list, Predicate<T> query)
        {
            int count = list.Count;
            for (int i = 0; i < count; ++i)
                if (query(list[i]))
                    return true;
            return false;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TrueForAll<T>(this IList<T> list, Predicate<T> query)
        {
            int count = list.Count;
            for (int i = 0; i < count; ++i)
                if (!query(list[i]))
                    return false;
            return true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool TrueForNone<T>(this IList<T> list, Predicate<T> query)
        {
            int count = list.Count;
            for (int i = 0; i < count; ++i)
                if (query(list[i]))
                    return false;
            return true;
        }
        
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
            var count = list.Count;
            for (var i = 0; i < count; ++i)
                if (query(list[i]))
                    return list[i];
            return default;
        }
        
        /// <summary>
        /// Fills a new <see cref="List{T}"/> with the results of <paramref name="query"/>.
        /// </summary>
        public static List<T> FindAll<T>(this IList<T> list, Predicate<T> query)
        {
            var count = list.Count;
            List<T> listToFill = new List<T>(count);
            for (var i = 0; i < count; ++i)
                if (query(list[i]))
                    listToFill.Add(list[i]);
            return listToFill;
        }
        
        /// <summary>
        /// Fills <paramref name="list"/> with the results of <paramref name="query"/>.
        /// </summary>
        public static void FindAll<T>(this IList<T> list, Predicate<T> query, List<T> listToFill)
        {
            var count = list.Count;
            for (var i = 0; i < count; ++i)
                if (query(list[i]))
                    listToFill.Add(list[i]);
        }
        
        /// <summary>
        /// Returns the last item in <paramref name="list"/> that <paramref name="query"/> returns true.
        /// </summary>
        public static T FindLast<T>(this IList<T> list, Predicate<T> query)
        {
            for (var i = list.Count - 1; i >=0; --i)
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
            for (var i = 0; i < count; ++i)
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
        
        public static bool TryFindAndRemove<T>(this List<T> list, 
            Predicate<T> query, out T foundItem)
        {
            foundItem = default;
            bool found = false;

            //iterate backwards to reduce left-shifts of elements after removal.
            for(int i = list.Count - 1; i >= 0; --i)
            {
                if(query(list[i]))
                {
                    found = true;
                    foundItem = list[i];
                    list.RemoveAt(i);
                    break;
                }
            }

            return found;
        }

        /// <summary>
        /// Remove and return the element at the given index.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="i"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetRemoveAt<T>(this List<T> list, int i)
        {
            //validate
            if (!list.IndexIsInRange(i))
                throw new IndexOutOfRangeException($"{i} | {list.Count}");

            //work
            T el = list[i];
            list.RemoveAt(i);
            return el;
        }

        /// <summary>
        /// Removes item at highest index. Useful because the List won't shift each 
        /// element (stack.Pop()).
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetRemoveLast<T>(this List<T> list)
            => list.GetRemoveAt(list.LastIndex());
            
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RemoveFirst<T>(this List<T> list)
            => list.RemoveAt(0);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RemoveLast<T>(this List<T> list)
            => list.RemoveAt(list.Count - 1);
        
        /// <summary>
        /// Remove each item and perform an action on it. O(n) time.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RemoveWhile<T>(this List<T> col, Action<T> action)
        {
            while(col.Count > 0)
            {   //iterate backwards to avoid shifting each element as you remove.
                var item = col.GetRemoveLast();
                action(item);
            }
        }

        /// <summary>
        /// Returns List with lowest Count from List of Lists.
        /// </summary>
        public static T GetShortestList<T>(this IList<T> lists)
            where T : IList
        {
            //first shortest path
            var shortestLength = int.MaxValue;
            var shortestIndex = 0;

            var count = lists.Count;
            for (var i = 0; i < count; ++i)
            {
                var workingCount = lists[i].Count;
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
        public static bool IndexIsInRange<T>(this List<T> col, int index)
            => index >= 0 && index < col.Count;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IndexIsInRange(this IList col, int index)
            => index >= 0 && index < col.Count;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullOrEmpty<T>(this IList<T> list)
            => list == null || list.Count == 0;
            
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEmpty<T>(this IList<T> list)
            => list.Count == 0;
            
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNotEmpty<T>(this IList<T> list)
            => list.Count != 0;

        /// <summary>
        /// A and B are the same size and every element in A is in B (order agnostic).
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns>True if A and B are the same size and every element in A is in B</returns>
        public static bool IsEquivalentTo<T>(this IList<T> a, IList<T> b)
        {
            if (a.Count != b.Count) return false;

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
            foreach (var item in a)
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
            int indexWrapped = (int)(index - count * System.Math.Floor(
                (double)index / count));
            return indexWrapped;
        }

        public static void InsertWrapped<T>(this List<T> list,
            int index, T item)
            => list.Insert(list.GetWrappedIndex(index), item);

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
        public static TSource FirstOrDefault<TSource>(this IEnumerable<TSource> source)
        {
            if (source == null)
                throw new System.ArgumentNullException(nameof(source));

            if (source is IList<TSource> list)
            {
                if (list.Count > 0)
                    return list[0];
            }
            else
            {
                using (IEnumerator<TSource> enumerator = source.GetEnumerator())
                {
                    if (enumerator.MoveNext())
                        return enumerator.Current;
                }
            }

            return default(TSource);
        }
        
        /// <summary>
        /// Element at position Count - 1.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T Last<T>(this IList<T> col) => col[col.Count - 1];
        
        /// <summary>
        /// Returns a 'new' list if <paramref name="list"/> is null or calls
        /// <see cref="List{T}.Clear"/> before returning it.
        /// </summary>
        /// <returns>A list where <see cref="List{T}.Count"/> is 0.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<T> NewOrClear<T>(this List<T> list)
        {
            if (list == null)
                list = new List<T>();
            else
                list.Clear();
            return list;
        }

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

            TReturn[] result = new TReturn[count];
            for (int i = 0; i < count; ++i)
                result[i] = expression(array[i + offset]);
            return result;
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
        public static void Swap<T>(this IList<T> list, int a, int b)
        {   //validate
            list.CastAs<IList>().IndexShouldBeInRange(a);
            list.CastAs<IList>().IndexShouldBeInRange(b);

            //perform swap
            T tmp = list[a];
            list[a] = list[b];
            list[b] = tmp;
        }

        /// <summary>
        /// Calls <see cref="List{T}.ToArray"/> and <see cref="List{T}.Clear"/> and returns the array.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] ToArrayAndClear<T>(this List<T> list)
        {
            T[] array = list.ToArray();
            list.Clear();
            return array;
        }

        #endregion

        #region Random and Collections
        
        /// <summary>
        /// Returns a random element from array, or default if collection is empty.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T GetRandomElement<T>(this IList<T> collection)
            => collection[Random.Range(0, collection.Count)];

        /// <summary>
        /// Get a random element from Collection that is not in usedCollection. 
        /// Up to caller to store this value in usedCollection
        /// </summary>
        public static T GetRandomUnused<T>(this IList<T> totalCollection, 
            IList<T> usedCollection)
        {
            //build a pool of indices that have not been used.  
            var possibleIndices = CommunalLists.Rent<int>();

            int totalCount = totalCollection.Count;
            for (int i = 0; i < totalCount; ++i)
                if (!usedCollection.Contains(totalCollection[i]))
                    possibleIndices.Add(i);//this index is safe to choose from

            if (possibleIndices.Count == 0)
            {
                Debug.Log("Every index has been used in collection of count: "
                    + totalCollection.Count);
                return default;
            }

            return totalCollection[possibleIndices[
                Random.Range(0, possibleIndices.Count)]];
        }
        
        /// <summary>
        /// Removes and returns a random element of list. [0, Count)
        /// </summary>
        public static T GetRemoveRandomElement<T>(this List<T> list)
        {
            var randomIndex = Random.Range(0, list.Count);
            T randomElement = list[randomIndex];
            list.RemoveAt(randomIndex);
            return randomElement;
        }

        /// <summary>
        /// Remove a random element in [start, end).
        /// </summary>
        public static T GetRemoveRandomElement<T>(this List<T> list, 
            int start, int end)
        {
            var count = list.Count;
            //validate
            start = (start < 0 || start >= count) ? 0 : start; //start [0, count - 1]
            end = (end < 1 || end > count) ? count : end;//end [1, count]

            //compute
            var randomIndex = Random.Range(start, end);
            T randomElement = list[randomIndex];
            list.RemoveAt(randomIndex);
            return randomElement;
        }

        /// <summary>
        /// Shuffle elements in the collection.
        /// </summary>
        public static void Shuffle<T>(this IList<T> list)
        {
            var count = list.Count; //cache for less function overhead on every iteration
            for (var i = 0; i < count; ++i)
            {
                var randomIndex = Random.Range(0, count);
                list.Swap(i, randomIndex);
            }
        }

        /// <summary>
        /// Shuffle elements in the collection n times.
        /// 7 has been proven to be a good amount.
        /// </summary>
        public static void Shuffle<T>(this IList<T> list, int repeat)
        {
            for (var i = 0; i < repeat; ++i)
                list.Shuffle();
        }

        #endregion
    }
}
