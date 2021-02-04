using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Random = UnityEngine.Random;

namespace Explore
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="ConstStrings"/>
    public static class Utility
    {    //stuff we can all use!
        #region Boolean Operations

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
	    public static bool Xor(this bool a, bool b)  => (a && !b) || (!a && b);

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
	        for (var i = 0; i < length; ++i)
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
	    public static int GetIndexOfElementInArray<T>(T element, T[] array)
	    {
	        var length = array.Length;
	        for(var i = 0; i < length; ++i)
	            if (array[i].Equals(element))
	                return i;
	        return -1;//not found
	    }

	    /// <summary>
	    /// Returns List with lowest Count from List of Lists.
	    /// </summary>
	    /// <typeparam name="T"></typeparam>
	    /// <param name="lists"></param>
	    /// <returns></returns>
	    /// <remarks>Ignores Lists with </remarks>
	    public static List<T> GetShortestList<T>(List<List<T>> lists)
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
	    public static bool IsEquivalentTo<T>(this List<T> a, List<T> b)
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
	    public static bool IsSubsetOf<T>(this List<T> a, List<T> b)
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

	    #endregion

	    #region Random and Collections

	    /// <summary>
	    /// Returns a random element from array, or default if collection is empty.
	    /// </summary>
	    /// <typeparam name="T"></typeparam>
	    /// <param name="collection"></param>
	    /// <returns></returns>
	    public static T GetRandomElement<T>(T[] collection)
	    {
	        var length = collection.Length;
	        if (length == 0) return default; // no elements!
	        return collection[Random.Range(0, length)];
	    }

	    /// <summary>
	    /// Returns a random element from array, or default if collection is empty.
	    /// </summary>
	    /// <typeparam name="T"></typeparam>
	    /// <param name="collection"></param>
	    /// <returns></returns>
	    public static T GetRandomElement<T>(List<T> collection)
	    {
	        var length = collection.Count;
	        if (length == 0) return default; // no elements!
	        return collection[Random.Range(0, length)];
	    }

	    /// <summary>
	    /// GetPrint a random element from Collection that is not in usedCollection. 
	    /// Up to caller to store this value in usedCollection
	    /// </summary>
	    /// <typeparam name="T"></typeparam>
	    /// <param name="totalCollection"></param>
	    /// <param name="usedCollection"></param>
	    /// <returns></returns>
	    public static T GetRandomUnused<T>(T[] totalCollection, 
	        T[] usedCollection)
	    {            
	        //build a pool of indices that have not been used.  
	        var possibleIndices = CommunityIndiceList;

	        var totalCount = totalCollection.Length;
	        for (var i = 0; i < totalCount; ++i)
	        {
	            if(!Contains(usedCollection, totalCollection[i])) 
	            {
	                possibleIndices.Add(i);//this index is safe to choose from
	            }
	        }

	        if(possibleIndices.Count == 0)
	        {
	            Debug.Log("Every index has been used in collection of count: " + totalCollection.Length);
	            return default;
	        }

	        return totalCollection[possibleIndices[Random.Range(0, possibleIndices.Count)]]; 
	    }

	    /// <summary>
	    /// GetPrint a random element from Collection that is not in usedCollection. 
	    /// Up to caller to store this value in usedCollection
	    /// </summary>
	    /// <typeparam name="T"></typeparam>
	    /// <param name="totalCollection"></param>
	    /// <param name="usedCollection"></param>
	    /// <returns></returns>
	    public static T GetRandomUnused<T>(List<T> totalCollection,
	        T[] usedCollection)
	    {
	        //build a pool of indices that have not been used.  
	        var possibleIndices = CommunityIndiceList;

	        var totalCount = totalCollection.Count;
	        for (var i = 0; i < totalCount; ++i)
	        {
	            if (!Contains(usedCollection, totalCollection[i]))
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
	    /// GetPrint a random element from Collection that is not in usedCollection. 
	    /// Up to caller to store this value in usedCollection
	    /// </summary>
	    /// <typeparam name="T"></typeparam>
	    /// <param name="totalCollection"></param>
	    /// <param name="usedCollection"></param>
	    /// <returns></returns>
	    public static T GetRandomUnused<T>(IList totalCollection, List<T> usedCollection)
	    {
	        //build a pool of indices that have not been used.  
	        var possibleIndices = CommunityIndiceList;

	        var totalCount = totalCollection.Count;
	        for (var i = 0; i < totalCount; ++i)
	        {
	            if (!usedCollection.Contains((T) totalCollection[i]))
	            {
	                possibleIndices.Add(i);//this index is safe to choose from
	            }
	        }

	        if (possibleIndices.Count == 0)
	        {
	            Debug.Log("Every index has been used in collection of count: " + totalCollection.Count);
	            return default;
	        }

	        return (T)totalCollection[possibleIndices[Random.Range(0, possibleIndices.Count)]];
	    }

	    /// <summary>
	    /// GetPrint a random element from Collection that is not in usedCollection. 
	    /// Up to caller to store this value in usedCollection
	    /// </summary>
	    /// <typeparam name="T"></typeparam>
	    /// <param name="totalCollection"></param>
	    /// <param name="usedCollection"></param>
	    /// <returns></returns>
	    public static T GetRandomUnused<T>(List<T> totalCollection, List<T> usedCollection)
	    {
	        //build a pool of indices that have not been used.  
	        var possibleIndices = CommunityIndiceList;

	        var totalCount = totalCollection.Count;
	        for (var i = 0; i < totalCount; ++i)
	        {
	            if(!usedCollection.Contains(totalCollection[i]))
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
	        callback.Invoke();
	    }

	    public static IEnumerator InvokeAfterDelay(this Action callback,
	        YieldInstruction yieldInstruction)
	    {
	        yield return yieldInstruction;
	        callback.Invoke();
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

        #endregion

        public static void SetLayerRecursively(this Transform obj, int newLayer )
        {
            obj.gameObject.layer = newLayer;
            var childCount = obj.childCount;
            for (var i = 0; i < childCount; ++i)
            {
                var child = obj.GetChild(i);
                SetLayerRecursively(child, newLayer);
            }
        }

        public static void SetLayerRecursively(this GameObject gameObj, int newLayer)
        {
            SetLayerRecursively(gameObj.transform, newLayer);
        }

        #region Math

        public static int AbsoluteValue(this int i)
            => i >= 0 ? i : -i;

        /// <summary>
        /// Runs ~twice as fast as Mathf.Abs().
        /// </summary>
        /// <param name="f"></param>
        /// <returns>Because Mathf.Abs() is managed code.</returns>
        public static float AbsoluteValue(this float f)
            => f >= 0 ? f : -f;

        /// <summary>
        /// f = |f|
        /// </summary>
        /// <param name="f"></param>
        /// <returns></returns>
        public static void SetAbsolute(ref this float f)
            => f = f >= 0 ? f : -f;

        public static Vector2 AbsoluteValue(this Vector2 v)
            => new Vector2(AbsoluteValue(v.x), AbsoluteValue(v.y));

        public static Vector3 AbsoluteValue(this Vector3 v)
            => new Vector3(AbsoluteValue(v.x), AbsoluteValue(v.y), AbsoluteValue(v.z));

        #endregion

    }
}
