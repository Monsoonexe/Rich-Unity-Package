using System;
using System.Collections.Generic;

namespace RichPackage.Pooling
{
	/// <summary>
	/// Generic pool for anything.
	/// </summary>
	/// <seealso cref="ArrayPool{T}"/>
	public class ObjectPool<T>
	{
		/// <summary>
		/// Shared class if you don't want to instantiate your own.
		/// </summary>
		public static readonly ObjectPool<T> Shared = new ObjectPool<T>();

		private readonly Stack<T> pool;

		/// <summary>
		/// Can override this to provide a custom initialization for the pooled object. <br/>
		/// Creates 'null' for reference types. 
		/// </summary>
		public Func<T> FactoryMethod = () => default;// new T(); //call default constructor

		/// <summary>
		/// If not null, this function will be called when an Item is removed <br/>
		/// from the pool.
		/// </summary>
		private Action<T> OnEnpoolMethod;

		/// <summary>
		/// If not null, this function will be called when an Item is added <br/>
		/// back to the pool.
		/// </summary>
		private Action<T> OnDepoolMethod;

		public int Count => pool.Count;

		/// <summary>
		/// Max count of items this pool will hold. If more than MaxCount items are
		/// added to the pool, they will be dropped rather than enpooled. <br/>
		/// MaxCount less than 1 implies there is no internal limit to the size of the pool.
		/// </summary>
		public int MaxCount { get; set; } = -1;

		public ObjectPool(int maxCount = -1)
		{
			MaxCount = maxCount;
			int amount = MaxCount >= 0 ? MaxCount : 16;
			pool = new Stack<T>(amount);
		}

		/// <param name="maxCount">Max count of items this pool will hold. If more than MaxCount items are
		/// added to the pool, they will be dropped rather than enpooled.<br/>
		/// MaxCount less than 1 implies there is no internal limit to the size of the pool.</param>
		/// <param name="preInit">Number of items to create up front.</param>
		public ObjectPool(int maxCount, int preInit)
			: this(maxCount)
		{
			//prebake
			for (int i = 0; i < preInit; ++i)
				pool.Push(FactoryMethod());
		}

		public ObjectPool(int maxCount, int preInit,
			Func<T> factoryMethod, Action<T> enpoolMethod = null,
			Action<T> depoolMethod = null)
			: this(maxCount)
		{
			FactoryMethod = factoryMethod;
			OnEnpoolMethod = enpoolMethod;
			OnDepoolMethod = depoolMethod;

			//prebake
			for (int i = 0; i < preInit; ++i)
				pool.Push(FactoryMethod());
		}

		/// <summary>
		/// Will always return an intance of T.
		/// Will call <see cref="OnDepoolMethod"/> if it is not null.
		/// </summary>
		/// <returns>A pooled item or one from the <seealso cref="FactoryMethod"/> if pool is empty.</returns>
		public T Depool()
		{
			T item = default;

			if (pool.Count > 0)
				item = pool.Pop();
			else
				item = FactoryMethod(); // create a new one

			OnDepoolMethod?.Invoke(item);
			return item;
		}

		/// <summary>
		/// Will only enpool if the pool is not full.
		/// Will call <see cref="OnEnpoolMethod"/> if it is not null.
		/// </summary>
		public void Enpool(T item)
		{
			if (MaxCount < 0 || pool.Count < MaxCount)
			{
				pool.Push(item);
				OnEnpoolMethod?.Invoke(item);
			}
		}

		public void TrimExcess() => pool.TrimExcess();
	}
}
