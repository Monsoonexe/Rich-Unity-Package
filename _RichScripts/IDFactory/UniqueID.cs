using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace RichPackage
{
    /// <summary>
    /// A string-based unique identifier.
    /// </summary>
    [Serializable]
    public struct UniqueID : IEquatable<UniqueID>, IEquatable<string>, IEquatable<int>
    {
        private const int MAX_LENGTH = 16;

        /// <summary>
        /// The factory method which is responsible for creating a
        /// new <see cref="UniqueID"/>.
        /// By default it implements the <see cref="Guid"/> class,
        /// but it can be overridden with your own implementation.
        /// </summary>
        public static Func<string> NewIdProvider { get; set; }
            = () => Guid.NewGuid().ToString().Remove("-").Substring(0, MAX_LENGTH);

        public static UniqueID New => FromString(NewIdProvider());

        public static readonly UniqueID None = new UniqueID(string.Empty);

        [field: SerializeField, LabelText(nameof(ID)),
            OnValueChanged(nameof(RecalculateHash))]
        public string ID { get; private set; } // I wish I could make this null-safe. if only it were backed by a SerializeField :/

        private int? cachedHash;
        public int Hash { get => cachedHash ?? RecalculateHash(); }

        #region Constructors

        public UniqueID(string id)
        {
            ID = id;
            cachedHash = null;
        }

        #endregion Constructors

        /// <summary>
        /// Only required to call if ID was changed magically and cached hash became stale.
        /// </summary>
        public int RecalculateHash()
        {
            if (ID == null)
                return 0;
            return (cachedHash = ID.GetHashCode()).Value;
        }

        public void GenerateNewId() => ID = New;

        #region Object

        public override string ToString() => ID;

        public override int GetHashCode() => Hash;

        public override bool Equals(object obj) => obj is UniqueID other && Equals(other);

        #endregion Object

        #region IEquatable

        public bool Equals(UniqueID other) => this.Hash == other.Hash;
        public bool Equals(string other) => ID.QuickEquals(other);
        public bool Equals(int other) => Hash == other;

        #endregion IEquatable

        public static UniqueID FromString(string src) => new UniqueID(src);
        public static ConditionInfo IsValid(UniqueID id)
        {
            if (id.ID == null)
                return ConditionInfo.FromFalse("Id is null");
            if (id.ID == "")
                return ConditionInfo.FromFalse("Id is empty");
            return true;
        }

        public static bool EnsureValid(ref UniqueID id, UniqueID fallback)
        {
            bool invalid;
            if (invalid = id.ID.IsNullOrEmpty())
            {
                id = fallback;
            }

            return !invalid;
        }

        #region Equality Operators

        public static bool operator ==(UniqueID a, UniqueID b) => a.Equals(b);
        public static bool operator !=(UniqueID a, UniqueID b) => !(a == b);

        public static bool operator ==(UniqueID a, string b) => a.Equals(b);
        public static bool operator !=(UniqueID a, string b) => !(a == b);

        public static bool operator ==(string a, UniqueID b) => b.Equals(a);
        public static bool operator !=(string a, UniqueID b) => !(a == b);

        public static bool operator ==(int a, UniqueID b) => b.Equals(a);
        public static bool operator !=(int a, UniqueID b) => !(a == b);

        public static bool operator ==(UniqueID a, int b) => b.Equals(a);
        public static bool operator !=(UniqueID a, int b) => !(a == b);

        #endregion Equality Operators

        public static implicit operator string(UniqueID id) => id.ID;
        public static implicit operator int(UniqueID id) => id.Hash;
        public static implicit operator UniqueID(string id) => new UniqueID(id);
    }
}
