using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

/// <summary>
/// A mostly-accurate timer Mono.
/// </summary>
public class Timer : RichMonoBehaviour
{
    private Coroutine timerRoutine;

    /// <summary>
    /// Total duration of timer. TOP
    /// </summary>
    [Tooltip("Total duration of timer. TOP")]
    public float TimerDuration = 0;

    /// <summary>
    /// Time remaining on current loop. COUNTDOWN
    /// </summary>
    [Tooltip("Time remaining on current loop. COUNTDOWN")]
    public float TimeRemaining = 0;

    /// <summary>
    /// Accumulated time that has elapsed since last Restart() or Init().
    /// </summary>
    public float TimeEllapsed { get; private set; }

    /// <summary>
    /// 0 will effectively pause effect but won't pause coroutine.
    /// </summary>
    [Tooltip("0 will effectively pause effect but won't pause coroutine.")]
    public float timeScale = 1.0f;

    [ShowNativeProperty]
    public float PercentComplete { get => (TimerDuration - TimeRemaining) / TimerDuration; }

    private bool paused = false;
    public bool Paused
    {
        get => paused;
        set
        {
            if (value == true)
                Stop();
            else
                Resume();
        }
    }

    public bool loop = false;

    [Header("---Events---")]
    [SerializeField]
    private UnityEvent onTimerExpire = new UnityEvent();
    public UnityEvent OnTimerExpire { get => onTimerExpire; }

    private void OnDisable()
    {
        Stop();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="duration">Length of timer.</param>
    /// <param name="callback">Override current callback with this one.</param>
    /// <param name="loop">Auto-repeat at end?</param>
    public void Initialize(float duration, Action callback, bool loop = false)
    {
        Debug.Assert(callback != null,
            "Callback is null and shouldn't be.", this);

        OnTimerExpire.RemoveAllListeners();
        OnTimerExpire.AddListener(callback.Invoke);//
        Initialize(duration, loop);//forward call
    }

    public void Initialize(float duration, bool loop = false)
    {
        TimerDuration = duration;
        this.loop = loop;
        paused = false;
        timerRoutine = StartCoroutine(TickTimer());
    }

    public void Stop()
    {
        paused = true;
        if (timerRoutine != null)
            StopCoroutine(timerRoutine);
    } 

    public void Restart()
        => Initialize(TimerDuration, loop);
    
    public void Resume()
    {
        paused = false;
        timerRoutine = StartCoroutine(TickTimer());
    }

    /// <summary>
    /// Will nearly immediately complete, just not right away.
    /// </summary>
    public void ForceCompleteEventually()
    {
        TimeRemaining = 0;
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
            TimeRemaining = TimerDuration;//reset timer
            TimeEllapsed = 0;

            //countdown loop
            while (TimeRemaining > 0)
            {
                yield return null;//wait for next frame
                var deltaTime = Time.deltaTime * timeScale;
                TimeEllapsed += deltaTime;//track total time (in case duration was modified while running)
                TimeRemaining -= deltaTime;//tick... tick... tick...
            }

            onTimerExpire.Invoke(); //ring ring ring
        } while (loop);
    }

    #region Constructors

    /// <summary>
    /// Creates a new inactive Timer.
    /// </summary>
    /// <returns></returns>
    public static Timer Construct()
        => new GameObject(typeof(Timer).Name).AddComponent<Timer>();

    #endregion

    #region Testing
#if UNITY_EDITOR

    public void Test1SecondTimer()
    {

    }

#endif
    #endregion

}
