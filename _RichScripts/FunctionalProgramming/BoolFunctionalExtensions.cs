using System.Runtime.CompilerServices;

namespace RichPackage.FunctionalProgramming
{
    public static class BoolFunctionalExtensions
    {
        /// <summary>
        /// Functional notation for checking truth value.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsTrue(this bool b) => b;

        /// <summary>
        /// Functional notation for checking truth value.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsFalse(this bool b) => !b;
    }

    public static class FloatFunctionalExtensions
    {
        public static float Multiply(this float x, float rhs) => x * rhs;
        public static float Add(this float x, float rhs) => x + rhs;
        public static float Divide(this float x, float rhs) => x / rhs;
    }
}
