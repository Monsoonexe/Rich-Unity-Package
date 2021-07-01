using System;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// A collection of recycled Lists. Created as needed. 
/// Better than littering a bunch of temporary Lists.
/// Assume 
/// </summary>
/// <example>var newList = CommunalLists.Get{int}();</example>
public static class CommunalLists
{
    private static readonly ListHub hub 
        = new ListHub();

    /// <summary>
    /// Replaces 'new List{T}'. Will be Clear() and ready to use.
    /// Expect stuff to break if you call another function that uses this
    /// or if you pass it around as a parameter.
    /// </summary>
    /// <returns>Ready-to-use List{T}</returns>
    public static List<T> Get<T>()
        => hub.GetList<T>();
	
	#region Constructors
	
	static CommunalLists()
    {
        //add defaults here
        //hub.GetList<int>();
    }
	
	#endregion
}

/// <summary>
/// A collection of recycled Lists. Created as needed. 
/// Better than littering a bunch of temporary Lists.
/// Assume 
/// </summary>
/// <example>var newList = CommunityLists.Get{int}();</example>
public class ListHub
{
    public int startingSize = 6;
    private readonly Dictionary<Type, IList> listTable
        = new Dictionary<Type, IList>();

    /// <summary>
    /// Replaces 'new List{T}'. Will be Clear() and ready to use.
    /// Expect stuff to break if you call another function that uses this
    /// or if you pass it around as a parameter.
    /// </summary>
    /// <returns>Ready-to-use List{T}</returns>
    public List<T> GetList<T>()
    {
        IList list = null;

        //get from dictionary, if already exists
        if (listTable.TryGetValue(typeof(T), out list))
        {
            list.Clear();
        }
        else //create and add
        {
            list = new List<T>(startingSize);
            listTable.Add(typeof(T), list);
        }

        return (List<T>)list;
    }
}
