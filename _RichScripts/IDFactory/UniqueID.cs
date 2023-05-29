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

        public static UniqueID New => FromString(NewIDProvider());

        [field: SerializeField, LabelText(nameof(ID)),
            CustomContextMenu("Regenerate", nameof(GenerateNewId))]
        public string ID { get; private set; }

        private int? cachedHash;
        public int Hash { get => cachedHash ?? (cachedHash = ID.GetHashCode()).Value; }

        #region Constructors

        public UniqueID(string id)
        {
            ID = id;
            cachedHash = null;
        }

        #endregion Constructors

        public void GenerateNewId() => ID = New;
        
        public override string ToString() => ID;

        public override int GetHashCode() => Hash;

        public bool Equals(UniqueID other) => Hash == other.Hash;

        public static UniqueID FromString(string src) => new UniqueID(src);

        public override bool Equals(object obj)
            => obj is UniqueID other && Equals(other);

        public static bool operator == (UniqueID a, UniqueID b) => a.Equals(b);
        public static bool operator != (UniqueID a, UniqueID b) => !(a == b);

        public static implicit operator string (UniqueID id) => id.ID;
        public static implicit operator int(UniqueID id) => id.Hash;
    }
}
