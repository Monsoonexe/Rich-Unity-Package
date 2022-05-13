using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using ScriptableObjectArchitecture;
using Sirenix.OdinInspector;

namespace RichPackage
{
	/// <summary>
	/// A pretty-accurate timer Mono.
	/// Counts down and raises an event when timer hits 0. Can loop.
	/// </summary>
	public class Timer : RichMonoBehaviour
	{
		private bool paused = false;

		[ShowInInspector, ReadOnly]
		public bool Paused
		{
			get => paused;
			set
			{
				if (value)
					Stop();
				else
					StartTimer();
			}
		}

		[Header("---Settings---")]
		public bool loop = false;

		/// <summary>
		/// Total duration of timer. TOP
		/// </summary>
		[Tooltip("Total duration of timer. TOP")]
		public float TimerDuration = 0;

		/// <summary>
		/// Time remaining on current loop. COUNTDOWN
		/// </summary>
		[Tooltip("Time remaining on current loop. COUNTDOWN")]
		public FloatReference TimeRemaining = new FloatReference(0);

		/// <summary>
		/// Accumulated time that has elapsed since last Restart() or Init().
		/// Not guaranteed to be equal to TimerDuration at end of timer due to deltaTime.
		/// </summary>
		[ShowInInspector, ReadOnly]
		public float TimeElapsed { get; private set; }

		/// <summary>
		/// 0 will effectively pause effect but won't pause coroutine.
		/// </summary>
		[Tooltip("0 will effectively pause effect but won't pause coroutine.")]
		public FloatReference TimeScale = new FloatReference(1.0f);

		[ShowInInspector, ReadOnly]
		public float PercentComplete
		{
			get => (TimerDuration - TimeRemaining) / TimerDuration;
		}

		[FoldoutGroup("---Events---")]
		[SerializeField]
		private UnityEvent onTimerExpire = new UnityEvent();
		public UnityEvent OnTimerExpire { get => onTimerExpire; }

		//runtime data
		private Coroutine timerRoutine;

		private void Reset()
		{
			SetDevDescription("Counts down and raises an event when timer hits 0. Can loop.");
		}

		private void Start()
		{
			//validate dependencies exist
			if (RichAppController.Instance == null)
				throw new MissingReferenceException($"{nameof(Timer)} relies on there being a " +
					$"{nameof(RichAppController)} in the scene, but there is none. Please add one.");
		}

		private void OnDisable()
		{
			Stop();
		}

		/// <param name="duration">Length of timer.</param>
		/// <param name="callback">Override current callback with this one.</param>
		/// <param name="loop">Auto-repeat at end?</param>
		public void Initialize(float duration, Action callback, 
			bool loop = false, bool startNow = true)
		{
			Debug.Assert(callback != null,
				"Callback is null and shouldn't be.", this);

			OnTimerExpire.RemoveAllListeners();
			OnTimerExpire.AddListener(callback.Invoke);//
			Initialize(duration, loop, startNow);//forward call
		}

		/// <param name="duration">Length of timer.</param>
		/// <param name="loop">Auto-repeat at end?</param>
		/// <param name="startNow">Should the <see cref="Timer"/> begin right away.</param>
		public void Initialize(float duration, 
			bool loop = false, bool startNow = true)
		{
			TimerDuration = duration;
			this.loop = loop;
			if(startNow)
				StartTimer();
		}

		[HorizontalGroup("butt")]
		[Button, DisableInEditorMode]
		public void Stop()
		{
			paused = true;
			if (timerRoutine != null)
				StopCoroutine(timerRoutine);
		}

		[HorizontalGroup("butt")]
		[Button, DisableInEditorMode]
		public void Restart()
			=> Initialize(TimerDuration, loop, startNow: true);

		[HorizontalGroup("butt")]
		[Button("Start"), DisableInEditorMode]
		public void StartTimer()
		{
			paused = false;
			if (timerRoutine != null) //prevent multiple coroutines.
				StopCoroutine(timerRoutine);
			timerRoutine = StartCoroutine(TickTimer());
		}

		/// <summary>
		/// Will nearly immediately complete, just not right away.
		/// </summary>
		public void ForceCompleteEventually()
		{
			TimeRemaining.Value = 0;
		}

		/// <summary>
		/// tick... tick... tick...
		/// </summary>
		/// <returns></returns>
		/// <remarks>TODO - Could greatly reduce Coroutine iteration by using WaitForSeconds
		/// and calculating the runtime values in the functions.</remarks>
		private IEnumerator TickTimer()
		{
			do // loop timer
			{
				TimeRemaining.Value = TimerDuration;//reset timer
				TimeElapsed = 0;

				//countdown loop
				while (TimeRemaining > 0)
				{
					yield return null;//wait for next frame
					var scaledDeltaTime = RichAppController.DeltaTime * TimeScale;
					TimeElapsed += scaledDeltaTime;//track total time (in case duration was modified while running)
					TimeRemaining.Value -= scaledDeltaTime;//tick... tick... tick...
				}
				TimeRemaining.Value = 0;//set to prevent overshoot
				onTimerExpire.Invoke(); //ring ring ring
			} while (loop);
		}

		#region Constructors

		/// <summary>
		/// Creates a new inactive Timer.
		/// </summary>
		/// <returns></returns>
		public static Timer Construct()
			=> new GameObject(nameof(Timer)).AddComponent<Timer>();

		/// <summary>
		/// Creates a new inactive Timer.
		/// </summary>
		/// <returns></returns>
		public static Timer Construct(GameObject ownerGameObject)
			=> ownerGameObject.AddComponent<Timer>();

		/// <summary>
		/// Creates a new inactive Timer.
		/// </summary>
		/// <returns></returns>
		public static Timer Construct(Component siblingComponent)
			=> siblingComponent.gameObject.AddComponent<Timer>();

		#endregion
	}
}
