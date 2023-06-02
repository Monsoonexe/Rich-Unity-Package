//source that this is faster concatenate method https://stackoverflow.com/questions/1547252/how-do-i-concatenate-two-arrays-in-c#:~:text=The%20result%20was%3A-,Roll%20your%20own%20wins.,-Share

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace RichPackage
{
    public static class ArrayExtensions
    {
        /// <summary>
        /// Sets first <see langword="null"/> slot to 
        /// <paramref name="newItem"/>.
        /// </summary>
        /// <typeparam name="T">Must be a reference type.</typeparam>
        /// <remarks> O(n) time b.c doesn't cache index.</remarks>
        public static void Add<T>(this T[] array, T newItem)
            where T : class
        {
            int count = array.Length;
            for (int i = 0; i < count; ++i)
            {
                if (array[i] == null)
                {
                    array[i] = newItem;
                    return;
                }
            }
        }

        /// <summary>
        /// Returns a new array which contains all elements of
		/// <paramref name="a"/> and <paramref name="b"/>.
        /// </summary>
        public static T[] Concat<T>(this T[] a, T[] b)
        {
            int aLen = a.Length;
            int bLen = b.Length;
            var newArr = new T[aLen + bLen]; //return value

            // copy first array
            for (int i = 0; i < aLen; ++i)
                newArr[i] = a[i];

            // copy second array
            for (int i = 0; i < bLen; ++i)
                newArr[i + aLen] = b[i];

            return newArr;
        }

        /// <summary>
        /// Returns a new array which contains all elements of
        /// <paramref name="a"/>, <paramref name="b"/>, and <paramref name="c"/>.
        /// </summary>
        public static T[] Concat<T>(this T[] a, T[] b, T[] c)
        {
            int aLen = a.Length;
            int bLen = b.Length;
            int cLen = c.Length;

            var newArr = new T[aLen + bLen + cLen]; //return value

            // copy first array
            for (int i = 0; i < aLen; ++i)
                newArr[i] = a[i];

            // copy second array
            for (int i = 0; i < bLen; ++i)
                newArr[i + aLen] = b[i];

            // copy third array
            for (int i = 0; i < cLen; ++i)
                newArr[i + bLen] = b[i];

            return newArr;
        }

        /// <returns>
        /// <see langword="true"/> if <paramref name="item"/> is found in
        /// <paramref name="array"/>; otherwise <see langword="false"/>.
        /// </returns>
        /// <seealso cref="List{T}.Contains(T)"/>
        public static bool Contains<T>(this T[] array, T item)
        {
            int _size = array.Length;
            if (item == null)
            {
                for (int i = 0; i < _size; i++)
                    if (array[i] == null)
                        return true;

                return false;
            }

            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            for (int j = 0; j < _size; j++)
                if (comparer.Equals(array[j], item))
                    return true;

            return false;
        }

        /// <returns>
        /// <see langword="false"/> if <paramref name="item"/> is found in
        /// <paramref name="array"/>; otherwise <see langword="true"/>.
        /// </returns>
        /// <seealso cref="List{T}.Contains(T)"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool DoesNotContain<T>(this T[] array, T query)
            => !Contains(array, query);

        /// <summary>
        /// Returns a new array of given size with as many elements (or fewer) copied over.
        /// </summary>
        public static T[] GetResized<T>(this T[] arr, int newSize)
        {
            var newArr = new T[newSize];
            int len = arr.Length;
            int smallerSize = newSize < len ? newSize : len;
            for (int i = 0; i < smallerSize; ++i)
                newArr[i] = arr[i];
            return newArr;
        }

        /// <summary>
        /// Returns the first index matching the given element, or -1 if not found.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int IndexOf<T>(this T[] array, T element)
            => Array.IndexOf(array, element);

        /// <summary>Gets a sub array of an existing array.</summary>
        /// <param name="index">Index to start taking the items from.</param>
        /// <param name="length">The length of the sub array.</param>
        public static T[] SubArray<T>(this T[] data, int index, int length)
        {
            var result = new T[length];
            Array.Copy(data, index, result, 0, length);
            return result;
        }
	    
		/// <returns>The index of the first item that matches against <paramref name="query"/> or -1 if none found.</returns>
		public static int IndexOf<T>(this T[] array, Predicate<T> query)
		{
			int len = array.Length;
			for (int i = 0; i < len; ++i)
			if (query(array[i]))
				return i;
			return -1;
			}
		}

    public static class ArrayUtils
    {
        /// <summary>
        /// Concatenates all arrays in <paramref name="arrays"/>.
        /// </summary>
        public static T[] Concat<T>(params T[][] arrays)
        {
            // calculate total length of new array
            int totalLength = 0;
            for (int i = 0; i < arrays.Length; i++)
                totalLength += arrays[i].Length;

            var result = new T[totalLength]; // return value
            int offset = 0;
            for (int i = 0; i < arrays.Length; i++)
            {
                T[] arr = arrays[i];
                Array.Copy(arr, 0, result, offset, arr.Length);
                offset += arr.Length;
            }

            return result;
        }
        
        public static T[] Concat<T>(params IList<T>[] arrays)
        {
            // calculate total length of new array
            int totalLength = 0;
            for (int i = 0; i < arrays.Length; i++)
                totalLength += arrays[i].Count;

            var result = new T[totalLength]; // return value
            int offset = 0;
            for (int i = 0; i < arrays.Length; i++)
            {
                IList<T> arr = arrays[i];
                arr.CopyTo(result, offset);
                offset += arr.Count;
            }

            return result;
        }
    }
}
