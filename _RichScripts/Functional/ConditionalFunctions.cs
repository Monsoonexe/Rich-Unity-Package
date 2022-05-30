using System;
using System.Runtime.CompilerServices;

namespace RichPackage.FunctionalProgramming
{
	/// <summary>
	/// Transform imperative control contstructs into functional constructs.
	/// </summary>
	public static class ConditionalFunctions
	{
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T If<T>(bool condition, T a, T b)
			=> condition ? a : b;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T If<T>(bool condition, Func<T> funcA, Func<T> funcB)
			=> condition ? funcA() : funcB();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T If<T>(bool condition, T a, Func<T> funcB)
			=> condition ? a : funcB();

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static T If<T>(bool condition, Func<T> funcA, T b)
			=> condition ? funcA() : b;

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void If(bool condition, Action actionA, Action actionB)
		{
			if (condition)
				actionA();
			else
				actionB();
		}

		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static void IfElse(params ConditionalStruct[] phrases)
		{
			foreach (var phrase in phrases)
			{
				if (phrase.condition())
				{
					phrase.action();
					return;
				}
			}
		}

		public class ConditionalStruct
		{
			public Func<bool> condition;
			public Action action;

			public ConditionalStruct((Func<bool>, Action) a)
				: this(a.Item1, a.Item2)
			{
				//nada
			}

			public ConditionalStruct(Tuple<Func<bool>, Action> a)
				: this(a.Item1, a.Item2)
			{
				//nada
			}

			public ConditionalStruct(Func<bool> condition, Action action)
			{
				this.condition = condition;
				this.action = action;
			}

			public static implicit operator (Func<bool>, Action)(ConditionalStruct s)
				=> (s.condition, s.action);

			public static implicit operator ConditionalStruct(Tuple<Func<bool>, Action> t)
				=> new ConditionalStruct(t.Item1, t.Item2);
		}
	}
}
