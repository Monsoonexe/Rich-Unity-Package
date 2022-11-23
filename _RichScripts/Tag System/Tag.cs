using System;
using UnityEngine;
using Sirenix.OdinInspector;

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
        /// The property name, such as "Clothing".
        /// </summary>
        [field: SerializeField, LabelText(nameof(Property))]
        public string Property { get; private set; }

        /// <summary>
        /// The value of the tag, such as "White".
        /// </summary>
        [field: SerializeField, LabelText(nameof(Value))]
        public string Value { get; private set; }

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
            // TODO - quick compare string
            return this.Property == other.Property
                && this.Value == other.Value;
        }

        public override string ToString()
        {
            return $"{{'{Property}':'{Value}'}}";
        }
    }
}
