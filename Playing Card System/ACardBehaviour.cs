using UnityEngine;

/// <summary>
/// Base class for card scene behaviours.
/// </summary>
public abstract class ACardBehaviour : RichMonoBehaviour
{
	public abstract Sprite FrontSprite { get; set; }
	public abstract Sprite BackSprite { get; set; }

	public virtual void UpdateVisuals(CardSO newCard)
		=> FrontSprite = newCard.FaceImage;

	private void Reset()
	{
		SetDevDescription("I'm the world-space representation of a card!");
	}
}