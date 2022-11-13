using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Random = UnityEngine.Random;

namespace RichPackage
{
    public static class ListExtensions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool AddIfNew<T>(this List<T> list, T item)
        {
            bool isNew;
            if (isNew = !list.Contains(item))
                list.Add(item);
            return isNew;
        }

        public static void AddRange<T>(this List<T> source, IList<T> other)
        {
            int len = other.Count;
            for (int i = 0; i < len; i++)
                source.Add(other[i]);
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InsertWrapped<T>(this List<T> list,
            int index, T item)
            => list.Insert(list.GetWrappedIndex(index), item);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Any<T>(this IList<T> list, Predicate<T> query)
        {
            int count = list.Count;
            for (int i = 0; i < count; ++i)
                if (query(list[i]))
                    return true;
            return false;
        }

        public static bool TryFindAndRemove<T>(this List<T> list,
            Predicate<T> query, out T foundItem)
        {
            foundItem = default;
            bool found = false;

            //iterate backwards to reduce left-shifts of elements after removal.
            for (int i = list.Count - 1; i >= 0; --i)
            {
                if (query(list[i]))
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
            // validate
            if (!list.IsIndexInRange(i))
                throw new IndexOutOfRangeException($"{i} | {list.Count}");

            // work
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
        public static void RemoveWhile<T>(this List<T> col, Action<T> action)
        {
            while (col.Count > 0)
            {   //iterate backwards to avoid shifting each element as you remove.
                var item = col.GetRemoveLast();
                action(item);
            }
        }

        /// <summary>
        /// Remove the first instance of an item in <paramref name="list"/> that matches
        /// <paramref name="predicate"/>.
        /// </summary>
        /// <seealso cref="TryFindAndRemove{T}(List{T}, Predicate{T}, out T)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void RemoveFirst<T>(this List<T> list, Predicate<T> predicate)
        {
            TryFindAndRemove(list, predicate, out _);
        }

        /// <summary>
        /// Remove all items in <paramref name="list"/> that match <paramref name="predicate"/>.
        /// </summary>
        public static void RemoveAll<T>(this List<T> list, Predicate<T> predicate)
        {
            for (int i = list.Count - 1; i >= 0; --i)
            {
                if (predicate(list[i]))
                    list.RemoveAt(i);
            }
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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsEmpty<T>(this List<T> list)
            => list.Count == 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsIndexInRange<T>(this List<T> col, int index)
            => index >= 0 && index < col.Count;

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
        /// A call to <see cref="List.Clear"/> that can be chained.
        /// </summary>
        /// <returns><paramref name="list"/></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static List<T> FluentClear<T>(this List<T> list)
        {
            list.Clear();
            return list;
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
    }

}
