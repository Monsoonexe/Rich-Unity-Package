/// <summary>
/// Holds an immutable ID.
/// </summary>
public struct AudioID
{
    public static uint IDCounter = 1;

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
    public static AudioID Invalid { get => new AudioID(0); }
    public static uint GetNextID() => IDCounter++; 
    public static AudioID GetNextKey() => new AudioID(GetNextID()); 
    public static implicit operator uint(AudioID a) => a.ID;
}
