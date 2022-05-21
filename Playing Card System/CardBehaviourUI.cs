using UnityEngine;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using RichPackage.Assertions;

namespace RichPackage.PlayingCards
{
	/// <summary>
	/// Card behaviour in Scene using UI.Images as backer.
	/// </summary>
	[SelectionBase]
	public class CardBehaviourUI : ACardBehaviour
	{
		[Header("---Prefab Refs---")]
		[Required, SerializeField]
		private Image frontImage;

		[Required, SerializeField]
		private Image backImage;

		public override Sprite FrontSprite { get => frontImage.sprite; set => frontImage.sprite = value; }
		public override Sprite BackSprite { get => backImage.sprite; set => backImage.sprite = value; }

		protected override void Awake()
		{
			base.Awake();

			//validate
			Assert.IsNotNull(frontImage, this);
			Assert.IsNotNull(backImage, this);
		}
	}
}
