using System.Runtime.CompilerServices;

namespace RichPackage
{
    public static class Bool_Extensions
    {
        //for situations where bools just don't cut it
        public const int TRUE_int = 1;
        public const int FALSE_int = 0;

        /// <summary>
        /// A && B in function form.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool And(this bool a, bool b) => a && b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Nand(this bool a, bool b) => !(a && b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Nor(this bool a, bool b) => !(a || b);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Not(this bool a) => !a;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Or(this bool a, bool b) => a || b;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Xnor(this bool a, bool b) => a == b;
            //!((a && !b) || (!a && b));

        /// <summary>
        /// One or the other, but not both. Returns true if a and b are different truth values.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Xor(this bool a, bool b) => a != b;
            //(a && !b) || (!a && b)

        /// <summary>
        /// Shortcut for !a;
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Negate(this bool a) => !a;

        /// <summary>
        /// Shortcut for a = !a;
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool Toggle(this ref bool a) => a = !a;

    }
}
