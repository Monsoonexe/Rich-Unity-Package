using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
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

        private readonly BlockingCollection<Action> jobQueue;

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
            jobQueue = new BlockingCollection<Action>();
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
            jobQueue.Dispose();
            GC.SuppressFinalize(this);
        }

        private bool IsWorkAvailable()
        {
            return jobQueue.Count > 0;
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
                if (RunTiming != null)
                    yield return RunTiming;
                
                int jobs = 0;
                while ((jobs++ < JobsPerFrame) && jobQueue.TryTake(out Action action))
                    SafeInvoke(action);
                
                yield return waitForWork;
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
            jobQueue.Add(action);
        }

        public static IDispatcher StartNew()
        {
            current = new Dispatcher();
            current.Run();
            return current;
        }
    }
    
    /// <summary>
    /// Thread-safe counter.
    /// </summary>
    internal class AtomicCounter
    {
        private int counter;

        public AtomicCounter() : this(0) { }

        public AtomicCounter(int value)
        {
            counter = value;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Set(int x)
        {
            Interlocked.Exchange(ref counter, x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Add(int x)
        {
            return Interlocked.Add(ref counter, x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Sub(int x)
        {
            return Interlocked.Add(ref counter, -x);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Increment()
        {
            return Interlocked.Increment(ref counter);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int Decrement()
        {
            return Interlocked.Decrement(ref counter);
        }

        public int Value
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Interlocked.Add(ref counter, 0);

            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            set => Interlocked.Exchange(ref counter, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator int(AtomicCounter a) => a.Value;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static AtomicCounter operator --(AtomicCounter a)
        {
            Interlocked.Decrement(ref a.counter);
            return a;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static AtomicCounter operator ++(AtomicCounter a)
        {
            Interlocked.Increment(ref a.counter);
            return a;
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
