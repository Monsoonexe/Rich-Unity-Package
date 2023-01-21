using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace RichPackage
{
    /// <summary>
    /// A string-based unique identifier.
    /// </summary>
    [Serializable]
    public struct UniqueID : IEquatable<UniqueID>
    {
        private const int MAX_LENGTH = 16;

        /// <summary>
        /// The factory method which is responsible for creating a
        /// new <see cref="UniqueID"/>.
        /// By default it implements the <see cref="Guid"/> class,
        /// but it can be overridden with your own implementation.
        /// </summary>
        public static Func<string> NewIDProvider { get; set; }
            = () => Guid.NewGuid().ToString().Remove("-").Substring(0, MAX_LENGTH);

        public static UniqueID New
        {
            get => FromString(NewIDProvider());
        }

        [field: SerializeField, LabelText(nameof(ID))]
        public string ID { get; private set; }

        #region Constructors

        public UniqueID(string id)
        {
            ID = id;
        }

        #endregion Constructors

        public override string ToString() => ID;

        public override int GetHashCode() => ID.GetHashCode();

        public static UniqueID FromString(string src) => new UniqueID(src);

        public bool Equals(UniqueID other) => ID.QuickEquals(other.ID);

        public override bool Equals(object obj)
            => obj is UniqueID other && Equals(other);

        public static bool operator ==(UniqueID a, UniqueID b) => a.Equals(b);

        public static bool operator !=(UniqueID a, UniqueID b) => !(a == b);

        public static implicit operator string(UniqueID id) => id.ID;
    }
}
