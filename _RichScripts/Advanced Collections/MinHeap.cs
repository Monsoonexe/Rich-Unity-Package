using System;
using System.Collections.Generic;

namespace RichPackage.Collections
{
	/// <summary>
	/// 
	/// </summary>
	/// <seealso cref="MaxHeap{T}"/>
	public class MinHeap<T> : AHeap<T>
	{
		#region Constructors

		public MinHeap() : base() {}

		public MinHeap(int capacity) : base(capacity) { }

		public MinHeap(IEnumerable<T> collection) : base(collection) {}

		#endregion

		protected override void HeapifyUp(int index)
		{
			int parentIndex = GetParentIndex(index);
			if (parentIndex >= 0 && comparer.Compare(elements[index], elements[parentIndex]) < 0)
			{
				Swap(index, parentIndex);
				HeapifyUp(parentIndex);
			}
		}

		protected override void HeapifyDown(int index)
		{
			int size = elements.Count;
			int smallest = index;
			int left = GetLeftIndex(index);
			int right = GetRightIndex(index);

			if (left < size && comparer.Compare(elements[left], elements[index]) < 0)
				smallest = left;

			if (right < size && comparer.Compare(elements[right], elements[smallest]) < 0)
				smallest = right;

			if (smallest != index)
			{
				Swap(index, smallest);
				HeapifyDown(smallest);
			}
		}
	}
}
