using System;
using System.Runtime.CompilerServices;

namespace RichPackage
{
    public static class String_Extensions
    {
        private const string DEFAULT_ARGUMENT_NAME = "Argument";

        /// <summary>
        /// More performant than <see cref="string.IsNullOrEmpty"/> because it's branchless.
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullOrEmpty(this string str)
            => str == null || str.Length == 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNullOrEmptyOrWhiteSpace(this string str)
            => IsNullOrEmpty(str) || IsWhiteSpace(str);

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsWhiteSpace(this string str)
        {
            int len = str.Length; //cache for re-use
            for (int i = 0; i < len; i++)
                if (!char.IsWhiteSpace(str[i]))
                    return false;
            return true;
        }

        #region Throws
       
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNullOrEmpty(this string str, string argumentName = DEFAULT_ARGUMENT_NAME)
        {
            if (str.IsNullOrEmpty())
                throw new ArgumentException($"{argumentName} is null or empty.");
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ThrowIfNullOrEmptyOrWhiteSpace(this string str, string argumentName = DEFAULT_ARGUMENT_NAME)
        {
            if (str.IsNullOrEmptyOrWhiteSpace())
                throw new ArgumentException($"{argumentName} is null or empty or whitespace.");
        }

        #endregion
    }
}
