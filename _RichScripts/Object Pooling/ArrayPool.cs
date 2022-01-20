//An attempted implementation of the ArrayPool class from the .NET Core 6.0 library
//https://docs.microsoft.com/en-us/dotnet/api/system.buffers.arraypool-1.shared?view=net-6.0#system-buffers-arraypool-1-shared

using System;
using System.Collections.Generic;

namespace RichPackage
{
    /// <summary>
    /// Using the ArrayPool<T> class to Rent() and Return() buffers <br/>
    /// can improve performance in situations where arrays are created and destroyed frequently, <br/>
    /// resulting in significant memory pressure on the garbage collector.
    /// </summary>
    public class ArrayPool<T>
    {
        public static ArrayPool<T> Shared { get; } = new ArrayPool<T>();

        private readonly List<T[]> _pool;

        /// <summary>
        /// Max count of buffers that will be kept in storage. < 0 for unlimited.
        /// </summary>
        public int MaxBucketCapacity { get; private set; }

        public ArrayPool(int maxBucketCapacity = -1)
        {
            if (maxBucketCapacity == 0)
                throw new ArgumentException("maxBucketCapacity must be greater than 0 or -1 for no limit.");

            MaxBucketCapacity = maxBucketCapacity;
            int size = MaxBucketCapacity > 0 ? MaxBucketCapacity : 16;
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
            //iterate backwards to reduce left-shifts of elements after removal.
            for (int i = _pool.Count - 1; i >= 0; --i)
            {
                if (_pool[i].Length >= minSize)
                {
                    T[] item = _pool[i];
                    _pool.RemoveAt(i);
                    if (clear)
                        Array.Clear(item, 0, item.Length);
                    return item;
                }
            }

            return new T[minSize];
        }

        public void Return(T[] array)
        {
            if (MaxBucketCapacity < 0 || _pool.Count < MaxBucketCapacity)
                _pool.Add(array);
            else //drop smallest array
            {
                T[] shortestItem = _pool.GetShortestList();
                if (shortestItem.Length < array.Length)
                {
                    _pool.Remove(shortestItem);
                    _pool.Add(array);
                }
            }
        }
    }
}
