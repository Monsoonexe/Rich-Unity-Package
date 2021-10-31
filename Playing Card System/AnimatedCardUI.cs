using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using Sirenix.OdinInspector;

/*TODO - Use composition to implement both sprite and ui versions
 * 
 */ 

/// <summary>
/// 
/// </summary>
/// <seealso cref="CardBehaviourWorld"/>
[SelectionBase]
public class AnimatedCardUI : CardBehaviourUI
{
	[Title("Orientation Settings")]
	public Vector3 faceUpRotation = new Vector3(0, 180, 0);
	public Vector3 faceDownRotation = new Vector3(0, 0, 0);

	[Title("Flip Animation Settings")]
	public float flipDuration = 1.0f;
	public Ease flipEase = Ease.OutQuart;

	[Title("Punch Animation Settings")]
	public bool punchOnFlip = true;
	public Vector3 punchScale = new Vector3(0.25f, 0.25f, 0);
	public int punchVibrato = 5;

	[FoldoutGroup("Events")]
	[SerializeField]
	private UnityEvent onFlipFaceDownCompleteEvent;

	[FoldoutGroup("Events")]
	[SerializeField]
	private UnityEvent onFlipFaceUpCompleteEvent;

	/// <summary>
	/// Action version.
	/// </summary>
	public void DoFlipFaceUp() => FlipFaceUp();

	/// <summary>
	/// Action version.
	/// </summary>
	public void DoFlipFaceDown() => FlipFaceDown();

	public Tween FlipFaceUp()
	{
		Sequence flipSequence = DOTween.Sequence(); //depool

		Tweener flipTween = transform.DOLocalRotate(
			faceUpRotation, flipDuration)
			.SetEase(flipEase);

		flipSequence.Append(flipTween);

		if(punchOnFlip)
		{
			//punch for added effect
			flipSequence.Join(transform.DOPunchScale(
				punchScale, flipDuration, punchVibrato)); //combine into single animation
		}
		//flipSequence.OnComplete(onFlipFaceUpCompleteEvent.Invoke);
		flipSequence.OnStart(onFlipFaceUpCompleteEvent.Invoke);

		return flipSequence;
	}

	public Tween FlipFaceDown()
	{

		Tweener flipTween = transform.DOLocalRotate(
			faceDownRotation, flipDuration)
			.SetEase(flipEase)
			.OnComplete(onFlipFaceDownCompleteEvent.Invoke);

		return flipTween;
	}

	/// <summary>
	/// Immediately, with no animation, change orientation.
	/// </summary>
	public void FlipFaceDownSnap()
		=> transform.localEulerAngles = faceDownRotation;

	/// <summary>
	/// Immediately, with no animation, change orientation.
	/// </summary>
	public void FlipFaceUpSnap()
		=> transform.localEulerAngles = faceUpRotation;
}
