//An attempted implementation of the ArrayPool class from the .NET Core 6.0 library
//https://docs.microsoft.com/en-us/dotnet/api/system.buffers.arraypool-1.shared?view=net-6.0#system-buffers-arraypool-1-shared

using System;
using System.Collections.Generic;

namespace RichPackage
{
    /// <summary>
    /// Using the <see cref="ArrayPool{T}"/> class to <see cref="Rent"/> and <see cref="Return"/> buffers <br/>
    /// can improve performance in situations where arrays are created and destroyed frequently, <br/>
    /// resulting in significant memory pressure on the garbage collector.
    /// </summary>
    /// <seealso cref="Pooling.ObjectPool{T}{T}"/>
    public class ArrayPool<T>
    {
        public static ArrayPool<T> Shared { get; } = new ArrayPool<T>();
        public static readonly IComparer<T[]> comparer = IListCountReverseComparer.Default; //sort descending

        private readonly List<T[]> _pool;

        /// <summary>
        /// Max count of buffers that will be kept in storage. < 0 for unlimited.
        /// </summary>
        public int MaxBucketCapacity { get; private set; }

        public int MaxArraySize = 4096;

        public ArrayPool(int maxBucketCapacity = -1)
        {
            if (maxBucketCapacity == 0)
                throw new ArgumentException("maxBucketCapacity must be greater than 0 or -1 for no limit.");

            MaxBucketCapacity = maxBucketCapacity;
            int size = maxBucketCapacity > 0 ? maxBucketCapacity : 16;
            _pool = new List<T[]>(size);
        }

        /// <summary>
        /// Rent a buffer of at least 'minSize' from the pool.
        /// </summary>
        /// <param name="minSize">Minimum size of the buffer to be rented.</param>
        /// <param name="clear">If true, the buffer will be cleared before being returned.</param>
        /// <returns>A buffer of at least 'minSize' from the pool.</returns>
        public T[] Rent(int minSize, bool clear = false)
        {
            int i = _pool.Count - 1;

            //if there is an item that will fit
            if (!(i > 1 && minSize > _pool[0].Length)) //largest size is at 0
            {
                //iterate backwards to reduce left-shifts of elements after removal.
                for (; i >= 0; --i)
                {
                    if (_pool[i].Length >= minSize)
                    {
                        T[] item = _pool.GetRemoveAt(i);
                        if (clear)
                            Array.Clear(item, 0, item.Length);
                        return item;
                    }
                }
            }

            return new T[minSize];
        }

        /// <summary>
        /// Returns a buffer to the pool.
        /// </summary>
        /// <param name="buffer">The buffer to return to the pool.</param>
        /// <param name="clear">If true, the buffer will be cleared before being added. This is good practice for reference types.</param>
        public void Return(T[] array, bool clear = false)
        {
            if (MaxArraySize > 0 && array.Length > MaxArraySize)
			{
                //drop arrays that are too large for pool
			}
            else if(MaxBucketCapacity < 0 || _pool.Count < MaxBucketCapacity)
            {
                _pool.Add(array);
                if(clear)
                    Array.Clear(array, 0, array.Length);
            }
            else //drop smallest array
            {
                T[] shortestItem = _pool.GetShortestList();
                if (shortestItem.Length < array.Length)
                {
                    _pool.Remove(shortestItem);
                    _pool.Add(array);
                    _pool.Sort(comparer); //sort descending
                    if(clear)
                        Array.Clear(array, 0, array.Length);
                }
            }
        }
    }
}
