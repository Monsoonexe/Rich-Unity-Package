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

}
