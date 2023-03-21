using System;
using System.Collections.Generic;

namespace RichPackage.Events
{
	/// <summary>
	/// Base class for a performant and memory-lite event handler.
	/// </summary>
	/// <remarks>This class was designed to work with <see cref="Action"/>.
	/// Inheritors must define their own "Invoke" method.</remarks>
	public abstract class AEventHandlerList<TDelegate>
		where TDelegate : class // can't use Action as constraint :(
	{
		// stretch goal -- manually implement a resizeable array
		protected readonly List<TDelegate> actions;

		#region Constructors

		protected AEventHandlerList() : this(0) { }

		protected AEventHandlerList(int capacity)
		{
			actions = new List<TDelegate>(capacity);
		}

		#endregion Constructors

		public void Add(TDelegate action)
		{
			actions.Add(action);
		}

		public void Remove(TDelegate action)
		{
			// event systems depending on order is an anti-pattern, so use quick remove
			actions.QuickRemove(action);
		}

		public void RemoveAll()
		{
			actions.Clear();
		}

		public void AddUnique(TDelegate action)
		{
			actions.AddIfNew(action);
		}
	}

	public sealed class EventHandlerList : AEventHandlerList<Action>
	{
		#region Constructors

		public EventHandlerList() : this(0) { }

		public EventHandlerList(int capacity) : base(capacity) { }
		
		#endregion Constructors

		public void Invoke()
		{
			// Reversal allows self-removal during dispatch (doesn't skip next listener)
			// Reversal allows safe addition during dispatch (doesn't fire immediately)
			for (int i = actions.Count - 1; i >= 0; --i)
			{
				actions[i]();
			}
		}
	}

	public sealed class EventHandlerList<TArg> : AEventHandlerList<Action<TArg>>
	{
		#region Constructors

		public EventHandlerList() : this(0) { }

		public EventHandlerList(int capacity) : base(capacity) { }

		#endregion Constructors

		public void Invoke(TArg arg)
		{
			// Reversal allows self-removal during dispatch (doesn't skip next listener)
			// Reversal allows safe addition during dispatch (doesn't fire immediately)
			for (int i = actions.Count - 1; i >= 0; --i)
			{
				actions[i](arg);
			}
		}
	}

	public sealed class EventHandlerList<TArg1, TArg2> : AEventHandlerList<Action<TArg1, TArg2>>
	{
		#region Constructors

		public EventHandlerList() : this(0) { }

		public EventHandlerList(int capacity) : base(capacity) { }

		#endregion Constructors

		public void Invoke(TArg1 arg1, TArg2 arg2)
		{
			// Reversal allows self-removal during dispatch (doesn't skip next listener)
			// Reversal allows safe addition during dispatch (doesn't fire immediately)
			for (int i = actions.Count - 1; i >= 0; --i)
			{
				actions[i](arg1, arg2);
			}
		}
	}

	public sealed class EventHandlerList<TArg1, TArg2, TArg3> : AEventHandlerList<Action<TArg1, TArg2, TArg3>>
	{
		#region Constructors

		public EventHandlerList() : this(0) { }

		public EventHandlerList(int capacity) : base(capacity) { }

		#endregion Constructors

		public void Invoke(TArg1 arg1, TArg2 arg2, TArg3 arg3)
		{
			// Reversal allows self-removal during dispatch (doesn't skip next listener)
			// Reversal allows safe addition during dispatch (doesn't fire immediately)
			for (int i = actions.Count - 1; i >= 0; --i)
			{
				actions[i](arg1, arg2, arg3);
			}
		}
	}
}
