using System.Runtime.CompilerServices;

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
}
