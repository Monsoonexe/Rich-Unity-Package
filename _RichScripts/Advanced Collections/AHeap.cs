/*
https://www.geeksforgeeks.org/min-heap-in-java/
https://en.wikipedia.org/wiki/Binary_heap
*/


using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

//TODO - implement ICollection or IEnumerable or something

namespace RichPackage.Collections
{
    public abstract class AHeap<T>
    {
        protected const int FRONT = 0;

        protected readonly List< T> elements = new List<T>(32); 

        #region Constructors

        public AHeap()
        {
            //nada
        }

        public AHeap(IEnumerable<T> source)
        {
            PushRange(source);
        }
        
        #endregion

        public void Push(T item)
        {
            elements.Add(item);
            HeapifyUp(elements.Count - 1);
        }

        public T Pop()
        {
            if (elements.Count == 0)
                throw new InvalidOperationException("No elementa in heap.");
            
            T item = elements[0];
            elements[0] = elements[elements.Count - 1];
            elements.RemoveAt(elements.Count - 1);
            HeapifyDown(FRONT);
            return item;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Peek() => (elements.Count > 0) ? elements[FRONT] 
            : throw new InvalidOperationException("No elements in heap.");

        #region Heapify

        protected abstract void HeapifyUp(int index); // to be overridden by MaxHeap and MinHeap

        protected abstract void HeapifyDown(int index); // to be overridden by MaxHeap and MinHeap

        /// <summary>
        /// Drivers must call this method after modifying sort values of the items in the heap. <br/>
        /// Such as: <see cref="ForEach(Action{T})"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Heapify()
        {
            for (int i = elements.Count / 2; i >= 0; i--) //non-leaf nodes
                HeapifyDown(i);
        }

        #endregion

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static int GetParentIndex(int index)
            => (index - 1) / 2;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static int GetLeftIndex(int index)
            => 2 * index + 1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static int GetRightIndex(int index)
            => 2 * index + 2;

        #region Collection interface

        public int Size => elements.Count;

        public void PushRange(IEnumerable<T> source)
        {
            elements.AddRange(source);
            Heapify();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ForEach(Action<T> action)
            => elements.ForEach(action);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void TrimExcess() => elements.TrimExcess();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Clear() => elements.Clear();

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool Contains(T item) => elements.Contains(item);
        
        /// <summary>
        /// Access the underlying array. Will re-heapify the array after the operation completes.
        /// Probably not a good idea to use this.
        /// </summary>
        public T this[int index]
        {
            get => elements[index];
            set
            {
                elements[index] = value;
                HeapifyUp(index);
                HeapifyDown(index);
            }
        }

        #endregion
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void Swap(int a, int b)
        {
            T item = elements[a];
            elements[a] = elements[b];
            elements[b] = item;
        }
    }
}
