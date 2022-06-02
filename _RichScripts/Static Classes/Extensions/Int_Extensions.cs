using System.Runtime.CompilerServices;

namespace RichPackage
{
	public static class Int_Extensions
	{
		/// <summary>
		/// Returns true if i is in [min, max].
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsWithin(this int i, int min, int max)
			=> i >= min && i <= max;

		/// <summary>
		/// Returns true if i is in [0, max].
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static bool IsWithin0(this int i, int max)
			=> IsWithin(i, 0, max);

		/// <summary>
		/// Functional notation for <paramref name="lhs"/> + <paramref name="rhs"/>.
		/// </summary>
		/// <returns><paramref name="lhs"/> + <paramref name="rhs"/></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Add(this int lhs, int rhs)
			=> lhs + rhs;

		/// <summary>
		/// Functional notation for <paramref name="lhs"/> - <paramref name="rhs"/>.
		/// </summary>
		/// <returns><paramref name="lhs"/> - <paramref name="rhs"/></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Sub(this int lhs, int rhs)
			=> lhs - rhs;

		/// <summary>
		/// Functional notation for <paramref name="lhs"/> ^ <paramref name="rhs"/>.
		/// </summary>
		/// <returns><paramref name="lhs"/> ^ <paramref name="rhs"/></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Xor(this int lhs, int rhs)
			=> lhs ^ rhs;
			
		/// <summary>
		/// Functional notation for <paramref name="lhs"/> !^ <paramref name="rhs"/>.
		/// </summary>
		/// <returns><paramref name="lhs"/> !^ <paramref name="rhs"/></returns>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		public static int Xnor(this int lhs, int rhs)
			=> ~(lhs ^ rhs);
	}
}
