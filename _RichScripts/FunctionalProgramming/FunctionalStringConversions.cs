using System.Runtime.CompilerServices;

namespace RichPackage.FunctionalProgramming
{
    public static class FunctionalStringConversions
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ToInt(this string value) => int.Parse(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static long ToLong(this string value) => long.Parse(value);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ToFloat(this string value) => float.Parse(value);
    }
}
