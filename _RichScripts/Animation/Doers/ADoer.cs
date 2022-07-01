using DG.Tweening;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace RichPackage.Animation
{
	/// <summary>
	/// 
	/// </summary>
	public abstract class ADoer : RichMonoBehaviour, IAnimate
	{
		[FoldoutGroup("Events")]
		[SerializeField, Tooltip("Ensure listener is idempotent.")]
		private UnityEvent onAnimationStart = new UnityEvent();
		public UnityEvent OnAnimationStart => onAnimationStart;

		[FoldoutGroup("Events")]
		[SerializeField, Tooltip("Ensure listener is idempotent.")]
		private UnityEvent onAnimationEnd = new UnityEvent();
		public UnityEvent OnAnimationEnd => onAnimationEnd;

		[Title("Settings")]
		public Transform target;
		[Min(0)]
		public float duration = 0.85f;
		public bool loop;
		[HideIf(nameof(loop)), Min(-1)]
		public int loops = -1;

		public Tween Tween { get; protected set; }

		[ShowInInspector, ReadOnly]
		public bool IsAnimating => Tween != null && Tween.IsPlaying();

		protected override void Reset()
		{
			SetDevDescription("Helps provide DOTween animations.");
			myTransform = GetComponent<Transform>();
			target = myTransform;
		}

		[Button, DisableInEditorMode, HorizontalGroup("b")]
		public abstract void Play();

		[Button, DisableInEditorMode, HorizontalGroup("b")]
		public virtual void Stop()
		{
			if (IsAnimating)
				Tween.Kill(complete: false);
		}

		protected void CallOnAnimationStartEvent()
			=> onAnimationStart.Invoke();

		protected void CallOnAnimationEndEvent()
		{
			onAnimationEnd.Invoke();
			Tween = null;
		}

		protected Tween SubscribeTweenEvents(Tween tween)
		{
			tween.OnStart(CallOnAnimationStartEvent);
			tween.OnComplete(CallOnAnimationEndEvent);

			return tween;
		}

		public void StopAllTweens() => StopAllTweens(complete: false);

		public void StopAllTweens(bool complete)
			=> target.DOKill(complete);
	}
}
