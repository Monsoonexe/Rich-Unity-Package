using UnityEngine;
using DG.Tweening;

namespace RichPackage.Animation
{
	public class DOMover : ADoer
	{
		public Space space = Space.World;

		public Vector3 destination;

		public Ease ease = Ease.OutQuart;

		public override void Play()
		{
			//local or world space?
			if (space == Space.Self)
				Tween = target.DOLocalMove(destination, duration);
			else
				Tween = target.DOMove(destination, duration);

			//configure
			Tween.SetEase(ease);
			if (loop)
				Tween.SetLoops(loops);
			SubscribeTweenEvents(Tween);
		}
	}
}
