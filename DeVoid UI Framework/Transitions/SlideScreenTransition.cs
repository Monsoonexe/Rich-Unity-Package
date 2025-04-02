using DG.Tweening;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace RichPackage.UI.Transitions
{
	/// <summary>
	/// UI Window slide in/out animation.
	/// </summary>
	/// <seealso cref="ScaleScreenTransition"/>
	public class SlideScreenTransition : ATransitionComponent
	{
		private const string FadeGroup = "Fade";

		[Title("Options")]
		[SerializeField, Tooltip("The position of this screen when it is hidden (not active)."),
			CustomContextMenu("Set hidden position", nameof(Editor_SetHiddenPosition))]
		private Vector2 hiddenPosition;

		[SerializeField]
		protected bool isOutAnimation;

		[SerializeField]
		protected Ease ease = Ease.Linear;

		[SerializeField, BoxGroup(FadeGroup)]
		protected bool doFade;

		[SerializeField, BoxGroup(FadeGroup), ShowIf(nameof(doFade))]
		protected float fadeDurationPercent = 0.5f;

		[SerializeField, BoxGroup(FadeGroup), ShowIf(nameof(doFade))]
		private CanvasGroup canvasGroup = null;

		// cache variables to avoid Closure
		private Action callback;
		private RectTransform rTransform;
		private Vector2 originalAnchoredPos;

		public override void Animate(Transform target, Action callWhenFinished = null)
		{
			Debug.Assert(target == this.target, "I didn't expect you to change targets!", this);

			// lazy init because awake isn't reliable
			if (rTransform is null)
			{
				rTransform = target as RectTransform;
			}

			originalAnchoredPos = rTransform.anchoredPosition;
			callback = callWhenFinished;
			rTransform.DOKill(complete: true);
			IsAnimating = true;

			if (doFade)
			{
				canvasGroup = gameObject.GetComponentIfNull(canvasGroup);
				canvasGroup.DOFade(isOutAnimation ? 0f : 1f, duration * fadeDurationPercent);
			}

			var tween = rTransform.DOAnchorPos(hiddenPosition, duration, true)
				.SetEase(ease)
				.OnComplete(() =>
				{
					IsAnimating = false;

					rTransform.anchoredPosition = originalAnchoredPos;
					if (canvasGroup != null)
					{
						canvasGroup.alpha = 1f;
					}
					callback?.Invoke();
					callback = null;
				})
				.SetUpdate(true);

			if (!isOutAnimation)
				tween.From();
		}

		public override void Stop()
		{
			rTransform.DOKill(complete: true);

			//if (callback != null)
			//{
			//	// handle state before transferring control
			//	Action action = callback;
			//	callback = null;
			//	action();
			//}
		}

		private void Editor_SetHiddenPosition()
		{
			hiddenPosition = GetComponent<RectTransform>().anchoredPosition;
		}
	}
}
