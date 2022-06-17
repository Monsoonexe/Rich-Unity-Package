using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace RichPackage.Animation
{
    /// <summary>
    /// 
    /// </summary>
    public class DORotator : ADoer
    {
        [Title("Animation Settings")]
        public Space space = Space.World;
        public Ease ease = Ease.OutQuart;
        public Vector3 destination = new Vector3 (0, 180, 0);
        public RotateMode mode = RotateMode.Fast;

		public override void Play()
		{
            if (space == Space.World)
                Tween = transform.DORotate(destination, duration, mode);
            else
                Tween = transform.DOLocalRotate(destination, duration, mode);

            //configure
            Tween.SetEase(ease);
            SubscribeTweenEvents(Tween);
        }
	}
}
