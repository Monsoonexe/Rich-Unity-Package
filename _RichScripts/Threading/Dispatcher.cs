using System;
using System.Collections.Concurrent;
using UnityEngine;
 
 namespace RichPackage.Threading
 {
    public class Dispatcher : RichMonoBehaviour
    {
        private static Dispatcher current;

        private BlockingCollection<Action> blockingCollection;

        protected override void Reset()
        {
            SetDevDescription("Thread-safe main thread dispatcher.");
        }

        protected override void Awake()
        {
            InitSingleton(this, ref current)
            blockingCollection = new BlockingCollection<Action>();
        }
        
        private void Update()
        {
            Action action;
            while (blockingCollection.TryTake(out action)) action();
        }

        private private void OnDestroy()
        {
            ReleaseSingleton(this, ref current);
            blockingCollection.Dispose();
        }
    
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
