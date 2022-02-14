using System;
using System.Collections;
using System.Collections.Generic;

namespace RichPackage.Collections
{
    public class MinHeap< T> : AHeap<T>
        where T : IComparable<T>
    {
        #region Constructors

        public MinHeap() : base() {}

        public MinHeap(IEnumerable<T> collection) : base(collection) {}

        #endregion

        protected override void HeapifyUp(int index)
        {
            int parentIndex = GetParentIndex(index);
            if (parentIndex >= 0 && elements[index].CompareTo(elements[parentIndex]) < 0)
            {
                Swap(index, parentIndex);
                HeapifyUp(parentIndex);
            }
        }

        protected override void HeapifyDown(int index)
        {
            int smallest = index;
            int left = GetLeftIndex(index);
            int right = GetRightIndex(index);

            if (left < Size && elements[left].CompareTo(elements[index]) < 0)
                smallest = left;

            if (right < Size && elements[right].CompareTo(elements[smallest]) < 0)
                smallest = right;

            if (smallest != index)
            {
                Swap(index, smallest);
                HeapifyDown(smallest);
            }
        }
    }
}
