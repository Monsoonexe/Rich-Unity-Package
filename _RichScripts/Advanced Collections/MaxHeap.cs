// https://en.wikipedia.org/wiki/Binary_heap

using System.Collections.Generic;

namespace RichPackage.Collections
{
    /// <summary>
    /// Binary heap data structure.
    /// </summary>
    /// <seealso cref="MinHeap{T}"/>
    public class MaxHeap<T> : AHeap<T>
    {
        #region Constructors

        public MaxHeap() : base() { }

        public MaxHeap(int capacity) : base(capacity) { }

        public MaxHeap(IEnumerable<T> collection) : base(collection) {}

        #endregion

        protected override void HeapifyUp(int index)
        {
            int parentIndex = GetParentIndex(index);
            if (parentIndex >= 0 && comparer.Compare(elements[index], elements[parentIndex]) > 0)
            {
                Swap(index, parentIndex);
                HeapifyUp(parentIndex);
            }
        }

        protected override void HeapifyDown(int index)
        {
            int size = elements.Count;
            int largest = index;
            int left = GetLeftIndex(index);
            int right = GetRightIndex(index);

            if (left < size && comparer.Compare(elements[left], elements[index]) > 0)
                largest = left;

            if (right < size && comparer.Compare(elements[right], elements[largest]) > 0)
                largest = right;

            if (largest != index)
            {
                Swap(index, largest);
                HeapifyDown(largest);
            }
        }
    }
}
