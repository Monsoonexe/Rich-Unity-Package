/* Inspired by: https://github.com/JimmyCushnie/JimmysUnityUtilities/blob/master/Scripts/Threading/LockedList.cs
 */

using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace RichPackage.Threading
{
    /// <summary>
    /// An implementation of <see cref="IList{T}"/> that is thread-safe;
    /// it acquires a lock before executing any read or write operation.
    /// </summary>
    public class ConcurrentList<T> : IList<T>
    {
        private readonly List<T> InternalList = new List<T>();
        public readonly object __InternalListLock = new object();

        public IEnumerator<T> GetEnumerator() => new Enumerator(this);

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();

        public T this[int index]
        {
            get
            {
                lock (__InternalListLock)
                    return InternalList[index];
            }
            set
            {
                lock (__InternalListLock)
                    InternalList[index] = value;
            }
        }

        public int Count
        {
            get
            {
                lock (__InternalListLock)
                    return InternalList.Count;
            }
        }

        public bool IsReadOnly => false;

        public void Add(T item)
        {
            lock (__InternalListLock)
                InternalList.Add(item);
        }

        public void Clear()
        {
            lock (__InternalListLock)
                InternalList.Clear();
        }

        public bool Contains(T item)
        {
            lock (__InternalListLock)
                return InternalList.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            lock (__InternalListLock)
                InternalList.CopyTo(array, arrayIndex);
        }

        public int IndexOf(T item)
        {
            lock (__InternalListLock)
                return InternalList.IndexOf(item);
        }

        public void Insert(int index, T item)
        {
            lock (__InternalListLock)
                InternalList.Insert(index, item);
        }

        public bool Remove(T item)
        {
            lock (__InternalListLock)
                return InternalList.Remove(item);
        }

        public void RemoveAt(int index)
        {
            lock (__InternalListLock)
                InternalList.RemoveAt(index);
        }

        /// <summary>
        /// Custom enumerator that uses <see cref="Monitor"/> directly 
        /// to support thread-safe enumeration such as foreach loops.
        /// </summary>
        public readonly struct Enumerator : IEnumerator<T>
        {
            private readonly IEnumerator<T> innerEnumerator;
            private readonly ConcurrentList<T> owner;

            public Enumerator(ConcurrentList<T> owner)
            {
                Monitor.Enter(owner.__InternalListLock);
                this.owner = owner;
                this.innerEnumerator = owner.InternalList.GetEnumerator();
            }

            public T Current => innerEnumerator.Current;

            object IEnumerator.Current => innerEnumerator.Current;

            public void Dispose()
            {
                innerEnumerator.Dispose();
                Monitor.Exit(owner.__InternalListLock);
            }

            public bool MoveNext()
            {
                return innerEnumerator.MoveNext();
            }

            public void Reset()
            {
                innerEnumerator.Reset();
            }
        }
    }
}
