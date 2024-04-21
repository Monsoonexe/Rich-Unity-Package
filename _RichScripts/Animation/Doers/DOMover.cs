using UnityEngine;
using DG.Tweening;
using RichPackage.FunctionalProgramming;

namespace RichPackage.Animation
{
    public sealed class DOMover : ADoer
	{
		public bool From = false;
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
			if (From)
				Tween.CastAs<Tweener>().From();
			SubscribeTweenEvents(Tween);
		}
	}
}
