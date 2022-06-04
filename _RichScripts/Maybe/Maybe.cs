/* Source: https://github.com/vkhorikov/CSharpFunctionalExtensions/blob/master/CSharpFunctionalExtensions/Maybe/Maybe.cs
 */

using System;

namespace RichPackage
{
    /// <summary>
    /// Maybes imply the intent that their underlying values might actually be null.
    /// In this context, you can assume non-maybe reference types to be not-null.
    /// </summary>
    /// <typeparam name="T">Underlying, backing type.</typeparam>
    [Serializable]
    public struct Maybe<T> : IEquatable<Maybe<T>>, IMaybe<T>
    {
        private readonly bool _isValueSet;

        private readonly T _value;

        /// <summary>
        /// Returns the inner value if there's one, otherwise throws an InvalidOperationException with <paramref name="errorMessage"/>
        /// </summary>
        /// <exception cref="InvalidOperationException">Maybe has no value.</exception>
        public T GetValueOrThrow(string errorMessage = null)
        {
            if (HasNoValue)
                throw new InvalidOperationException(errorMessage
                    ?? $"{nameof(Maybe)} has no value.");

            return _value;
        }

        public T GetValueOrDefault(T defaultValue = default)
        {
            if (HasNoValue)
                return defaultValue;

            return _value;
        }

        /// <summary>
        /// Try to use GetValueOrThrow() or GetValueOrDefault() instead for better explicitness.
        /// </summary>
        public T Value => GetValueOrThrow();

        public static Maybe<T> None => new Maybe<T>();

        /// <summary>
        /// The underlying value is non-null.
        /// </summary>
        public bool HasValue => _isValueSet;

        /// <summary>
        /// The underlying value is null.
        /// </summary>
        public bool HasNoValue => !_isValueSet;

        private Maybe(T value)
        {
            if (value == null)
            {
                _isValueSet = false;
                _value = default;
            }
			else
            {
                _isValueSet = true;
                _value = value;
            }
        }

        public static implicit operator Maybe<T>(T value)
        {
            return (value is Maybe<T> maybeValue) ? maybeValue : new Maybe<T>(value);
        }

        public static implicit operator Maybe<T>(Maybe _) => None;

        public static Maybe<T> From(T obj)
        {
            return new Maybe<T>(obj);
        }

        public static bool operator ==(Maybe<T> maybe, T value)
        {
            return (value is Maybe<T> valueMaybe && maybe.Equals(valueMaybe))
                || (maybe.HasNoValue && value == null)
                || maybe._value.Equals(value);
        }

        public static bool operator !=(Maybe<T> maybe, T value)
        {
            return !(maybe == value);
        }

        public static bool operator ==(Maybe<T> first, Maybe<T> second)
        {
            return first.Equals(second);
        }

        public static bool operator !=(Maybe<T> first, Maybe<T> second)
        {
            return !(first == second);
        }

        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;

            //TODO - remove reflection in favor of 'is'
            if (obj.GetType() != typeof(Maybe<T>))
            {
                if (obj is T objT)
                {
                    obj = new Maybe<T>(objT);
                }

                if (!(obj is Maybe<T>))
                    return false;
            }

            var other = (Maybe<T>)obj;
            return Equals(other);
        }

        public bool Equals(Maybe<T> other)
        {
            return (HasNoValue && other.HasNoValue) //both are null
                || (HasValue && other.HasValue //both have values 
                    && _value.Equals(other._value)); //and those values are equal
        }

        public override int GetHashCode()
        {
            if (HasNoValue)
                return 0;

            return _value.GetHashCode();
        }

        public override string ToString()
        {
            if (HasNoValue)
                return "No value";

            return _value.ToString();
        }
    }

    /// <summary>
    /// Non-generic entrypoint for <see cref="Maybe{T}" /> members
    /// </summary>
    public struct Maybe
    {
        public static Maybe None => new Maybe();

        /// <summary>
        /// Creates a new <see cref="Maybe{T}" /> from the provided <paramref name="value"/>
        /// </summary>
        public static Maybe<T> From<T>(T value) => Maybe<T>.From(value);
    }
}
