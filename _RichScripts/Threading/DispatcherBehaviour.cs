using Cysharp.Threading.Tasks;
using System;

namespace RichPackage.Threading
{
    /// <summary>
    /// A <seealso cref="Dispatcher"/> that lives on a <see cref="UnityEngine.GameObject"/>.
    /// </summary>
    public class DispatcherBehaviour : RichMonoBehaviour, IDispatcher
    {
        private Dispatcher dispatcher;

        public bool Enabled => enabled;

        #region Unity Messages

        protected override void Awake()
        {
            base.Awake();
            dispatcher = new Dispatcher(this);
        }

        private void OnDestroy()
        {
            dispatcher.Dispose();
        }

        private void OnEnable()
        {
            dispatcher.Run();
        }

        private void OnDisable()
        {
            dispatcher.Stop();
        }

        #endregion Unity Messages

        public UniTask BeginInvoke(Action action)
        {
            return dispatcher.BeginInvoke(action);
        }

        public UniTask<T> BeginInvoke<T>(Func<T> function)
        {
            return dispatcher.BeginInvoke(function);
        }

        public void Invoke(Action action)
        {
            dispatcher.Invoke(action);
        }

        public void Invoke(Action action, int delay)
        {
            dispatcher.Invoke(action, delay);
        }

        /// <summary>
        /// Begin message pumping.
        /// </summary>
        public void Run()
        {
            enabled = true;
        }

        public void Stop()
        {
            enabled = false;
        }
    }
}
