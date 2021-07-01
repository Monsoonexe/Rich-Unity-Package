using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

//clarifications
using Random = UnityEngine.Random;
using Debug = UnityEngine.Debug;

/// <summary>
/// 
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

    public static void AddIfNew<T>(this List<T> list, T item)
    {
        if (!list.Contains(item))
            list.Add(item);
    }

    [Conditional("UNITY_EDITOR")]//editor only
    public static void AssertValidIndex<T>(
        this IList<T> collection, int index)
        => Debug.AssertFormat(index >= 0 && index < collection.Count,
            "[SetAnimatorPropertiesHelper] Index out of bounds [{0} | {1}].",//report index
            index, collection.Count);

    [Conditional("UNITY_EDITOR")]//editor only
    public static void AssertValidIndex<T>(
        this IList<T> collection, int index, string name)
        => Debug.AssertFormat(index >= 0 && index < collection.Count,
            "[SetAnimatorPropertiesHelper] Index out of bounds [{0} | {1}] " + //report index
            "on: {2}", //report name of problem mono
            index, collection.Count, name);

    /// <summary>
    /// Returns 'true' if at least 1 item in array Equals() given item.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="array"></param>
    /// <param name="elem"></param>
    /// <returns></returns>
    public static bool Contains<T>(this T[] array, T elem)
    {
        var length = array.Length;
        for (var i = 0; i < length; ++i) //TODO what if this is an array of ints? ints can't be null. wtf?
            if (array[i] != null && array[i].Equals(elem))
                return true;
        return false;
    }

    /// <summary>
    /// Returns a random element from array, or default if collection is empty.
    /// </summary>
    public static T GetRandomElement<T>(this IList<T> collection)
    {
        var length = collection.Count;
        if (length == 0) return default; // no elements!
        return collection[Random.Range(0, length)];
    }

    /// <summary>
    /// Get a random element from Collection that is not in usedCollection. 
    /// Up to caller to store this value in usedCollection
    /// </summary>
    public static T GetRandomUnused<T>(this IList<T> totalCollection, 
        IList<T> usedCollection)
    {
        //build a pool of indices that have not been used.  
        var possibleIndices = CommunalLists.Get<int>();

        var totalCount = totalCollection.Count;
        for (var i = 0; i < totalCount; ++i)
        {
            if (!usedCollection.Contains(totalCollection[i]))
            {
                possibleIndices.Add(i);//this index is safe to choose from
            }
        }

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
    /// Remove and return the element at the given index.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="i"></param>
    /// <returns></returns>
    public static T GetRemoveAt<T>(this List<T> list, int i)
    {
        list.AssertValidIndex(i);
        var el = list[i];
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
    public static T GetRemoveLast<T>(this List<T> list)
        => list.GetRemoveAt(list.LastIndex());

    /// <summary>
    /// Removes and returns a random element of list. [0, Count)
    /// </summary>
    public static T GetRemoveRandomElement<T>(this List<T> list)
    {
        var randomIndex = Random.Range(0, list.Count);
        var randomElement = list[randomIndex];
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
        var randomElement = list[randomIndex];
        list.RemoveAt(randomIndex);
        return randomElement;
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
    /// <remarks></remarks>
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
            if (workingCount > 0 && // must have at least one waypoint
                    workingCount < shortestLength)
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
    /// <typeparam name="T"></typeparam>
    /// <param name="element"></param>
    /// <param name="array"></param>
    /// <returns></returns>
    public static int IndexOf<T>(this T[] array, T element)
        => Array.IndexOf(array, element);

    public static bool IndexIsInRange(this IList col, int index)
        => index >= 0 && index < col.Count;

    /// <summary>
    /// A & B are the same size and every element in A is in B (order agnostic).
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns>True if A & B are the same size and every element in A is in B</returns>
    public static bool IsEquivalentTo<T>(this IList<T> a, IList<T> b)
    {
        if (a.Count != b.Count) return false;

        foreach (var item in a)
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

    /// <summary>
    /// Element at Count - 1.
    /// </summary>
    /// <param name="col"></param>
    /// <returns></returns>
    public static T Last<T>(this IList<T> col) => col[col.Count - 1];

    /// <summary>
    /// Count - 1
    /// </summary>
    /// <param name="col"></param>
    /// <returns></returns>
    public static int LastIndex(this IList col) => col.Count - 1;

    /// <summary>
    /// Remove each item and perform an action on it. O(n) time.
    /// </summary>
    public static void RemoveWhile<T>(this List<T> col, 
        Action<T> action)
    {
        while(col.Count > 0)
        {   //iterate backwards to avoid shifting each element as you remove.
            var item = col.GetRemoveLast();
            action(item);
        }
    }

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
    /// Shuffle elements in the collection.
    /// </summary>
    public static void Shuffle<T>(this IList<T> list)
    {
        var count = list.Count;
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

    #region Functional 

    /// <summary>
    /// A 'foreach' with a 'for' backbone
    /// </summary>
    public static void ForEach<T>(this IList<T> list, Action<T> action)
    {
        for (int i = 0; i < list.Count; ++i)
            action(list[i]);
    }

    /// <summary>
    /// A 'foreach' with a 'for' backbone
    /// </summary>
    public static void ForEachBackwards<T>(this IList<T> list, Action<T> action)
    {
        for (int i = list.Count - 1; i >= 0; --i)
            action(list[i]);
    }

    #endregion
}
