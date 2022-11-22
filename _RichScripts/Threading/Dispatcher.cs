using System;
using System.Collections;
using System.Collections.Concurrent;
using UnityEngine;
 
 namespace RichPackage.Threading
 {
    /// <summary>
    /// Dispatcher to Unity's Main Thread.
    /// </summary>
    public partial class Dispatcher : IDispatcher, IDisposable
    {
        private static Dispatcher current;
        public static Dispatcher Current { get => current ?? CreateStatic(); }
        public static readonly YieldInstruction DefaultRunTiming = new WaitForEndOfFrame();

        private readonly BlockingCollection<Action> messageQueue;

        private Coroutine updateRoutine;
        private readonly MonoBehaviour coroutineRunner;

        /// <summary>
        /// Describes the timing at which to run the queue. <br/>
        /// Default is <see cref="DefaultRunTiming"/>.
        /// </summary>
        public YieldInstruction RunTiming;

        private int _jobsPerFrame = 1;

        /// <summary>
        /// Currently has no effect and is locked at 1.
        /// </summary>
        public int JobsPerFrame
        {
            get => _jobsPerFrame;
            set
            {
                if (value < 0)
                    value = int.MaxValue;
                else if (value == 0)
                    throw new ArgumentException($"0 is not a valid value for {nameof(JobsPerFrame)}.");
                _jobsPerFrame = value;
            }
        }
        
        public bool Enabled
        {
            get => updateRoutine != null;
            set
            {
                if (value && !Enabled)
                    Run();
                else if (!value && Enabled)
                    Stop();
            }
        }

        #region Constructors

        public Dispatcher() : this(RichAppController.Instance) // something static
        {
            // exists
        }

        public Dispatcher(MonoBehaviour coroutineRunner)
        {
            this.coroutineRunner = coroutineRunner;
            messageQueue = new BlockingCollection<Action>();
            RunTiming = DefaultRunTiming;
        }

        ~Dispatcher()
        {
            Dispose();
        }

        #endregion Constructors

        public void Dispose()
        {
            Stop();
            messageQueue.Dispose();
            GC.SuppressFinalize(this);
        }

        private bool IsWorkAvailable()
        {
            return messageQueue.Count > 0;
        }

        private void SafeInvoke(Action action)
        {
            // exception safety wrapper
            try
            {
                action();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex);
            }
        }

        private IEnumerator Update()
        {
            var waitForWork = new WaitUntil(IsWorkAvailable);

            while (true)
            {
                // TODO - add options for how many jobs can be processed in single frame
                yield return RunTiming;
                if (messageQueue.TryTake(out Action action))
                    SafeInvoke(action);
                else
                    yield return waitForWork;
            }
        }

        public void Run()
        {
            updateRoutine = coroutineRunner.StartCoroutine(Update());
        }

        public void Stop()
        {
            coroutineRunner.StopCoroutineSafely(updateRoutine);
            updateRoutine = null;
        }

        /// <summary>
        /// Thread-safe enqueue work. Will be called on the next coroutine update.
        /// </summary>
        public void Invoke(Action action)
        {
            messageQueue.Add(action);
        }

        public static Dispatcher CreateStatic()
        {
            current = new Dispatcher();
            current.Run();
            return current;
        }
    }
}

    /*
    // Or, on main thread, during initialization:
    var syncContext = System.Threading.SynchronizationContext.Current;

    // On your worker thread
    syncContext.Post(_ =>
    {
        // This code here will run on the main thread
        Debug.Log("Hello from main thread!");
    }, null);

    */
