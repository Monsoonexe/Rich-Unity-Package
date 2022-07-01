using Sirenix.OdinInspector;
using System.Diagnostics;
using UnityEngine;

namespace RichPackage.Decks
{
	/// <summary>
	/// Wrapper class so <see cref="ADeck{TCard}"/> can be serialized.
	/// This class isn't needed if Unity can directly serialize generics without
	/// Odin's help. #2020+
	/// </summary>
	public abstract class ADeckBox<TCard> : SerializedScriptableObject
	{
#if UNITY_EDITOR
		[SerializeField, TextArea]
		[PropertyOrder(-5)]
#pragma warning disable IDE0052 // Remove unread private members
		private string developerDescription = "Please enter a description or a note.";
#pragma warning restore IDE0052 // Remove unread private members
#endif

		[InlineProperty, HideLabel, Title(nameof(Deck))]
		public ADeck<TCard> Deck;

		public static implicit operator ADeck<TCard>(
			ADeckBox<TCard> a) => a.Deck;

		protected virtual void Reset()
		{
			SetDevDescription("Wraps ADeck<TCard> so it can be drawn in the Inspector by Odin. Obsolete in Unity 2020+");
		}

		#region Editor

		/// <summary>
		/// This call will be stripped out of Builds. Use anywhere.
		/// </summary>
		/// <param name="newDes"></param>
		[Conditional(ConstStrings.UNITY_EDITOR)]
		public void SetDevDescription(string newDes)
		{
#if UNITY_EDITOR
			developerDescription = newDes;
#endif
		}

		#endregion Editor
	}
}
