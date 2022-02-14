using System;
using System.Collections;
using System.Collections.Generic;

namespace RichPackage.Collections
{
    public class MaxHeap< T> : AHeap<T>
        where T : IComparable<T>
    {
        #region Constructors

        public MaxHeap() : base() {}

        public MaxHeap(IEnumerable<T> collection) : base(collection) {}

        #endregion

        protected override void HeapifyUp(int index)
        {
            int parentIndex = GetParentIndex(index);
            if (parentIndex >= 0 && elements[index].CompareTo(elements[parentIndex]) > 0)
            {
                Swap(index, parentIndex);
                HeapifyUp(parentIndex);
            }
        }

        protected override void HeapifyDown(int index)
        {
            int largest = index;
            int left = GetLeftIndex(index);
            int right = GetRightIndex(index);

            if (left < Size && elements[left].CompareTo(elements[index]) > 0)
                largest = left;

            if (right < Size && elements[right].CompareTo(elements[largest]) > 0)
                largest = right;

            if (largest != index)
            {
                Swap(index, largest);
                HeapifyDown(largest);
            }
        }
    }
}
