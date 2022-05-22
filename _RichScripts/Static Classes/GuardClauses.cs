using System;
using System.Collections;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace RichPackage.GuardClauses
{
    /// <summary>
    /// Throw exceptions if the guard condition is true.
    /// </summary>
    public static class GuardAgainst
    {
        // https://devblogs.microsoft.com/csharpfaq/what-is-the-difference-between-const-and-static-readonly/
        private const string VALID_EMAIL_ADDRESS_PATTERN =
            (@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$");
        //https://regex101.com/r/8M0MqB/1
        //https://stackoverflow.com/questions/6038061/regular-expression-to-find-urls-within-a-string
        private const string VALID_URL_PATTERN =
            @"([\w+]+\:\/\/)?([\w\d-]+\.)*[\w-]+[\.\:]\w+([\/\?\=\&\#.]?[\w-]+)*\/?";

        private const string DEFAULT_PARAM_NAME = "argument";

        //Should add the Compiled option?
        private static readonly Regex emailRegex = new Regex(VALID_EMAIL_ADDRESS_PATTERN,
                RegexOptions.IgnoreCase);
        private static readonly Regex urlRegex = new Regex(VALID_URL_PATTERN,
                RegexOptions.IgnoreCase);

        #region Boolean

        /// <summary>
        /// Will thow an <see cref="ArgumentException"/> if the 
        /// <paramref name="condition"/> is <see langword="true"/>.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsTrue(bool condition,
            string paramName = DEFAULT_PARAM_NAME)
        {
            if (condition)
                throw new ArgumentException($"{paramName} is true but should not be.");
        }

        /// <summary>
        /// Will thow an <see cref="ArgumentException"/> if the 
        /// <paramref name="condition"/> is <see langword="false"/>.
        /// </summary>
        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsFalse(bool condition,
            string paramName = DEFAULT_PARAM_NAME)
        {
            if (!condition)
                throw new ArgumentException($"{paramName} is false but should not be.");
        }

        #endregion

        #region Arithmetic

        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsZero(int argumentValue,
            string paramName = DEFAULT_PARAM_NAME)
        {
            if (argumentValue == 0)
                throw new ArgumentException($"{paramName} is zero.");
        }

        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsZero(long argumentValue,
            string paramName = DEFAULT_PARAM_NAME)
        {
            if (argumentValue == 0)
                throw new ArgumentException($"{paramName} is zero.");
        }

        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsZero(decimal argumentValue,
            string paramName = DEFAULT_PARAM_NAME)
        {
            if (argumentValue == 0)
                throw new ArgumentException($"{paramName} is zero.");
        }

        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsZero(double argumentValue,
            string paramName = DEFAULT_PARAM_NAME)
        {
            if (argumentValue == 0)
                throw new ArgumentException($"{paramName} is zero.");
        }

        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsZero(float argumentValue,
            string paramName = DEFAULT_PARAM_NAME)
        {
            if (argumentValue == 0)
                throw new ArgumentException($"{paramName} is zero.");
        }

        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsNegative(int argumentValue,
            string paramName = DEFAULT_PARAM_NAME)
        {
            if (argumentValue < 0)
                throw new ArgumentException($"{paramName} is negative number.");
        }

        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsNegative(long argumentValue,
            string paramName = DEFAULT_PARAM_NAME)
        {
            if (argumentValue < 0)
                throw new ArgumentException($"{paramName} is negative number.");
        }

        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsNegative(decimal argumentValue,
            string paramName = DEFAULT_PARAM_NAME)
        {
            if (argumentValue < 0)
                throw new ArgumentException($"{paramName} is negative number.");
        }

        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsNegative(double argumentValue,
            string paramName = DEFAULT_PARAM_NAME)
        {
            if (argumentValue < 0)
                throw new ArgumentException($"{paramName} is negative number.");
        }

        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsNegative(float argumentValue,
            string paramName = DEFAULT_PARAM_NAME)
        {
            if (argumentValue < 0)
                throw new ArgumentException($"{paramName} is 0 or negative number.");
        }

        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsPositive(int argumentValue,
            string paramName = DEFAULT_PARAM_NAME)
        {
            if (argumentValue > 0)
                throw new ArgumentException($"{paramName} is positive number.");
        }

        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsPositive(long argumentValue,
            string paramName = DEFAULT_PARAM_NAME)
        {
            if (argumentValue > 0)
                throw new ArgumentException($"{paramName} is positive number.");
        }

        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsPositive(decimal argumentValue,
            string paramName = DEFAULT_PARAM_NAME)
        {
            if (argumentValue > 0)
                throw new ArgumentException($"{paramName} is positive number.");
        }

        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsPositive(float argumentValue,
            string paramName = DEFAULT_PARAM_NAME)
        {
            if (argumentValue > 0)
                throw new ArgumentException($"{paramName} is positive number.");
        }

        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsPositive(double argumentValue,
            string paramName = DEFAULT_PARAM_NAME)
        {
            if (argumentValue > 0)
                throw new ArgumentException($"{paramName} is positive number.");
        }

        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsZeroOrNegative(int argumentValue,
            string paramName = DEFAULT_PARAM_NAME)
        {
            if (argumentValue <= 0)
                throw new ArgumentException($"{paramName} is 0 or negative number.");
        }

        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsZeroOrNegative(long argumentValue,
            string paramName = DEFAULT_PARAM_NAME)
        {
            if (argumentValue <= 0)
                throw new ArgumentException($"{paramName} is 0 or negative number.");
        }

        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsZeroOrNegative(decimal argumentValue,
            string paramName = DEFAULT_PARAM_NAME)
        {
            if (argumentValue <= 0)
                throw new ArgumentException($"{paramName} is 0 or negative number.");
        }

        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsZeroOrNegative(double argumentValue,
            string paramName = DEFAULT_PARAM_NAME)
        {
            if (argumentValue <= 0)
                throw new ArgumentException($"{paramName} is 0 or negative number.");
        }

        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsZeroOrNegative(float argumentValue,
            string paramName = DEFAULT_PARAM_NAME)
        {
            if (argumentValue <= 0)
                throw new ArgumentException($"{paramName} is 0 or negative number.");
        }

        #endregion

        #region Equivalence

        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsEqualTo<T>(T argumentValue, T guardValue,
            string paramName = DEFAULT_PARAM_NAME)
            where T : IEquatable<T>
        {
            if (argumentValue.Equals(guardValue))
                throw new ArgumentException($"{paramName}: " +
                    $"{argumentValue} is equal to {guardValue}.");
        }

        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsNotEqualTo<T>(T argumentValue, T guardValue,
            string paramName = DEFAULT_PARAM_NAME)
            where T : IEquatable<T>
        {
            if (!argumentValue.Equals(guardValue))
                throw new ArgumentException($"{paramName}: " +
                    $"{argumentValue} is not equal to {guardValue}.");
        }

        #endregion

        #region Comparison

        //TODO - moar

        /// <exception cref="ArgumentOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsOutOfRange<T>(T argumentValue,
            T min, T max, string paramName = DEFAULT_PARAM_NAME)
            where T : IComparable<T>
        {
            if (argumentValue.CompareTo(min) < 0
                || argumentValue.CompareTo(max) > 0)
                throw new ArgumentOutOfRangeException(paramName, argumentValue,
                    $"Range: [{min}, {max}]");
        }

        /// <exception cref="IndexOutOfRangeException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IndexOutOfRange(IList list, int index,
            string paramName = DEFAULT_PARAM_NAME)
        {
            if (index < 0 || index >= list.Count)
                throw new IndexOutOfRangeException(
                    $"{paramName} <{index}> is out of range: [0, {list.Count})");
        }

        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsGreaterThan<T> (T argumentValue, 
            T compareValue, string paramName = DEFAULT_PARAM_NAME)
            where T : IComparable<T>
        {
            if (argumentValue.CompareTo(compareValue) > 0)
                throw new ArgumentException(paramName, $"{argumentValue} > {compareValue}");
        }

        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsLessThan<T>(T argumentValue,
            T compareValue, string paramName = DEFAULT_PARAM_NAME)
            where T : IComparable<T>
        {
            if (argumentValue.CompareTo(compareValue) < 0)
                throw new ArgumentException(paramName, $"{argumentValue} < {compareValue}");
        }

        #endregion

        #region Collections

        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsLengthExceeded(ICollection argumentValue,
            int maximumLength, string paramName = DEFAULT_PARAM_NAME)
        {
            int count = argumentValue.Count;
            if (count > maximumLength)
                throw new ArgumentException($"{paramName} " +
                    $"({count}) has exceeded maximum number " +
                    $"of characters: {maximumLength}.");
        }

        #endregion

        #region Strings

        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LengthMoreThan(string argumentValue,
            int maximumLength, string paramName = DEFAULT_PARAM_NAME)
        {
            int count = argumentValue.Length;
            if (count > maximumLength)
                throw new ArgumentException($"{paramName} " +
                    $"({argumentValue}: {count})" +
                    $" has too many characters: {maximumLength}.");
        }

        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LengthLessThan(string argumentValue,
            int minimumLength, string paramName = DEFAULT_PARAM_NAME)
        {
            int count = argumentValue.Length;
            if (count < minimumLength)
                throw new ArgumentException($"{paramName} " +
                    $"({argumentValue}: {count})" +
                    $" does not have enough characters: {minimumLength}.");
        }

        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void LengthIsNotExactly(string argumentValue,
            int mandatedLength, string paramName = DEFAULT_PARAM_NAME)
        {
            if (argumentValue.Length != mandatedLength)
            {
                throw new ArgumentException($"{paramName}: {argumentValue.Length} " +
                    $"is not exactly the correct length: {mandatedLength}.");
            }
        }

        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsNullOrEmptyOrWhiteSpace(string argumentValue,
            string paramName = DEFAULT_PARAM_NAME)
        {
            IsNullOrWhiteSpace(argumentValue, paramName);
            IsNullOrEmpty(argumentValue, paramName);
        }

        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsNullOrWhiteSpace(string argumentValue,
            string paramName = DEFAULT_PARAM_NAME)
        {
            if (string.IsNullOrWhiteSpace(argumentValue))
                throw new ArgumentException($"{paramName} is null or white space.");
        }

        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsNullOrEmpty(string argumentValue,
            string paramName = DEFAULT_PARAM_NAME)
        {
            if (argumentValue == null || argumentValue.Length == 0)
                throw new ArgumentException($"{paramName} is null or empty string.");
        }

        #endregion //strings

        /// <exception cref="ArgumentNullException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ArgumentIsNull(object value,
            string paramName = DEFAULT_PARAM_NAME)
        {
            if (value == null)
                throw new ArgumentNullException($"{paramName} is null object.");
        }

        /// <exception cref="ArgumentNullException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void ArgumentIsNull(object value,
            string paramName, string message)
        {
            if (value == null)
                throw new ArgumentNullException($"{paramName} is null object. "
                    + message);
        }

        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsStringEmptyGuid(string argumentValue, 
            string paramName = DEFAULT_PARAM_NAME)
        {
            if (argumentValue == Guid.Empty.ToString())
                throw new ArgumentException($"{paramName} " +
                    $"cannot be string with value of empty guid.");
        }

        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void IsEmptyGuid(Guid argumentValue,
            string paramName = DEFAULT_PARAM_NAME)
        {
            if (argumentValue == Guid.Empty)
                throw new ArgumentException($"{paramName} " +
                    $"cannot be empty guid.");
        }

        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InvalidEmail(string email)
        {
            if (!emailRegex.IsMatch(email))
                throw new ArgumentException($"{email} is not a valid email.");
        }

        /// <exception cref="ArgumentException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void InvalidURL(string url)
        {
            if (!urlRegex.IsMatch(url))
                throw new ArgumentException($"{url} is not a valid URL.");
        }

        #region File IO

        /// <exception cref="FileNotFoundException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FileDoesNotExist(string filePath)
        {
            if (!File.Exists(filePath))
                throw new FileNotFoundException("The resource " +
                    $"at <{filePath}> does not exist.");
        }

        /// <exception cref="FileNotFoundException"></exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void FileAlreadyExists(string filePath)
        {
            if (File.Exists(filePath))
                throw new ArgumentException("The resource " +
                    $"at <{filePath}> already exists.");
        }

        #endregion
    }
}
