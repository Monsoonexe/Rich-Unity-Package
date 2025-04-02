using Cysharp.Threading.Tasks;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace RichPackage.UI.Transitions
{
	/// <summary>
	/// Translates a target transform.
	/// </summary>
	public abstract class ATransitionComponent : MonoBehaviour
	{
		/// <summary>
		/// How many seconds to complete the Animation.
		/// </summary>
		[Tooltip("How many seconds to complete the Animation.")]
		[SerializeField, Min(0)]
		protected float duration = 0.5f;

		public float Duration { get => duration; }

		public virtual bool IsAnimating { get; protected set; }

		[SerializeField]
		protected Transform target;

		#region Unity Messages

		protected virtual void Reset()
		{
			target = GetComponent<Transform>();
		}

		#endregion Unity Messages

		/// <summary>
		/// Perform the animation.
		/// </summary>
		/// <param name="targetXform">What to animate.</param>
		/// <param name="onCompleteCallback">[Optional] What to do when finished.</param>
		public abstract void Animate(Transform targetXform, Action onCompleteCallback = null);

		public abstract void Stop();

		public void Play(Transform puppet) => Animate(target = puppet);

		[Button, DisableInEditorMode]
		public void Play() => Play(target ? target : transform);

		public UniTask PlayAsync()
		{
			if (IsAnimating)
				Stop();

			// need to pass event here because it is really hard to get unitasks await timing correct,
			// and sometimes there is a frame of delay between the animation ending and the continuation
			var tcs = new UniTaskCompletionSource();
			Animate(target, () => tcs.TrySetResult());
			return tcs.Task;
		}
	}
}
