using System;
using System.Collections.Generic;
using RichPackage.Collections;

namespace RichPackage.Pooling
{
	/// <summary>
	/// Generic pool for anything.
	/// </summary>
	/// <seealso cref="ArrayPool{T}"/>
	public class ObjectPool<T>
	{
		protected const int Default_Capacity = 16;

		/// <summary>
		/// Count of items inside the pool.
		/// </summary>
		public int Count {get; private set; } = 0;

		/// <summary>
		/// Should the pool continue to create more items when it is empty? <br/>
		/// If true, another item will be created using the <see cref="FactoryMethod"/>. <br/>
		/// if false, an <see cref="InvalidOperationException"/> is thrown
		///	when the pool is empty and an item is requested with <see cref="Depool"/>.
		/// </summary>
		public bool CreateNewWhenEmpty = true;

		/// <summary>
		/// A reference to the underlying pool.
		/// </summary>
		public object? PoolObject { get; protected set;}
		
		/// <summary>
		/// Function used to remove items from the pool. <br/>
		/// Example: Stack.Pop();
		/// </summary>
		protected Func<T> DepoolerMethod;
		
		/// <summary>
		/// Function used to add items into the pool. <br/>
		/// Example: Stack.Push(item);
		/// </summary>
		protected Action<T> EnpoolerMethod;

		/// <summary>
		/// This function is used to create new instances. Can supply a different method to 
		///	provide a custom creator for the pooled object. <br/>
		/// Example: () => new T(); <br/>
		///	Default is 'null' for reference types. <br/>
		/// </summary>
		public Func<T> FactoryMethod = () => default;

		/// <summary>
		/// The maximum number of items this pool will hold. If more than MaxCount items are
		/// added to the pool, they will be dropped rather than enpooled. <br/>
		/// MaxCount less than 0 implies there is no internal limit to the size of the pool.
		/// </summary>
		public int MaxCount { get; set; }

		/// <summary>
		/// If not null, this function will be called when an Item is removed <br/>
		/// from the pool. Use this to de-initialize your items as they are returned to the pool.
		/// </summary>
		public Action<T> OnEnpoolMethod;

		/// <summary>
		/// If not null, this function will be called when an Item is added <br/>
		/// back to the pool. Use this to initialize your items as they are removed from the pool.
		/// </summary>
		public Action<T> OnDepoolMethod;

		#region Constructors
		
		/// <summary>
		/// By default implements a <see cref="Stack{T}"/>.
		/// </summary>
		/// <seealso cref="StackPool{T}"/>
		public ObjectPool() : this(-1) {}

		/// <summary>
		/// By default implements a <see cref="Stack{T}"/>.
		/// </summary>
		/// <seealso cref="StackPool{T}"/>
		public ObjectPool(int maxCount)
		{
			MaxCount = maxCount;
			int capacity = MaxCount >= 0 ? MaxCount : Default_Capacity;
			
			var stack = new Stack<T>(capacity);
			EnpoolerMethod = stack.Push;
			DepoolerMethod = stack.Pop;
			PoolObject = stack;
		}

		/// <summary>
		/// By default implements a <see cref="Stack{T}"/>.
		/// </summary>
		/// <seealso cref="StackPool{T}"/>
		public ObjectPool(int maxCount, int preInit) : this(maxCount)
		{
			for (int i = 0; i < preInit; i++)
				Enpool(FactoryMethod());
		}

		/// <summary>
		/// Creates a new pool where the pooling behaviour is delegated.
		///	Use this constructor when you are using a custom pooling behaviour 
		///	and you don't want to create a new class that derives from <see cref="ObjectPool{T}"/>.
		/// </summary>
		public ObjectPool(int maxCount, Func<T> DepoolerMethod, Action<T> EnpoolerMethod)
		{
			//validate
			if (DepoolerMethod == null)
				throw new ArgumentNullException(nameof(DepoolerMethod));

			if (EnpoolerMethod == null)
				throw new ArgumentNullException(nameof(EnpoolerMethod));

			//work
			MaxCount = maxCount;
			int capacity = MaxCount >= 0 ? MaxCount : Default_Capacity;

			this.DepoolerMethod = DepoolerMethod;
			this.EnpoolerMethod = EnpoolerMethod;
		}

		#endregion

		/// <summary>
		/// Will always return an intance of T.
		/// Will call <see cref="OnDepoolMethod"/> if it is not null.
		/// </summary>
		/// <returns>A pooled item or one from the <seealso cref="FactoryMethod"/> 
		///	if pool is empty.</returns>
		/// <exception cref="InvalidOperationException">Thrown if there are no 
		///	more items in the pool and <see cref="CreateNewWhenEmpty"/> is not set.</exception>
		public T Depool()
		{
			T item = default;

			if (Count > 0)
			{
				item = DepoolerMethod(); //remove item from pool
				--Count; //track
			}
			else 
			{ 
				if (CreateNewWhenEmpty)
					item = FactoryMethod();
				else 
					throw new InvalidOperationException("Pool is empty.");
			}

			OnDepoolMethod?.Invoke(item);
			return item;
		}

		/// <summary>
		/// Will only enpool if the pool is not full.
		/// Will call <see cref="OnEnpoolMethod"/> if it is not null.
		/// </summary>
		public void Enpool(T item)
		{
			if (MaxCount < 0 || Count < MaxCount)
			{
				EnpoolerMethod(item); //add to pool
				OnEnpoolMethod?.Invoke(item); //de-init
				++Count; //track
			}
		}
	}

	#region Special Pools

	/// <summary>
	/// A pool that is backed by a <see cref="MaxHeap{T}"/>.
	/// </summary>
	public class MaxHeapPool<T> : ObjectPool<T>
	{
		public MaxHeap<T> MaxHeap { get => (MaxHeap<T>)PoolObject; }

		public MaxHeapPool() : this(-1) { }

		public MaxHeapPool(int maxCount)
		{
			MaxCount = maxCount;
			int capacity = MaxCount >= 0 ? MaxCount : Default_Capacity;
			
			var heap = new MaxHeap<T>(capacity);
			EnpoolerMethod = heap.Push;
			DepoolerMethod = heap.Pop;
			PoolObject = heap;
		}
		
		public MaxHeapPool(int maxCount, int preInit) : this(maxCount)
		{
			for (int i = 0; i < preInit; i++)
				Enpool(FactoryMethod());
		}
	}

	/// <summary>
	/// A pool that is backed by a <see cref="MinHeap{T}"/>.
	/// </summary>
	public class MinHeapPool<T> : ObjectPool<T>
	{
		public MinHeap<T> MinHeap { get => (MinHeap<T>)PoolObject; }

		public MinHeapPool() : this(-1) { }

		public MinHeapPool(int maxCount)
		{
			MaxCount = maxCount;
			int capacity = MaxCount >= 0 ? MaxCount : Default_Capacity;
			
			var heap = new MinHeap<T>(capacity);
			EnpoolerMethod = heap.Push;
			DepoolerMethod = heap.Pop;
			PoolObject = heap;
		}
		
		public MinHeapPool(int maxCount, int preInit) : this(maxCount)
		{
			for (int i = 0; i < preInit; i++)
				Enpool(FactoryMethod());
		}
	}

	/// <summary>
	/// A pool that is backed by a <see cref="Queue{T}"/>.
	/// </summary>
	public class QueuePool<T> : ObjectPool<T>
	{
		public Queue<T> Queue { get => (Queue<T>)PoolObject; }

		public QueuePool() : this(-1) { }

		public QueuePool(int maxCount)
		{
			MaxCount = maxCount;
			int capacity = MaxCount >= 0 ? MaxCount : Default_Capacity;
			
			var queue = new Queue<T>(capacity);
			EnpoolerMethod = queue.Enqueue;
			DepoolerMethod = queue.Dequeue;
			PoolObject = queue;
		}
		
		public QueuePool(int maxCount, int preInit) : this(maxCount)
		{
			for (int i = 0; i < preInit; i++)
				Enpool(FactoryMethod());
		}
	}

	/// <summary>
	/// A pool that is backed by a <see cref="Stack{T}"/>.
	/// </summary>
	/// <remarks>By default, <see cref="ObjectPool{T}"/> implements a <see cref="Stack{T}",
	///	so this class just strongly types that behaviour.</remarks>
	public class StackPool<T> : ObjectPool<T>
	{
		public Stack<T> Stack { get => (Stack<T>)PoolObject; }

		public StackPool() : base() { }

		public StackPool(int maxCount) : base(maxCount) { }
		
		public StackPool(int maxCount, int preInit) : base(maxCount, preInit) { }
	}

	#endregion
}
