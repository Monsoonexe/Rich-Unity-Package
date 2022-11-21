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

        #region Comparisons

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EqualsOrdinal(this string strA, string strB)
            => string.CompareOrdinal(strA, strB) == 0;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool EqualsOrdinalIgnoreCase(this string strA, string strB)
            => string.Compare(strA, strB, StringComparison.OrdinalIgnoreCase) == 0;

        /// <summary>
        /// 
        er than <see cref="String.EndsWith(string)"/>. Prefer this for 
        /// non-localized strings because it compares the byte value rather than 
        /// the character represented by the value. 
        /// </summary>
        /// <remarks>https://docs.unity3d.com/Manual/BestPracticeUnderstandingPerformanceInUnity5.html#:~:text=Inefficient%20built%2Din%20string%20APIs</remarks>
        public static bool QuickEndsWith(this string a, string b)
        {
            int ap = a.Length - 1;
            int bp = b.Length - 1;

            while (ap >= 0 && bp >= 0 && a[ap] == b[bp])
            {
                ap--;
                bp--;
            }

            return (bp < 0);
        }

        /// <summary>
        /// Quicker than <see cref="String.StartsWith(string)"/>. Prefer this for 
        /// non-localized strings because it compares the byte value rather than 
        /// the character represented by the value. 
        /// </summary>
        /// <remarks>https://docs.unity3d.com/Manual/BestPracticeUnderstandingPerformanceInUnity5.html#:~:text=Inefficient%20built%2Din%20string%20APIs</remarks>
        public static bool QuickStartsWith(this string a, string b)
        {
            int aLen = a.Length;
            int bLen = b.Length;

            int ap = 0;
            int bp = 0;

            while (ap < aLen && bp < bLen && a[ap] == b[bp])
            {
                ap++;
                bp++;
            }

            return (bp == bLen);
        }

        #endregion Comparisons
            
        /// <summary>
        /// Splits <paramref name="source"/> into chunks of size <paramref name="chunkLength"/>.
        /// </summary>
        /// <returns>An iterator over each chunk</returns>
        /// <remarks>https://stackoverflow.com/questions/1450774/splitting-a-string-into-chunks-of-a-certain-size</remarks>
        public static IEnumerable<string> SplitBy(this string source, int chunkLength)
        {
            GuardClauses.GuardAgainst.IsNullOrWhiteSpace(source, nameof(source));
            GuardClauses.GuardAgainst.IsZeroOrNegative(chunkLength, nameof(chunkLength));

            int len = source.Length;
            for (int i = 0; i < len; i += chunkLength)
            {
                // handle remainder
                if (chunkLength + i > len)
                    chunkLength = len - i;

                // yield chunk
                yield return source.Substring(i, chunkLength);
            }
        }

        /// <summary>
        /// Returns <paramref name="str"/>'s <see cref="string.Length"/>
        /// property or '0' if it is <see langword="null"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int SafeLength(this string str)
            => str?.Length ?? 0;
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static char First(this string str) => str[0];

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static char FirstOrDefault(this string str, char defaultValue = '\0')
            => str.Length > 0 ? str[0] : defaultValue;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static char Last(this string str) => str[str.Length - 1];

        #region Functional Conversions

        /// <summary>
        /// Returns <see langword="true"/> if and only if <paramref name="source"/>
        /// is the string 'true' (ignoring case).
        /// </summary>
        public static bool ToBool(this string source)
            => source.EqualsOrdinalIgnoreCase(bool.TrueString);

        /// <summary>
        /// Shorthand for <see cref="float.Parse(string)"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static float ToFloat(this string source)
            => float.Parse(source);

        /// <summary>
        /// Shorthand for <see cref="int.Parse(string)"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int ToInt(this string source)
            => int.Parse(source);

        /// <summary>
        /// Case insensitive check. True iff source == "true".
        /// </summary>
        /// <param name="source">Source string.</param>
        /// <param name="result">Result is invalid if 'false' is returned.</param>
        /// <returns>True if parse was successful. Result is invalid if 'false' is returned.</returns>
        public static bool TryToBool(this string source, out bool result)
        {
            // result is true iff equal to 'true'.
            result = source.EqualsOrdinalIgnoreCase(bool.TrueString);

            // success is true iff equal to 'true' or 'false'
            bool success = result
                || source.EqualsOrdinalIgnoreCase(bool.FalseString);

            return success;
        }

        #endregion Functional Conversions

        /// <summary>
        /// Returns a new <see cref="string"/> with all the <see cref="char"/>s reversed.
        /// Results may vary when operating on strings with <see cref="System.Text.Encoding.Unicode"/> 
        /// (UTF-16) characters.
        /// </summary>
        /// <returns>A new <see cref="String"/> with all the <see cref="char"/>s reversed.</returns>
        public static string ReverseUTF8(this string source)
        {
            if (source is null)
                return null;

            char[] arr = source.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Remove(this string source, string query)
            => source.Replace(query, string.Empty);


    }
}
