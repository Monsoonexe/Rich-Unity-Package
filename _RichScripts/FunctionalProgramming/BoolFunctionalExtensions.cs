using System.Runtime.CompilerServices;

namespace ApexCommon.FunctionalProgramming
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
}
