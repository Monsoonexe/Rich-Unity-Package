using UnityEngine;

/// <summary>
/// Always draws cards in the exact same order.
/// </summary>
/// <typeparam name="T"></typeparam>
public class SequentialDeck<T> : Deck<T>
{
    public override void Shuffle()
    {
        Reload();
    }

    /// <summary>
    /// This has no effect on a Sequential Deck but is not an error to do so.
    /// </summary>
    public override void ShuffleRemaining()
    {
        //shuffling has no random effect
    }
}
