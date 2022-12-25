using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace RichPackage.TagSystem
{
    [Serializable]
    public class Tag : IEquatable<Tag>
    {
        /// <summary>
        /// Empty array of tags. None.
        /// </summary>
        public static readonly Tag[] None = new Tag[0];

        /// <summary>
        /// Does the <see cref="Property"/> contain a value?
        /// </summary>
        public bool HasProperty => !string.IsNullOrEmpty(Property);

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
            => MatchProperty(other.Property) && MatchValue(other.Value);

        public override string ToString()
            => $"{{'{Property}':'{Value}'}}";

        #region Querries

        public bool MatchProperty(string query)
            => Property.EqualsOrdinal(query);

        public bool MatchValue(string query)
            => Value.EqualsOrdinal(query);

        public bool Match(string propertyQuery, string valueQuery)
            => MatchProperty(propertyQuery) && MatchValue(valueQuery);

        #endregion Querries
    }
}
