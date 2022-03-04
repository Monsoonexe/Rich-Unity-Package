using System.Collections;
using System.Collections.Generic;

namespace RichPackage
{
	/// <summary>
	/// 
	/// </summary>
	public class ListCountComparer<T> : IComparer<T> where T : IList
	{
		public int Compare(T x, T y) => x.Count.CompareTo(y.Count);

		public static readonly ListCountComparer<T> Default = new ListCountComparer<T>();
	}

	/// <summary>
	/// 
	/// </summary>
	public class ListCountReverseComparer<T> : IComparer<T> where T : IList
	{
		public int Compare(T x, T y) => y.Count.CompareTo(x.Count);

		public static readonly ListCountReverseComparer<T> Default = new ListCountReverseComparer<T>();
	}

	public class ArrayLengthComparer<T> : IComparer<T[]>
	{
		public int Compare(T[] x, T[] y) => x.Length.CompareTo(y.Length);

		public static readonly ArrayLengthComparer<T> Default = new ArrayLengthComparer<T>();
	}

	public class ArrayLengthReverseComparer<T> : IComparer<T[]>
	{
		public int Compare(T[] x, T[] y) => y.Length.CompareTo(x.Length);

		public static readonly ArrayLengthReverseComparer<T> Default = new ArrayLengthReverseComparer<T>();
	}

}
