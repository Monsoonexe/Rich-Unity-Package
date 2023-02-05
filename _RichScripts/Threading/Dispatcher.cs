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
        private static IDispatcher current;
        public static IDispatcher Current { get => current ?? StartNew(); }
        public static readonly YieldInstruction DefaultRunTiming = new WaitForEndOfFrame();

        private readonly ConcurrentQueue<Action> jobQueue;
        private AtomicCounter jobCounter;

        /// <summary>
        /// Jobs that are pending.
        /// </summary>
        public int PendingJobCount => jobCounter.UnsafeValue; // if we miss a job, we'll get it next loop

        private Coroutine updateRoutine;
        private readonly MonoBehaviour coroutineRunner;

        /// <summary>
        /// Describes the timing at which to run the queue. <br/>
        /// Default is <see cref="DefaultRunTiming"/>.
        /// </summary>
        public YieldInstruction RunTiming;

        /// <summary>
        /// The backing value for <see cref="JobsPerFrame"/>.
        /// </summary>
        private int _jobsPerFrame = 1;

        /// <summary>
        /// How many jobs the dispatcher will run per frame before yielding the thread. <br/>
        /// A negative value indicates that the dispatcher will run all jobs
        /// in the queue before yielding the thread. <br/>
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
            jobQueue = new ConcurrentQueue<Action>();
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
            GC.SuppressFinalize(this);
        }

        private bool IsWorkAvailable()
        {
            return PendingJobCount > 0;
        }

        private void SafeInvoke(Action action)
        {
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
                if (!IsWorkAvailable())
                    yield return waitForWork;
                
                if (RunTiming != null)
                    yield return RunTiming;
                
                int jobs = 0;
                while ((jobs++ < JobsPerFrame) && jobQueue.TryDequeue(out Action action))
                    SafeInvoke(action);

                yield return null;
            }
        }

        public void Run()
        {
            if (!Enabled)
            {
                updateRoutine = coroutineRunner.StartCoroutine(Update());
            }
        }

        public void Stop()
        {
            if (Enabled)
            {
                coroutineRunner.StopCoroutineSafely(updateRoutine);
                updateRoutine = null;
            }
        }

        /// <summary>
        /// Thread-safe enqueue work. Will be called on the next coroutine update.
        /// </summary>
        public void Invoke(Action action)
        {
            jobQueue.Enqueue(action);
            ++jobCounter;
        }

        public static IDispatcher StartNew()
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
