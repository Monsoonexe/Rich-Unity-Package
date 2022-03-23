
namespace RichPackage.Audio
{
    /// <summary>
    /// Holds an immutable ID.
    /// </summary>
    public struct AudioID
    {
        private static uint IDCounter = 1;
        public const uint INVALID_ID = 0;

        public readonly uint ID;

        /// <summary>
        /// preferred method: var key = new AudioID(AudioID.GetNextID);
        /// </summary>
        private AudioID(uint id)
        {
            ID = id;
        }

        /// <summary>
        /// Returns a new AudioID of 0, which is a flag it's invalid.
        /// </summary>
        public static AudioID Invalid => new AudioID(INVALID_ID);

        public static AudioID GetNextKey() => new AudioID(IDCounter++); 

        public static implicit operator uint(AudioID a) => a.ID;
    }
}
