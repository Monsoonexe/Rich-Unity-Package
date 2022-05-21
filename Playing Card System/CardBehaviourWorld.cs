using UnityEngine;
using Sirenix.OdinInspector;
using RichPackage.Assertions;

namespace RichPackage.PlayingCards
{
	/// <summary>
	/// Card behaviour in Scene using SpriteRenderers as backer.
	/// </summary>
	[SelectionBase]
	public class CardBehaviourWorld : ACardBehaviour
	{
		[Header("---Prefab Refs---")]
		[Required, SerializeField]
		private SpriteRenderer frontImage;

		[Required, SerializeField]
		private SpriteRenderer backImage;

		public override Sprite FrontSprite { get => frontImage.sprite; set => frontImage.sprite = value; }
		public override Sprite BackSprite { get => backImage.sprite; set => backImage.sprite = value; }

		protected override void Awake()
		{
			base.Awake();

			//validate
			Assert.IsNotNull(frontImage);
			Assert.IsNotNull(backImage);
		}
	}
}
