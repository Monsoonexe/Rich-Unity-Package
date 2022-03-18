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
            //hub.GetList<float>();
            //hub.GetList<string>();
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
        public int startingSize = 8;
        private readonly Dictionary<Type, IList> listTable
            = new Dictionary<Type, IList>();

        public ListHub(params Type[] types)
        {
            foreach (Type type in types)
                GetList(type); //create new list
        }

        /// <summary>
        /// Replaces 'new List{T}'. Will be Clear() and ready to use.
        /// Expect stuff to break if you call another function that uses this
        /// or if you pass it around as a parameter.
        /// </summary>
        /// <returns>Ready-to-use <see cref="List{T}"/>.</returns>
        public List<T> Get<T>()
        {
            IList<T> list;

            //get from dictionary, if already exists
            if (listTable.TryGetValue(typeof(T), out IList entry))
            {
                list = entry as IList<T>;
                list.Clear(); //reset list
            }
            else //create and add
            {
                list = new List<T>(startingSize);
                listTable.Add(typeof(T), list);
            }

            return list;
        }

        /// <summary>
        /// Removes the give <see cref="List{T}"/> from the collection.
        /// It can be <see cref="Get{T}"/>ed again.
        /// </summary>
        public void Return<T>()
            => listTable.Remove(typeof(T));
    }
}
