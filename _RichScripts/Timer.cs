using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// A mostly-accurate timer Mono.
/// </summary>
public class Timer : RichMonoBehaviour
{
    private Coroutine timerRoutine;

    /// <summary>
    /// Total duration of timer.
    /// </summary>
    private float timerDuration = 0;

    /// <summary>
    /// Time remaining on current loop.
    /// </summary>
    private float timeRemaining = 0;

    /// <summary>
    /// Time remaining on current loop.
    /// </summary>
    public float TimeRemaining { get => timeRemaining; }

    /// <summary>
    /// How much time has passed.
    /// </summary>
    public float ElapsedTime { get => timerDuration - timeRemaining; }

    /// <summary>
    /// 
    /// </summary>
    public float PercentComplete { get => (timerDuration - timeRemaining) / timerDuration; }

    private bool paused = false;
    public bool Paused
    {
        get => paused;
        set
        {
            if (value == true)
                PauseTimer();
            else
                ResumeTimer();
        }
    }

    public bool loop = false;

    [Header("---Events---")]
    [SerializeField]
    private UnityEvent onTimerExpire = new UnityEvent();
    public UnityEvent OnTimerExpire { get => onTimerExpire; }

    private void OnDisable()
    {
        StopTimer();
    }

    public void InitTimer(float duration, Action callback, bool loop = false)
    {
        OnTimerExpire.AddListener(callback.Invoke);//
        InitTimer(duration, loop);//forward call
    }

    public void InitTimer(float duration, bool loop = false)
    {
        timerDuration = duration;
        timeRemaining = duration;
        this.loop = loop;
        paused = false;
        timerRoutine = StartCoroutine(TickTimer());
    }

    public void PauseTimer()
    {
        paused = true;
        if (timerRoutine != null)
            StopCoroutine(timerRoutine);
    } 

    public void RestartTimer()
    {
        InitTimer(timerDuration, loop);
    }

    public void ResumeTimer()
    {
        paused = false;
        timerRoutine = StartCoroutine(TickTimer());
    }

    public void StopTimer()
    {
        timerDuration = 0;
        timeRemaining = 0;
        if(timerRoutine != null)
            StopCoroutine(timerRoutine);
        paused = true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <returns></returns>
    /// <remarks>TODO - Could greatly reduce Coroutine iteration by using WaitForSeconds
    /// and calculating the runtime values in the functions.</remarks>
    private IEnumerator TickTimer()
    {
        restartTimer:
        while (timeRemaining > 0) 
        {
            yield return null;//wait for next frame
            timeRemaining -= Time.deltaTime;
        }

        onTimerExpire.Invoke(); //raise event

        if (loop)
        {
            timeRemaining = timerDuration;//reset timer
            goto restartTimer;
        }
    }

    #region Constructors

    /// <summary>
    /// Creates a new inactive Timer.
    /// </summary>
    /// <returns></returns>
    public static Timer Construct()
    {
        return new GameObject("Flip Timer").AddComponent<Timer>();
    }

    #endregion

    #region Testing
#if UNITY_EDITOR

    public void Test1SecondTimer()
    {

    }

#endif
    #endregion

}
