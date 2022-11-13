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

        public bool Enabled { get => updateRoutine != null; }

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
                yield return waitForWork;
                yield return RunTiming;
                while (messageQueue.TryTake(out Action action))
                    SafeInvoke(action);
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
