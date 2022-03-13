using System.Diagnostics;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

//TODO - Peeking feature. Look at the next card to be drawn but don't actually draw it.

/// <summary>
/// Base class for decks to help with serialization.
/// </summary>
public abstract class ADeck : RichScriptableObject
{
    //exists as base class
}

/// <summary>
/// Base class for a Deck of somethings.
/// </summary>
/// <typeparam name="T">The thing that you want to get out of this Deck.</typeparam>
/// <seealso cref="Deck{T}"/>
public abstract class ADeck<T> : ADeck
{
    [SerializeField]
    [Tooltip("A list of the cards that belong to this deck.")]
    protected List<T> manifest = new List<T>();

    /// <summary>
    /// A list of the cards that belong to this deck.
    /// </summary>
    public List<T> Manifest { get => manifest; }

    public int TotalDeckSize { get => manifest.Count; }

    /// <summary>
    /// Draw()s remaining until Deck needs to Reload().
    /// </summary>
    [ShowInInspector]
    public abstract int CardsRemaining { get; }

    protected virtual void OnEnable()
	{
        Reload();
	}

    /// <summary>
    /// Get a card out of the deck.
    /// </summary>
    /// <returns></returns>
    public abstract T Draw();

    /// <summary>
    /// Get a card out of the deck and cast it.
    /// </summary>
    /// <typeparam name="U"></typeparam>
    /// <returns></returns>
    public U Draw<U>() where U : T => (U)Draw();

    /// <summary>
    /// Recombine un/used cards.
    /// </summary>
    [Button]
    public abstract void Reload();
    [Button]
    public abstract void Shuffle();
    [Button]
    public abstract void ShuffleRemaining();

    [Button]
    [Conditional("UNITY_EDITOR")]
    public void TestDraw()
    {
        var card = Draw(); //breakpoint here.
        UnityEngine.Debug.Log(card);
    }
}
