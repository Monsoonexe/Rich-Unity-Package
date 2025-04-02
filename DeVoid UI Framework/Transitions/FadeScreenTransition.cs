using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace RichPackage.UI.Transitions
{
	/// <summary>
	/// Fades a CanvasGroup in/out.
	/// </summary>
	public sealed class FadeScreenTransition : ATransitionComponent
	{
		[Title("Settings")]
		public bool isOutAnimation;
		public Ease ease = Ease.Linear;

		[Title("Scene Refs"), Required]
		public CanvasGroup canvasGroup;

		public override bool IsAnimating
		{
			get => Animation != null;
		}

		//runtime data
		public Tweener Animation { get; private set; }
		private float cachedAlpha;

		protected override void Reset()
		{
			base.Reset();
			canvasGroup = GetComponent<CanvasGroup>();
		}

		public override void Animate(Transform targetXform,
			Action onCompleteCallback = null)
		{
			//cache
			cachedAlpha = canvasGroup.alpha;

			//create tween
			float fadeAmount = 0.0f;
			if (isOutAnimation)
			{
				fadeAmount = 0.0f;
			}
			else //fade in
			{
				canvasGroup.alpha = 0; //start at zero
				fadeAmount = cachedAlpha; //current amount
			}

			Animation = canvasGroup.DOFade(fadeAmount, duration);

			//rig on complete callback
			onCompleteCallback += OnAnimationComplete;
			Animation.OnComplete(onCompleteCallback.Invoke);
		}

		private void OnAnimationComplete()
		{
			Animation = null;//clear flag
			canvasGroup.alpha = cachedAlpha; //leave it how you found it
		}

		public override void Stop() => Animation?.Kill();
	}
}