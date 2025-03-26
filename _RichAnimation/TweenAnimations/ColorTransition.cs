using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using Sirenix.OdinInspector;
using DG.Tweening;

namespace RichPackage.Animation
{
	/// <summary>
	/// 
	/// </summary>
	/// <see cref="UIAnimation"/>
	public class ColorTransition : RichMonoBehaviour, IAnimate, IAnimate<Graphic>
	{
		[Title("Prefab Refs")]
		[SerializeField, Required]
		private Graphic targetGraphic;

		[Title("---Settings---")]
		[SerializeField, Min(0)]
		private float animTime = 1.0f;

		public Ease ease = Ease.Linear;

		public Color color = new Color(0, 0, 0, 1);

		public bool loop = false;

		[ShowIf("@loop"), Tooltip("-1 means 'infinite'.")]
		[Min(-1)]
		public int loops = -1;

		[ShowIf("@loop")]
		[Tooltip("Incremental is not recommended.")]
		public LoopType loopType = LoopType.Yoyo;

		[Tooltip("Should the color return to its starting color when the animation is complete? This would be true for effect-style animations but false for transition-style animations.")]
		public bool returnToStartColorOnEnd = true;

		[FoldoutGroup("---Events---")]
		[SerializeField]
		private UnityEvent onAnimComplete = new UnityEvent();
		public UnityEvent OnAnimComplete { get => onAnimComplete; }//readonly

		[ShowInInspector, ReadOnly]
		public bool IsAnimating { get => animTween != null; }
        public float Duration { get => animTime; set => animTime = value; }

        //runtime data
        private Tween animTween;
		private Color cachedColor;

		/// <summary>
		/// The tweener performing the animation if running, null if idle.
		/// </summary>
		public Tween Tween { get => animTween; }

		protected override void Reset()
		{
			base.Reset();
			SetDevDescription("Animates an Image's Color property.");

			//make a general attempt to auto locate the necessary components.
			targetGraphic = GetComponent<Graphic>();
			if (targetGraphic == null)
				targetGraphic = GetComponentInChildren<Graphic>();
		}

		private void OnDisable()
		{
			Stop();
		}

		private void OnAnimationComplete()
		{
			if (returnToStartColorOnEnd)
				targetGraphic.color = cachedColor;

			animTween = null;
			onAnimComplete.Invoke();
		}

		[Button, DisableInEditorMode]
		public void Play()
		{
			Stop(); //prevent multiple, duplicate animations

			if (returnToStartColorOnEnd)
				cachedColor = targetGraphic.color;

			animTween = targetGraphic.DOBlendableColor(color, animTime);
			animTween.onComplete += OnAnimationComplete;
			animTween.SetEase(ease);

			if (loop)
				animTween.SetLoops(loops, loopType);

			animTween.Play();
		}

		public void Play(Graphic graphic)
		{
			targetGraphic = graphic;
			Play();
		}

		[Button, DisableInEditorMode]
		public void Stop()
		{
			if (animTween != null)
			{
				animTween.Kill(complete: true);
			}
		}
	}
}
