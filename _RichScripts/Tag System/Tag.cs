using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RichPackage.TagSystem
{
    [Serializable]
    public class Tag : IEquatable<Tag>, IEquatable<string>
    {
        /// <summary>
        /// Empty array of tags. None.
        /// </summary>
        public static Tag[] None => Array.Empty<Tag>();

        /// <summary>
        /// The property name, such as "Clothing".
        /// </summary>
        [field: SerializeField, LabelText(nameof(Property))]
        public string Property { get; private set; }

        /// <summary>
        /// The value of the tag, such as "White".
        /// </summary>
        [field: SerializeField, LabelText(nameof(Value))]
        public string Value { get; private set; }

        /// <summary>
        /// Does the <see cref="Property"/> contain a value?
        /// </summary>
        public bool HasProperty => !string.IsNullOrEmpty(Property);

        /// <summary>
        /// Does the <see cref="Value"/> contain a value?
        /// </summary>
        public bool HasValue => !string.IsNullOrEmpty(Value);

        #region Constructors

        public Tag(string property) : this(property, string.Empty) { }

        public Tag(string property, string value)
        {
            Property = property;
            Value = value;
        }

        #endregion Constructors

        /// <summary>
        /// Tags are equal to each other if both of their properties are equal.
        /// </summary>
        public bool Equals(Tag other)
        {
            if (other is null)
                return false;

            return MatchProperty(other.Property) && MatchValue(other.Value);
        }

        public override bool Equals(object obj)
        {
            return obj is Tag other && Equals(other);
        }

        public bool Equals(string other)
        {
            return !HasValue && MatchProperty(other);
        }

        public override int GetHashCode()
        {
            int hashCode = -1027930222;
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(Property);
            hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(Value);
            return hashCode;
        }

        public override string ToString()
        {
            return HasValue
                ? $"{{'{Property}':'{Value}'}}"
                : Property;
        }

        #region Querries

        public bool MatchProperty(string query)
            => Property.EqualsOrdinal(query);

        public bool MatchValue(string query)
            => Value.EqualsOrdinal(query);

        public bool Match(string propertyQuery, string valueQuery)
            => MatchProperty(propertyQuery) && MatchValue(valueQuery);

        #endregion Querries

        public static bool operator ==(Tag a, Tag b)
        {
            if (ReferenceEquals(a, b))
                return true;
            if (a is null)
                return false;
            return a.Equals(b);
        }
        public static bool operator !=(Tag a, Tag b) => !(a == b);

        public static implicit operator string(Tag t) => t?.Property ?? string.Empty;
        public static implicit operator Tag(string s) => new Tag(s ?? string.Empty);
    }
}
