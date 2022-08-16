using System;

namespace RichPackage.Audio
{
    /// <summary>
    /// Holds an immutable, unique ID which can be used to interact with the <see cref="AudioManager"/>
    /// after an <see cref="UnityEngine.AudioClip"/> has been played.
    /// </summary>
    public struct AudioID : IEquatable<AudioID>
    {
        public const uint INVALID_ID = 0;
        private static uint IDCounter = 1;

        public readonly uint ID;

        /// <summary>
        /// Use <see cref="GetNext"/>.
        /// </summary>
        private AudioID(uint id)
        {
            ID = id;
        }

        /// <summary>
        /// Returns a new AudioID of 0, which is a flag it's invalid.
        /// </summary>
        public static AudioID Invalid => new AudioID(INVALID_ID);

        public static AudioID GetNext() => new AudioID(IDCounter++);

		public bool Equals(AudioID other) => this.ID == other.ID;

		public override int GetHashCode() => ID.GetHashCode();

		public override string ToString() => $"{nameof(AudioID)}-{ID}";

		public static implicit operator uint(AudioID a) => a.ID;
    }
}
