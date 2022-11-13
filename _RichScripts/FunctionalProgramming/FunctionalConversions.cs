using System.Runtime.CompilerServices;

namespace RichPackage.FunctionalProgramming
{
    /// <summary>
    /// Methods to explicitly convert primitives to <see langword="int"/>.
    /// </summary>
    public static class FunctionalIntegerConversions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ToInt(this byte value) => (int)value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ToInt(this short value) => (int)value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ToInt(this ushort value) => (int)value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ToInt(this long value) => (int)value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ToInt(this ulong value) => (int)value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ToInt(this float value) => (int)value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ToInt(this double value) => (int)value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ToInt(this System.Enum value)
            => value.GetHashCode();
    }

    /// <summary>
    /// Methods to explicitly convert primitives to <see langword="float"/>.
    /// </summary>
    public static class FunctionalFloatConversions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ToFloat(this byte value) => (float)value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ToFloat(this short value) => (float)value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ToFloat(this ushort value) => (float)value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ToFloat(this int value) => (float)value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ToFloat(this uint value) => (float)value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ToFloat(this long value) => (float)value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ToFloat(this ulong value) => (float)value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ToFloat(this double value) => (float)value;
    }

    /// <summary>
    /// Methods to explicitly convert primitives to <see langword="double"/>.
    /// </summary>
    public static class FunctionalDoubleConversions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ToDouble(this long value) => (double)value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static double ToDouble(this ulong value) => (double)value;
    }

    /// <summary>
    /// Methods to explicitly convert primitives to <see langword="byte"/>.
    /// </summary>
    public static class FunctionalByteConversions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ToByte(this char value) => (byte)value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ToByte(this int value) => (byte)value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ToByte(this uint value) => (byte)value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ToByte(this long value) => (byte)value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ToByte(this ulong value) => (byte)value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte ToByte(this System.Enum value)
            => (byte)value.ToInt();
    }

    /// <summary>
    /// Methods to explicitly convert primitives to <see langword="char"/>.
    /// </summary>
    public static class FunctionalCharConversions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static char ToChar(this byte value) => (char)value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static char ToChar(this int value) => (char)value;
    }
}
