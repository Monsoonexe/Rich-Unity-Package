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

        protected readonly List<T> elements = new List<T>(32); 

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

        /// <summary>
        /// Look at the next item in the heap (without actually removing it).
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public T Peek() => (elements.Count > 0) ? elements[FRONT] 
            : throw new InvalidOperationException("No elements in heap.");

        /// <summary>
        /// Get the next item from the heap.
        /// </summary>
        public T Pop()
        {
            if (elements.Count == 0)
                throw new InvalidOperationException("No elements in heap.");
            
            int BACK = elements.Count - 1; //cache for re-use
            T item = elements[FRONT];
            elements[FRONT] = elements[BACK];
            elements.RemoveAt(BACK);
            HeapifyDown(FRONT);
            return item;
        }

        /// <summary>
        /// Add an item to the heap.
        /// </summary>
        public void Push(in T item)
        {
            elements.Add(item);
            HeapifyUp(elements.Count - 1);
        }

        #region Modify
            
        /// <summary>
        /// Modify the first item that returns true from <paramref name="query"/> and
        /// process it with <paramref name="procedure"/>. <br/>
        /// Prefer <see cref="ModifyItem(Predicate{T}, Action{T})"/> if {T} is an object.
        /// </summary> <br/>
        public void ModifyItem(Predicate<T> query, ActionRef<T> procedure)
        {
            int count = elements.Count;
            for(int i = 0; i < count; ++i)
            {
                T element = elements[i];
                if(query(element))
                {
                    procedure(ref element);
                    elements[i] = element; //reassign (for value types)
                    HeapifyUp(i);
                    HeapifyDown(i);
                    break;
                }
            }
        }
            
        /// <summary>
        /// Modify the first item that returns true from <paramref name="query"/> and
        /// process it with <paramref name="procedure"/>. <br/>
        /// Throws a <see cref="NotSupportedException"/> if {T} is a value type.
        /// Prefer <see cref="ModifyItems(Predicate{T}, ActionRef{T})"/> if {T} is a value type.
        /// </summary> <br/>
        /// <exceptions><see cref="NotSupportedException"/></exceptions>
        public void ModifyItem(Predicate<T> query, Action<T> procedure)
        {
            if(typeof(T) != typeof(object))
                throw new NotSupportedException("This method does not work on value types as it won't modify the underlying value in the backing array.");

            int count = elements.Count;
            for(int i = 0; i < count; ++i)
            {
                T element = elements[i];
                if(query(element))
                {
                    procedure(element);
                    HeapifyUp(i);
                    HeapifyDown(i);
                    break;
                }
            }
        }
        
        /// <summary>
        /// Modify all items that returns true from <paramref name="query"/> and
        /// process it with <paramref name="procedure"/>. <br/>
        /// Prefer <see cref="ModifyItems(Predicate{T}, Action{T})"/> if {T} is an object.
        /// </summary>
        public void ModifyItems(Predicate<T> query, ActionRef<T> procedure)
        {
            int count = elements.Count;
            for(int i = 0; i < count; ++i)
            {
                T element = elements[i];
                if(query(element))
                {
                    procedure(ref element);
                    elements[i] = element; //reassign (for value types)
                }
            }
            Heapify();
        }

        /// <summary>
        /// Modify all items that returns true from <paramref name="query"/> and
        /// process it with <paramref name="procedure"/>. <br/>
        /// Throws a <see cref="NotSupportedException"/> if {T} is a value type.
        /// Prefer <see cref="ModifyItems(Predicate{T}, ActionRef{T})"/> if {T} is a value type.
        /// </summary>
        /// <exceptions><see cref="NotSupportedException"/></exceptions>
        public void ModifyItems(Predicate<T> query, Action<T> procedure)
        {
            if(typeof(T) != typeof(object))
                throw new NotSupportedException("This method does not work on value types as it won't modify the underlying value in the backing array.");

            int count = elements.Count;
            for(int i = 0; i < count; ++i)
            {
                T element = elements[i];
                if(query(element))
                    procedure(element);
            }
            Heapify();
        }

        #endregion

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static int GetParentIndex(int index)
            => (index - 1) / 2;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static int GetLeftIndex(int index)
            => 2 * index + 1;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected static int GetRightIndex(int index)
            => 2 * index + 2;

        #endregion

        #region Collection interface

        public int Size => elements.Count;

        public void PushRange(IEnumerable<T> source)
        {
            elements.AddRange(source);
            Heapify();
        }

        /// <summary>
        /// Calls <paramref name="action"/> on each element in backing array. <br/>
        /// Follow this call with <see cref="Heapify"/> if the backing array is modified.
        /// Prefer <see cref="ForEach(ActionRef{T})"/> if {T} is a value type and the values are modified.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ForEach(Action<T> action)
            => elements.ForEach(action);

        /// <summary>
        /// Calls <paramref name="action"/> on each element in backing array. <br/>
        /// Follow this call with <see cref="Heapify"/> if the backing array is modified.
        /// Prefer <see cref="ForEach(Action{T})"/> if {T} is a reference type.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)] 
        public void ForEach(ActionRef<T> action)
        {
            for(int i = 0; i < Size; ++i)
            {
                T element = elements[i];
                action(ref element);
                elements[i] = element;
            }
        }

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
