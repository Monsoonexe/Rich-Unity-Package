using UnityEngine;

/// <summary>
/// Always draws cards in the exact same order.
/// </summary>
/// <typeparam name="T"></typeparam>
public class SequentialDeck<T> : Deck<T>
{
    public override void Shuffle()
    {
        ReloadDeck();
    }

    public override void ShuffleRemaining()
    {
        ReloadDeck();
    }
}
