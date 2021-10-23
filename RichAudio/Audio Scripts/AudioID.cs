/// <summary>
/// Holds an immutable ID.
/// </summary>
public struct AudioID
{
    private static uint IDCounter = 1;
    public const int INVALID_ID = 0;

    public readonly uint ID;

    /// <summary>
    /// preferred method: var key = new AudioID(AudioID.GetNextID);
    /// </summary>
    /// <param name="id"></param>
    public AudioID(uint id)
    {
        ID = id;
    }

    /// <summary>
    /// Returns a new AudioID of 0, which is a flag it's invalid.
    /// </summary>
    public static AudioID Invalid { get => new AudioID(INVALID_ID); }
    //private static uint GetNextID() => IDCounter++; 
    public static AudioID GetNextKey() => new AudioID(IDCounter++); 
    public static implicit operator uint(AudioID a) => a.ID;
}
