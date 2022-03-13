using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using RichPackage.Collections;
using UnityEngine;

//clarifications
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

/// <summary>
/// My Collection Extension collection.
/// </summary>
public static class Collection_Extensions
{
    #region Collection Helpers
    
    /// <summary>
    /// Sets first null item to 'newItem'. O(n) time b.c doesn't cache index.
    /// </summary>
    /// <typeparam name="T">Must be class.</typeparam>
    public static void Add<T>(this T[] array, T newItem)
        where T : class
    {
        var count = array.Length;
        for(var i = 0; i < count; ++i)
        {
            if (array[i] == null)
            {
                array[i] = newItem;
                return;
            }
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool AddIfNew<T>(this List<T> list, T item)
    {
        bool isNew;
        if (isNew = !list.Contains(item))
            list.Add(item);
        return isNew;
    }

    [Conditional("UNITY_EDITOR")]//editor only
    public static void AssertValidIndex<T>(
        this IList<T> collection, int index)
        => Debug.AssertFormat(index >= 0 && index < collection.Count,
            "Index out of bounds [{0} | {1}].",//report index
            index, collection.Count);

    [Conditional("UNITY_EDITOR")]//editor only
    public static void AssertValidIndex<T>(
        this IList<T> collection, int index, string name)
        => Debug.AssertFormat(index >= 0 && index < collection.Count,
            "Index out of bounds [{0} | {1}] " + //report index
            "on: {2}", //report name of problem mono
            index, collection.Count, name);

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
    /// Returns 'true' if at least 1 item in array `Equals()` given item.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="elem"></param>
    /// <returns></returns>
    public static bool Contains<T>(this T[] array, T elem)
    {
        var length = array.Length;
        for (var i = 0; i < length; ++i)
            if (elem.Equals(array[i]))
                return true;
        return false;
    }
    
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

    public static bool TrueForAll<T>(this IList<T> list, Predicate<T> query)
    {
        bool contains = true; //return value
        int count = list.Count;
        for (int i = 0; i < count; ++i)
        {
            if (!query(list[i]))
            {
                contains = false;
                break;
            }
        }
        return contains;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool TrueForNone<T>(this IList<T> list, Predicate<T> query)
        => !TrueForAll(list, query);
    
    /// <summary>
    /// Returns 'true' if all elements of each <see cref="IList{T}"/> are equivalent, otherwise returns 'false'.
    /// </summary>
    public static bool SequenceEqual<T>(this IList<T> a, IList<T> b)
    {
        int aCount = a.Count; //cache to reduce function overhead
        int bCount = b.Count;
        bool equivalent = true; //return value
        if (aCount == bCount)
            return false;
        for (int i = 0; i < aCount; i++)
        {
            if (!a[i].Equals(b[i]))
            {
                equivalent = false;
                break;
            }
        }
        return equivalent;
    }
    
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
        list.AssertValidIndex(i);
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
    /// Returns a new array of given size with as many elements (or fewer) copied over.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="arr"></param>
    /// <param name="newSize"></param>
    public static T[] GetResized<T>(this T[] arr, int newSize)
    {
        var newArr = new T[newSize];
        var len = arr.Length;
        var smallerSize = newSize < len ? newSize : len;
        for (var i = 0; i < smallerSize; ++i)
            newArr[i] = arr[i];
        return newArr;
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


    /// <summary>
    /// Returns the first index matching the given element, or -1 if not found.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static int IndexOf<T>(this T[] array, T element)
        => Array.IndexOf(array, element);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static bool IndexIsInRange(this IList col, int index)
        => index >= 0 && index < col.Count;

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
    /// Element at position Count - 1.
    /// </summary>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static T Last<T>(this IList<T> col) => col[col.Count - 1];

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
        list.AssertValidIndex(a);
        list.AssertValidIndex(b);

        //perform swap
        T tmp = list[a];
        list[a] = list[b];
        list[b] = tmp;
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
        var possibleIndices = CommunalLists.Get<int>();

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
