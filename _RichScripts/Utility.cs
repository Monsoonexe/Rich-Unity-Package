using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using UnityEngine;
using DG.Tweening;

//clarifications between UnityEngine and System classes
using Debug = UnityEngine.Debug;
using Random = UnityEngine.Random;

/// <summary>
/// 
/// </summary>
/// <seealso cref="ConstStrings"/>
public static class Utility
{    //stuff we can all use!
    #region Boolean Operations
    
    //for situations where bools just don't cut it
    public const int TRUE_int = 1;
    public const int FALSE_int = 0;

    /// <summary>
    /// A && B in function form.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool And(this bool a, bool b) => a && b;

    public static bool Nand(this bool a, bool b) => !(a && b);

    public static bool Nor(this bool a, bool b) => !(a || b);

    public static bool Not(this bool a) => !a;

    public static bool Or(this bool a, bool b) => a || b;

    public static bool Xnor(this bool a, bool b) => !(a.Xor(b));

    /// <summary>
    /// One or the other, but not both. Returns true if a and b are different truth values.
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <returns></returns>
    public static bool Xor(this bool a, bool b) => (a && !b) || (!a && b);

    /// <summary>
    /// Shortcut for a = !a;
    /// </summary>
    /// <param name="a"></param>
    /// <returns></returns>
    public static bool Negate(this ref bool a) => a = !a;

    /// <summary>
    /// Shortcut for a = !a;
    /// </summary>
    /// <param name="a"></param>
    /// <returns></returns>
    public static bool Toggle(this ref bool a) => a = !a;

    #endregion

    //stuff we can all use!
    #region Communal Properties

    private static StringBuilder communityStringBuilder = new StringBuilder();

    /// <summary>
    /// Community String Builder (so you don't have to 'new' one).
    /// Just don't bet it will hold its data. Always safe to use right away.
    /// </summary>
    public static StringBuilder CommunityStringBuilder
    {
        get
        {
            communityStringBuilder.Clear(); // clear so it's safe to sue
            return communityStringBuilder;
        }
    }

    private static List<int> communityIndiceList = new List<int>(4);

    /// <summary>
    /// Community indice list (so you don't have to 'new' one).
    /// Just don't bet it will hold its data. Always safe to use right away.
    /// </summary>
    public static List<int> CommunityIndiceList
    {
        get
        {
            communityIndiceList.Clear(); // clear so it's safe to sue
            return communityIndiceList;
        }
    }

    public static readonly WaitForEndOfFrame WaitForEndOfFrame = new WaitForEndOfFrame();

    #endregion

    #region Collection Helpers

    public static void AddIfNew<T>(this List<T> list, T item)
    {
        if (!list.Contains(item))
            list.Add(item);
    }

    public static T Last<T>(this IList<T> collection) => collection[collection.Count - 1];

    /// <summary>
    /// Count - 1
    /// </summary>
    /// <param name="col"></param>
    /// <returns></returns>
    public static int LastIndex(this IList col) => col.Count - 1;

    /// <summary>
    /// Element at Count - 1.
    /// </summary>
    /// <param name="col"></param>
    /// <returns></returns>
    public static T Last<T>(this IList<T> col) => col[col.Count - 1];

    public static bool IndexIsInRange(this IList source, int index)
        => index >= 0 && index < source.Count;

    public static T[] CloneArray<T>(this T[] source)
    {
        var len = source.Length;
        var newArr = new T[len];
        for (var i = 0; i < len; ++i)
            newArr[i] = source[i];
        return newArr;
    }

    public static bool Contains<T>(this T[] array, T elem)
    {
        var length = array.Length;
        for (var i = 0; i < length; ++i) //TODO what if this is an array of ints? ints can't be null. wtf?
            if (array[i] != null && array[i].Equals(elem))
                return true;
        return false;
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

    /// <summary>
    /// Returns List with lowest Count from List of Lists.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="lists"></param>
    /// <returns></returns>
    /// <remarks>Ignores Lists with </remarks>
    public static List<T> GetShortestList<T>(IList<IList<T>> lists)
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
    /// Has an n probability of returning 'true'.
    /// </summary>
    /// <returns></returns>
    public static bool Chance(double n) => Random.Range(0, 1) <= n;

    /// <summary>
    /// Returns a random element from array, or default if collection is empty.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="collection"></param>
    /// <returns></returns>
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
    /// <typeparam name="T"></typeparam>
    /// <param name="totalCollection"></param>
    /// <param name="usedCollection"></param>
    /// <returns></returns>
    public static T GetRandomUnused<T>(IList<T> totalCollection, IList<T> usedCollection)
    {
        //build a pool of indices that have not been used.  
        var possibleIndices = CommunityIndiceList;

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
            Debug.Log("Every index has been used in collection of count: " + totalCollection.Count);
            return default;
        }

        return totalCollection[possibleIndices[Random.Range(0, possibleIndices.Count)]];
    }

    /// <summary>
    /// Removes and returns a random element of list. [0, Count)
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static T RemoveRandomElement<T>(this List<T> list)
    {
        var randomIndex = Random.Range(0, list.Count);
        var randomElement = list[randomIndex];
        list.RemoveAt(randomIndex);
        return randomElement;
    }

    /// <summary>
    /// Remove a random element in [start, end).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <param name="start"></param>
    /// <param name="end"></param>
    /// <returns></returns>
    public static T RemoveRandomElement<T>(this List<T> list, int start, int end)
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
    /// Removes item at highest index. Useful because the List won't shift each element (stack).
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="list"></param>
    /// <returns></returns>
    public static T GetRemoveLast<T>(this List<T> list)
        => list.GetRemoveAt(list.LastIndex());

    public static int RollDice(int dice, int sides, int mod = 0)
    {   //validate
        Debug.Assert(dice >= 0 && sides > 0, "[Utility] Invalid RollDice input: " 
            + dice.ToString() + " " + sides.ToString());

        var result = mod;
        var upperRandomLimit = sides + 1;//because upper bound of random is exclusive
        for (; dice > 0; --dice)
            result += Random.Range(1, upperRandomLimit);
        return result;
    }

    public static int RollDie(int sides) => Random.Range(1, sides + 1);

    #endregion

    #region DelayInvokation

    /// <summary>
    /// Must be called from StartCoroutine() on a Mono.
    /// </summary>
    /// <param name="delay"></param>
    /// <param name="callback"></param>
    /// <returns></returns>
    public static IEnumerator InvokeAfterDelay(this Action callback, float delay)
    {
        yield return new WaitForSeconds(delay);
        callback();
    }

    public static IEnumerator InvokeAfterDelay(this Action callback,
        YieldInstruction yieldInstruction)
    {
        yield return yieldInstruction;
        callback();
    }

    /// <summary>
    /// Version of InvokeAfterDelay that uses DOTween instead of coroutines
    /// </summary>
    /// <param name="callback"></param>
    /// <param name="delay"></param>
    /// <returns></returns>
    public static Sequence InvokeAfterDelaySequence(this Action callback, float delay)
    {
        return DOTween.Sequence() //new Sequence()
            .AppendInterval(delay)
            .AppendCallback(callback.Invoke);
    }

    public static IEnumerator InvokeAtEndOfFrame(this Action callback)
    {
        yield return WaitForEndOfFrame;
        callback();
    }

    public static IEnumerator InvokeNextFrame(this Action callback)
    {
        yield return null;
        callback();
    }

    /// <summary>
    /// Postpone execution until after so many frames have passed.
    /// </summary>
    /// <param name="callback"></param>
    /// <param name="frames"></param>
    /// <returns></returns>
    public static IEnumerator InvokeAfterFrames(this Action callback, int frames)
    {
        for(var i = 0; i < frames; ++i)
        {
            yield return null;
        }
        callback();
    }

    #endregion

    /// <summary>
    /// Set this and all children (and on) to given layer.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="newLayer"></param>
    public static void SetLayerRecursively(this Transform obj, int newLayer)
    {
        obj.gameObject.layer = newLayer;
        var childCount = obj.childCount;
        for (var i = 0; i < childCount; ++i)
        {
            var child = obj.GetChild(i);
            SetLayerRecursively(child, newLayer);
        }
    }

    /// <summary>
    /// Set this and all children (and on) to given layer.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="newLayer"></param>
    public static void SetLayerRecursively(this GameObject gameObj, int newLayer)
        => SetLayerRecursively(gameObj.transform, newLayer);

    /// <summary>
    /// Case insensitive check. True iff source == "true".
    /// </summary>
    /// <param name="source">Source string.</param>
    /// <param name="result">Result is invalid if 'false' is returned.</param>
    /// <returns>True if parse was successful. Result is invalid if 'false' is returned.</returns>
    public static bool TryStringToBool(this string source, out bool result)
    {
        var lowerSource = source.ToLower();
        var isTrue = lowerSource == ConstStrings.TRUE_STRING_LOWER;
        var isFalse = lowerSource == ConstStrings.FALSE_STRING_LOWER;
        var success = isTrue || isFalse;

        result = isTrue;

        return success;
    }

    #region Debug Assertions

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

    #endregion

    #region Functional Iterating

    /// <summary>
    /// Simply loops a given number of times
    /// </summary>
    /// <param name="cycles"></param>
    /// <param name="action"></param>
    public static void Repeat(Action action, int cycles)
    {
        for (int i = 0; i < cycles; ++i)
            action();
    }

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
        for (int i = list.Count - 1; i >= 0; ++i)
            action(list[i]);
    }
    #endregion

    /// <summary>
    /// Times how long the action took to complete and returns that time in milliseconds.
    /// </summary>
    public static long SpeedTest(Action action)
    {
        Stopwatch watch = Stopwatch.StartNew();
        action();
        watch.Stop();
        return watch.ElapsedMilliseconds;
    }
}
