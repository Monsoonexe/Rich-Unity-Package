using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace RichPackage.Animation
{

	/// <summary>
	/// I do a shake tween animation.
	/// </summary>
	public class TweenShaker : RichMonoBehaviour
	{
		public const float DEFAULT_SHAKE_STRENGTH = 1.0f;
		public const int DEFAULT_SHAKE_VIBRATO = 10;
		public const float DEFAULT_SHAKE_DURATION = 0.5f;
		public const float DEFAULT_SHAKE_RANDOMNESS = 90f;
		public const bool DEFAULT_SHAKE_SNAP = false;
		public const bool DEFAULT_SHAKE_FADE_OUT = true;

		[Title("Animation Options")]
		public Transform target;
		public bool playOnAwake = false;
		public float shakeDuration = DEFAULT_SHAKE_STRENGTH;

		#region Shake Position
		[BoxGroup("SHAKE_POSITION")]
		public bool shouldShakePosition;
		[BoxGroup("SHAKE_POSITION"), ShowIf("@shouldShakePosition")]
		public float shakePositionStrength = DEFAULT_SHAKE_STRENGTH;
		[BoxGroup("SHAKE_POSITION"), ShowIf("@shouldShakePosition")]
		public int shakePositionVibrato = DEFAULT_SHAKE_VIBRATO;
		[BoxGroup("SHAKE_POSITION"), ShowIf("@shouldShakePosition")]
		public float shakePositionRandomness = DEFAULT_SHAKE_RANDOMNESS;
		[BoxGroup("SHAKE_POSITION"), ShowIf("@shouldShakePosition")]
		public bool shakeSnap = DEFAULT_SHAKE_SNAP;
		[BoxGroup("SHAKE_POSITION"), ShowIf("@shouldShakePosition")]
		public bool shakePositionFadeOut = DEFAULT_SHAKE_FADE_OUT;
		#endregion

		#region Shake Rotation
		[BoxGroup("SHAKE_ROTATION")]
		public bool shouldShakeRotation;
		[BoxGroup("SHAKE_ROTATION"), ShowIf("@shouldShakeRotation")]
		public float shakeRotationStrength = 1.0f;
		[BoxGroup("SHAKE_ROTATION"), ShowIf("@shouldShakeRotation")]
		public int shakeRotationVibrato = 10;
		[BoxGroup("SHAKE_ROTATION"), ShowIf("@shouldShakeRotation")]
		public float shakeRotationRandomness = 90;
		[BoxGroup("SHAKE_ROTATION"), ShowIf("@shouldShakeRotation")]
		public bool shakeRotationFadeOut = true;
		#endregion

		#region Shake Scale

		[BoxGroup("SHAKE_SCALE")]
		public bool shouldShakeScale;
		[BoxGroup("SHAKE_SCALE"), ShowIf("@shouldShakeScale")]
		public float shakeScaleStrength = 1.0f;
		[BoxGroup("SHAKE_SCALE"), ShowIf("@shouldShakeScale")]
		public int shakeScaleVibrato = 10;
		[BoxGroup("SHAKE_SCALE"), ShowIf("@shouldShakeScale")]
		public float shakeScaleRandomness = 90;
		[BoxGroup("SHAKE_SCALE"), ShowIf("@shouldShakeScale")]
		public bool shakeScaleFadeOut = true;

		#endregion

		protected override void Reset()
		{
			base.Reset();
			SetDevDescription("I do a shake tween animation.");
			target = GetComponent<Transform>();
		}

		protected override void Awake()
		{
			base.Awake();
			if(target == null) target = myTransform;
			if (playOnAwake) Shake();
		}

		[Button, DisableInEditorMode]
		public Tween Shake()
		{
			Sequence shakeSequence = DOTween.Sequence();//get from pool

			if (shouldShakePosition)
				shakeSequence.Join(ShakePosition());

			if (shouldShakeRotation)
				shakeSequence.Join(ShakeRotation());

			if (shouldShakeScale)
				shakeSequence.Join(ShakeScale());

			return shakeSequence;
		}

		[Button, DisableInEditorMode]
		public Tweener ShakePosition()
		{
			return transform.DOShakePosition(shakeDuration,
					shakePositionStrength, shakePositionVibrato,
					shakePositionRandomness,
					shakeSnap, shakePositionFadeOut);
		}

		[Button, DisableInEditorMode]
		public Tweener ShakeRotation()
		{
			return transform.DOShakeRotation(shakeDuration,
					shakeRotationStrength, shakeRotationVibrato,
					shakeRotationRandomness, shakeRotationFadeOut);
		}

		public Tweener SetAndShakePosition(
			float duration = DEFAULT_SHAKE_DURATION,
			float strength = DEFAULT_SHAKE_STRENGTH,
			int vibrato = DEFAULT_SHAKE_VIBRATO,
			float randomness = DEFAULT_SHAKE_RANDOMNESS,
			bool fadeOut = DEFAULT_SHAKE_FADE_OUT,
			bool snap = DEFAULT_SHAKE_SNAP)
		{
			shakeDuration = duration;
			shakePositionStrength = strength;
			shakePositionVibrato = vibrato;
			shakePositionRandomness = randomness;
			shakePositionFadeOut = fadeOut;
			shakeSnap = snap;
			return ShakePosition();
		}

		[Button, DisableInEditorMode]
		public Tweener ShakeScale()
		{
			return transform.DOShakeScale(shakeDuration,
					shakeScaleStrength, shakeScaleVibrato,
					shakeScaleRandomness, shakeScaleFadeOut);
		}
	}
}
