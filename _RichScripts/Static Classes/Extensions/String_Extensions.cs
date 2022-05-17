using System;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace RichPackage
{
    public static class String_Extensions
    {
        public static readonly Regex sWhitespace = new Regex(@"\s+");

        /// <summary>
        /// If the size of a string is larger than this amount, it's probably better to use a 
        /// <see cref="Regex"/>. Arbitrarily chosen; please tune.
        /// </summary>
        private const int REGEX_SIZE_LIMIT = 5000;

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

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IndexIsInRange(this string str, int index)
            => index >= 0 && index < str.Length;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string RemoveWhitespace(this string str)
        {
            string output;

            if (str.Length > REGEX_SIZE_LIMIT)
                output = sWhitespace.Replace(str, string.Empty);
            else 
                output = string.Join(string.Empty, str.Split(default(string[]),
                    StringSplitOptions.RemoveEmptyEntries));

            return output;
        }
        
        public static string RemoveTrailingWhitespace(this string str)
        {
            //validate
            if (str.IsNullOrEmpty())
                return str;

            //work
            int len = str.Length;
            int endingIndex = len - 1;

            while (str.IndexIsInRange(endingIndex)
                && char.IsWhiteSpace(str[endingIndex]))
                --endingIndex;

            if (endingIndex == len - 1)
                return str; //no trailing whitespace
            else
                return str.Substring(0, endingIndex + 1);
        }

        public static string RemoveLeadingWhitespace(this string str)
        {
            //validate
            if (str.IsNullOrEmpty())
                return str;

            //work
            int startingIndex = 0;

            while (str.IndexIsInRange(startingIndex)
                && char.IsWhiteSpace(str[startingIndex]))
                ++startingIndex;

            if (startingIndex == 0)
                return str;
            else
                return str.Substring(startingIndex);
        }
    }
}
