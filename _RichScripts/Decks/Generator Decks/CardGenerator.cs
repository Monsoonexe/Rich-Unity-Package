
/// <summary>
/// Generators never expend cards -- probability of outcome never changes.
/// </summary>
/// <typeparam name="TContainer"></typeparam>
/// <typeparam name="TValue"></typeparam>
public class CardGenerator<TContainer, TValue> : StackedDeck<TContainer, TValue>
    where TContainer : AWeightedProbability<TValue>
{
    public override int CardsRemaining { get => stackedDeck.unusedCards.Count; }

    public override TValue Draw()
    {
        var deck = stackedDeck.unusedCards;
        if (deck.Count == 0) return default;

        var iCard = GetWeightedIndex(deck);
        return deck[iCard].Value;         
    }

    public override void ReloadDeck()
    {
    }

    public override void ShuffleRemaining()
    {
    }
}
