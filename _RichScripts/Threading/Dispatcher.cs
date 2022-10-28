using RichPackage.Tweening;
using System;
using System.Collections;
using System.Collections.Concurrent;
using UnityEngine;
 
 namespace RichPackage.Threading
 {
    /// <summary>
    /// Dispatcher to Unity's Main Thread.
    /// </summary>
    public class Dispatcher
    {
        private static Dispatcher current;

        private readonly BlockingCollection<Action> blockingCollection;

        private bool active = false;

        static Dispatcher()
        {
            Singleton.InitSingleton(new Dispatcher(), ref current);
            RichTweens.Init();
            current.Run();
        }

        public Dispatcher()
        {
            blockingCollection = new BlockingCollection<Action>();
        }

        ~Dispatcher()
        {
            blockingCollection.Dispose();
        }
        
        private bool IsWorkAvailable()
        {
            return blockingCollection.Count > 0;
        }

        private IEnumerator Update()
        {
            var waitForWork = new WaitUntil(IsWorkAvailable);

            while (active)
            {
                yield return waitForWork;
                while (blockingCollection.TryTake(out Action action))
                {
                    // exception safety wrapper
                    try
                    {
                        action();
                    }
                    catch (Exception ex)
                    {
                        UnityEngine.Debug.LogException(ex);
                    }
                }
            }
        }

        private void OnDestroy()
        {
            Singleton.ReleaseSingleton(this, ref current);
            blockingCollection.Dispose();
        }

        public void Run()
        {
            active = true;
            RichTweens.StartCoroutine(Update());
        }

        public void Stop()
        {
            active = false;
        }
    
        /// <summary>
        /// Thread-safe enqueue work. Will be called on the next coroutine update.
        /// </summary>
        public static void Invoke(Action action)
        {
            current.blockingCollection.Add(action);
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
