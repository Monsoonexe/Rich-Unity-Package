//source that this is faster concatenate method https://stackoverflow.com/questions/1547252/how-do-i-concatenate-two-arrays-in-c#:~:text=The%20result%20was%3A-,Roll%20your%20own%20wins.,-Share

using System;
using System.Runtime.CompilerServices;

namespace RichPackage
{
	public static class ArrayExtensions
	{
		/// <summary>
		/// Sets first null item to 'newItem'. O(n) time b.c doesn't cache index.
		/// </summary>
		/// <typeparam name="T">Must be class.</typeparam>
		public static void Add<T>(this T[] array, T newItem)
			where T : class
		{
			var count = array.Length;
			for (var i = 0; i < count; ++i)
			{
				if (array[i] == null)
				{
					array[i] = newItem;
					return;
				}
			}
		}

		/// <summary>
		/// Returns a new array with elements of both.
		/// </summary>
		public static T[] ConcatArray<T>(this T[] a, T[] b)
		{
			int aLen = a.Length;
			int bLen = b.Length;
			T[] newArr = new T[aLen + bLen]; //return value

			//copy first array
			for (int i = 0; i < aLen; ++i)
				newArr[i] = a[i];

			//copy second array
			for (int i = 0; i < bLen; ++i)
				newArr[i + aLen] = b[i];

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
			var len = arr.Length;
			var smallerSize = newSize < len ? newSize : len;
			for (var i = 0; i < smallerSize; ++i)
				newArr[i] = arr[i];
			return newArr;
		}

		/// <summary>
		/// Returns the first index matching the given element, or -1 if not found.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int IndexOf<T>(this T[] array, T element)
			=> Array.IndexOf(array, element);
	}
}
