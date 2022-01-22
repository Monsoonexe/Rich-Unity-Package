using System;
using System.Collections.Generic;

namespace RichPackage
{
	/// <summary>
	/// Generic pool for anything.
	/// </summary>
	/// <seealso cref="ArrayPool{T}"/>
	public class ClassPool<T>
	{
		/// <summary>
		/// Shared class if you don't want to instantiate your own.
		/// </summary>
		public static readonly ClassPool<T> Shared = new ClassPool<T>();

		private Stack<T> pool;

		/// <summary>
		/// Can override this to provide a custom initialization for the pooled object.
		/// </summary>
		public Func<T> factory = () => default;// new T(); //call default constructor

		public int Count => pool.Count;

		public int MaxCount { get; set; } = -1;

		public ClassPool(int maxCount = -1)
		{
			MaxCount = maxCount;
			int amount = MaxCount >= 0 ? MaxCount : 16;
			pool = new Stack<T>(amount);
		}

		/// <summary>
		/// Will always return an intance of T.
		/// </summary>
		public T Depool()
		{
			T depooledItem = default;

			if (pool.Count > 0)
				depooledItem = pool.Pop();
			else
				depooledItem = factory(); // create a new one

			return depooledItem;
		}

		/// <summary>
		/// Will only enpool if the pool is not full.
		/// </summary>
		public void Enpool(T item)
		{
			if (MaxCount < 0 || pool.Count < MaxCount)
				pool.Push(item);
		}
	}
}