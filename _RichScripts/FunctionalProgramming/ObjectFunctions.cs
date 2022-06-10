using System.Runtime.CompilerServices;

namespace RichPackage.FunctionalProgramming
{
    internal static class ObjectFunctions
    {
        /// <summary>
        /// Functional notation for `obj == null`
        /// </summary>
        /// <returns><see langword="true"/> if the object is <see langword="null"/>,
        /// otherwise <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNull(this object obj) => obj == null;

        /// <summary>
        /// Functional notation for `obj != null`
        /// </summary>
        /// <returns><see langword="true"/> if the object is not <see langword="null"/>,
        /// otherwise <see langword="false"/>.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNotNull(this object obj) => obj != null;
    }
}
