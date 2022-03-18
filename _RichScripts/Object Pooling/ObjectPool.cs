using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using RichPackage.Collections;

namespace RichPackage.Pooling
{
	/// <summary>
	/// Generic pool for anything.
	///	This is the preferred <see cref="ObjectPool{T}"/> if you 
	/// don't want to worry about the internal workings of the pool.
	/// </summary>
	/// <seealso cref="StackPool{T}"/>
	/// <seealso cref="QueuePool{T}"/>
	/// <seealso cref="MinHeapPool{T}"/>
	/// <seealso cref="MaxHeapPool{T}"/>
	/// <seealso cref="ArrayPool{T}"/>
	public class ObjectPool<T>
	{
		protected const int Default_Initial_Capacity = 16;
		protected const int Infinite_Pool_Size = -1;

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
		///	Use <see cref="MaxCount"/> property instead of directly accessing this backer.
		/// </summary>
		private int _maxCount; //

		/// <summary>
		/// The maximum number of items this pool will hold. If more than MaxCount items are
		/// added to the pool, they will be dropped rather than enpooled. <br/>
		/// MaxCount &lt; 0 implies there is virtually no internal limit to the size of the pool (<see cref="MaxCount"/> == <see cref="int.MaxValue"/>).
		/// </summary>
		/// <remarks>Won't trim pool if <see cref="Count"/> &gt; <see cref="MaxCount"/>. See <see cref="TrimPool"/>.</remarks>
		public int MaxCount 
		{ 
			get => _maxCount;
		 	set => _maxCount = value < 0 ? int.MaxValue : value; // if value is less than 0, set to max value
		}

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
		public ObjectPool() : this(Infinite_Pool_Size, Default_Initial_Capacity) {}

		/// <summary>
		/// By default implements a <see cref="Stack{T}"/>.
		/// </summary>
		/// <seealso cref="StackPool{T}"/>
		public ObjectPool(int maxCount) : this(maxCount, Default_Initial_Capacity) {}

		/// <summary>
		/// By default implements a <see cref="Stack{T}"/>.
		/// </summary>
		/// <param name="initialCapacity">The initial capacity of the pool.</param>
		/// <seealso cref="StackPool{T}"/>
		public ObjectPool(int maxCount, int initialCapacity)
		{
			MaxCount = maxCount;

			int initialAmount = CalculateInitialCapacity(initialCapacity);
			var pool = new Stack<T>(initialAmount);
			ConfigurePool(pool.Push, pool.Pop, pool);
		}

		/// <summary>
		/// By default implements a <see cref="Stack{T}"/>.
		/// </summary>
		/// <seealso cref="StackPool{T}"/>
		/// <param name="initialCapacity">The initial capacity of the pool.</param>
		/// <param name="preInit">A number of elements to create and add to the pool right now using <see cref="FactoryMethod"/>.</param>
		public ObjectPool(int maxCount, int initialCapacity, int preInit) : this(maxCount, initialCapacity) 
		{
			for (int i = 0; i < preInit; i++)
				Enpool(FactoryMethod());
		}
		
		/// <summary>
		/// By default implements a <see cref="Stack{T}"/>.
		/// </summary>
		/// <seealso cref="StackPool{T}"/>
		/// <param name="initialCapacity">The initial capacity of the pool.</param>
		/// <param name="preInit">A number of elements to create and add to the pool right now using <paramref name="factoryMethod"/>.</param>
		/// <param name="factoryMethod">A number of elements to add to the pool right now using <see cref="FactoryMethod"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="factoryMethod"/> is null.</exception>
		public ObjectPool(int maxCount, int initialCapacity, int preInit, Func<T> factoryMethod) : this(maxCount, initialCapacity) 
		{
			//validate
			if (factoryMethod == null)
				throw new ArgumentNullException(nameof(factoryMethod));

			//work
			FactoryMethod = factoryMethod;
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

			this.DepoolerMethod = DepoolerMethod;
			this.EnpoolerMethod = EnpoolerMethod;
		}

		#endregion

		#region API

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public void Add() => Add(1);

		public void Add(int count)
		{
			for (int i = 0; i < count; i++)
				Enpool(FactoryMethod());
		}

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

		public void Clear()
		{
			while (Count > 0)
				Depool();
		}

		#endregion

		#region Utility

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected static int CalculateInitialCapacity(int initialAmount)
			=> initialAmount >= 0 ? initialAmount : Default_Initial_Capacity;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		protected void ConfigurePool(Action<T> enpoolMethod, Func<T> depoolMethod, object pool)
		{
			EnpoolerMethod = enpoolMethod;
			DepoolerMethod = depoolMethod;
			PoolObject = pool;
		}

		#endregion
	}

	#region Special Pools

	/// <summary>
	/// A pool that is backed by a <see cref="MaxHeap{T}"/>.
	/// </summary>
	/// <seealso cref="MaxHeap{T}"/>
	public class MaxHeapPool<T> : ObjectPool<T>
	{
		/// <summary>
		/// Internal backing object. It is safe to manipulate this backing object, but do NOT change the Count.
		/// </summary>
		public MaxHeap<T> MaxHeap { get => (MaxHeap<T>)PoolObject; }

		#region Constructors

		public MaxHeapPool() : this(Infinite_Pool_Size, Default_Initial_Capacity) {}

		public MaxHeapPool(int maxCount) : this(maxCount, Default_Initial_Capacity) {}

		/// <param name="initialCapacity">The initial capacity of the pool.</param>
		public MaxHeapPool(int maxCount, int initialCapacity)
		{
			MaxCount = maxCount;

			int initialAmount = CalculateInitialCapacity(initialCapacity);
			var pool = new MaxHeap<T>(initialAmount);
			ConfigurePool(pool.Push, pool.Pop, pool);
		}
		
		/// <param name="initialCapacity">The initial capacity of the pool.</param>
		/// <param name="preInit">A number of elements to create and add to the pool right now using <see cref="FactoryMethod"/>.</param>
		public MaxHeapPool(int maxCount, int initialCapacity, int preInit) : this(maxCount, initialCapacity)
		{
			for (int i = 0; i < preInit; i++)
				Enpool(FactoryMethod());
		}

		/// <param name="initialCapacity">The initial capacity of the pool.</param>
		/// <param name="preInit">A number of elements to create and add to the pool right now using <paramref name="factoryMethod"/>.</param>
		/// <param name="factoryMethod">A number of elements to add to the pool right now using <see cref="FactoryMethod"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="factoryMethod"/> is null.</exception>
		public MaxHeapPool(int maxCount, int initialCapacity, int preInit, Func<T> factoryMethod) : this(maxCount, initialCapacity) 
		{
			//validate
			if (factoryMethod == null)
				throw new ArgumentNullException(nameof(factoryMethod));

			//work
			FactoryMethod = factoryMethod;
			for (int i = 0; i < preInit; i++)
				Enpool(FactoryMethod());
		}

		#endregion
	}

	/// <summary>
	/// A pool that is backed by a <see cref="MinHeap{T}"/>.
	/// </summary>
	public class MinHeapPool<T> : ObjectPool<T>
	{
		/// <summary>
		/// Internal backing object. It is safe to manipulate this backing object, but do NOT change the Count.
		/// </summary>
		public MinHeap<T> MinHeap { get => (MinHeap<T>)PoolObject; }

		#region Constructors

		public MinHeapPool() : this(Infinite_Pool_Size, Default_Initial_Capacity) {}

		public MinHeapPool(int maxCount) : this(maxCount, Default_Initial_Capacity) {}

		/// <param name="initialCapacity">The initial capacity of the pool.</param>
		public MinHeapPool(int maxCount, int initialCapacity)
		{
			MaxCount = maxCount;

			int initialAmount = CalculateInitialCapacity(initialCapacity);
			var pool = new MinHeap<T>(initialAmount);
			ConfigurePool(pool.Push, pool.Pop, pool);
		}
		
		/// <param name="initialCapacity">The initial capacity of the pool.</param>
		/// <param name="preInit">A number of elements to create and add to the pool right now using <see cref="FactoryMethod"/>.</param>
		public MinHeapPool(int maxCount, int initialCapacity, int preInit) : this(maxCount, initialCapacity)
		{
			for (int i = 0; i < preInit; i++)
				Enpool(FactoryMethod());
		}

		/// <param name="initialCapacity">The initial capacity of the pool.</param>
		/// <param name="preInit">A number of elements to create and add to the pool right now using <paramref name="factoryMethod"/>.</param>
		/// <param name="factoryMethod">A number of elements to add to the pool right now using <see cref="FactoryMethod"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="factoryMethod"/> is null.</exception>
		public MinHeapPool(int maxCount, int initialCapacity, int preInit, Func<T> factoryMethod) : this(maxCount, initialCapacity) 
		{
			//validate
			if (factoryMethod == null)
				throw new ArgumentNullException(nameof(factoryMethod));

			//work
			FactoryMethod = factoryMethod;
			for (int i = 0; i < preInit; i++)
				Enpool(FactoryMethod());
		}

		#endregion
	}

	/// <summary>
	/// A pool that is backed by a <see cref="Queue{T}"/>.
	/// </summary>
	public class QueuePool<T> : ObjectPool<T>
	{
		/// <summary>
		/// Internal backing object. It is safe to manipulate this backing object, but do NOT change the Count.
		/// </summary>
		public Queue<T> Queue { get => (Queue<T>)PoolObject; }

		#region Constructors

		public QueuePool() : this(Infinite_Pool_Size, Default_Initial_Capacity) {}

		public QueuePool(int maxCount) : this(maxCount, Default_Initial_Capacity) {}

		/// <param name="initialCapacity">The initial capacity of the pool.</param>
		public QueuePool(int maxCount, int initialCapacity)
		{
			MaxCount = maxCount;

			int initialAmount = CalculateInitialCapacity(initialCapacity);
			var pool = new Queue<T>(initialAmount);
			ConfigurePool(pool.Enqueue, pool.Dequeue, pool);
		}
		
		/// <param name="initialCapacity">The initial capacity of the pool.</param>
		/// <param name="preInit">A number of elements to create and add to the pool right now using <see cref="FactoryMethod"/>.</param>
		public QueuePool(int maxCount, int initialCapacity, int preInit) : this(maxCount, initialCapacity)
		{
			for (int i = 0; i < preInit; i++)
				Enpool(FactoryMethod());
		}

		/// <param name="initialCapacity">The initial capacity of the pool.</param>
		/// <param name="preInit">A number of elements to create and add to the pool right now using <paramref name="factoryMethod"/>.</param>
		/// <param name="factoryMethod">A number of elements to add to the pool right now using <see cref="FactoryMethod"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="factoryMethod"/> is null.</exception>
		public QueuePool(int maxCount, int initialCapacity, int preInit, Func<T> factoryMethod) : this(maxCount, initialCapacity) 
		{
			//validate
			if (factoryMethod == null)
				throw new ArgumentNullException(nameof(factoryMethod));

			//work
			FactoryMethod = factoryMethod;
			for (int i = 0; i < preInit; i++)
				Enpool(FactoryMethod());
		}

		#endregion
	}

	/// <summary>
	/// A pool that is backed by a <see cref="Stack{T}"/>.
	/// </summary>
	/// <remarks>By default, <see cref="ObjectPool{T}"/> implements a <see cref="Stack{T}",
	///	so this class just strongly types that behaviour.</remarks>
	public class StackPool<T> : ObjectPool<T>
	{
		/// <summary>
		/// Internal backing object. It is safe to manipulate this backing object, but do NOT change the Count.
		/// </summary>
		public Stack<T> Stack { get => (Stack<T>)PoolObject; }

		#region Constructors

		public StackPool() : base() {}

		public StackPool(int maxCount) : base(maxCount) {}
		
		/// <param name="initialCapacity">The initial capacity of the pool.</param>
		public StackPool(int maxCount, int initialCapacity) : base(maxCount, initialCapacity) {}
		
		/// <param name="initialCapacity">The initial capacity of the pool.</param>
		/// <param name="preInit">A number of elements to create and add to the pool right now using <see cref="FactoryMethod"/>.</param>
		public StackPool(int maxCount, int initialCapacity, int preInit) : base(maxCount, initialCapacity, preInit) {}
		
		/// <param name="initialCapacity">The initial capacity of the pool.</param>
		/// <param name="preInit">A number of elements to create and add to the pool right now using <paramref name="factoryMethod"/>.</param>
		/// <param name="factoryMethod">A number of elements to add to the pool right now using <see cref="FactoryMethod"/>.</param>
		/// <exception cref="ArgumentNullException">Thrown if <paramref name="factoryMethod"/> is null.</exception>
		public StackPool(int maxCount, int initialCapacity, int preInit, Func<T> factoryMethod) : base(maxCount, initialCapacity, preInit, factoryMethod) {}

		#endregion
	}

	#endregion
}
