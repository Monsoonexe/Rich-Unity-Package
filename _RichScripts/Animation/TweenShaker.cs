using DG.Tweening;
using Sirenix.OdinInspector;

/// <summary>
/// I do a shake tween animation.
/// </summary>
public class TweenShaker : RichMonoBehaviour
{
	[Title("Animation Options")]
	public bool playOnAwake = false;
	public float shakeDuration = 1.0f;

	#region Shake Position
	[BoxGroup("SHAKE_POSITION")]
	public bool shouldShakePosition;
	[BoxGroup("SHAKE_POSITION"), ShowIf("@shouldShakePosition")]
	public float shakePositionStrength = 1.0f;
	[BoxGroup("SHAKE_POSITION"), ShowIf("@shouldShakePosition")]
	public int shakePositionVibrato = 10;
	[BoxGroup("SHAKE_POSITION"), ShowIf("@shouldShakePosition")]
	public float shakePositionRandomness = 90;
	[BoxGroup("SHAKE_POSITION"), ShowIf("@shouldShakePosition")]
	public bool shakeSnap = false;
	[BoxGroup("SHAKE_POSITION"), ShowIf("@shouldShakePosition")]
	public bool shakePositionFadeOut = true;
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

	private void Reset()
	{
		SetDevDescription("I do a shake tween animation.");
	}

	protected override void Awake()
	{
		base.Awake();
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

	[Button, DisableInEditorMode]
	public Tweener ShakeScale()
	{
		return transform.DOShakeScale(shakeDuration,
				shakeScaleStrength, shakeScaleVibrato,
				shakeScaleRandomness, shakeScaleFadeOut);
	}
}
