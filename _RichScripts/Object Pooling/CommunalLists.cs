using System;
using System.Collections;
using System.Collections.Generic;

namespace RichPackage
{
	/// <summary>
	/// A collection of recycled Lists. Created as needed. 
	/// Better than littering a bunch of temporary Lists.
	/// Assume 
	/// </summary>
	/// <example>var newList = CommunalLists.Get{int}();</example>
	public static class CommunalLists
	{
		private static readonly ListHub hub = new ListHub();

		/// <summary>
		/// Replaces 'new List{T}'. Will be Clear() and ready to use.
		/// Expect stuff to break if you call another function that uses this
		/// or if you pass it around as a parameter.
		/// </summary>
		/// <returns>Ready-to-use List{T}</returns>
		public static List<T> Rent<T>() => hub.Rent<T>();

		public static void Return<T>(List<T> list) => hub.Return(list);

		public static void Remove<T>() => hub.Remove<T>();

		#region Constructors

		static CommunalLists()
		{
			//add defaults here
			//hub.GetList<int>();
			//hub.GetList<float>();
			//hub.GetList<string>();
		}

		#endregion Constructors
	}

	/// <summary>
	/// A collection of recycled Lists. Created as needed. 
	/// Better than littering a bunch of temporary Lists.
	/// </summary>
	/// <example>var newList = CommunityLists.Get{int}();</example>
	public class ListHub
	{
		public int MaxListSize = 1024;
		public int startingSize = 8;
		public int ListCount => listTable.Count;

		private readonly Dictionary<Type, IList> listTable
			= new Dictionary<Type, IList>();

		/// <summary>
		/// Replaces 'new List{T}'. Will be Clear() and ready to use.
		/// Expect stuff to break if you call another function that uses this
		/// or if you pass it around as a parameter.
		/// </summary>
		/// <returns>Ready-to-use <see cref="List{T}"/>.</returns>
		public List<T> Rent<T>()
		{
			//get from dictionary, if already exists
			if (listTable.TryGetValue(typeof(T), out IList list))
			{
				list.Clear(); //reset list
			}
			else //create and add
			{
				list = new List<T>(startingSize);
				listTable.Add(typeof(T), list);
			}

			return list.CastAs<List<T>>();
		}

		/// <summary>
		/// Returns the rented array to the pool.
		/// </summary>
		public void Return<T>(List<T> list)
		{
			list.Clear();
			if (!listTable.ContainsKey(typeof(T)))
			{
				listTable.Add(typeof(T), list);
				if (list.Count > MaxListSize)
					list.Capacity = MaxListSize;
			}
		}

		public void Remove<T>() => listTable.Remove(typeof(T));
	}
}
