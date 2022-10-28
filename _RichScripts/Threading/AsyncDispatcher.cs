using Cysharp.Threading.Tasks;
using System;

namespace RichPackage.Threading
{
    public partial class Dispatcher : IDispatcher, IDisposable
    {
        /// <summary>
        /// Thread-safe enqueue work. Will be called on the next coroutine update.
        /// </summary>
        /// <param name="delay">Milliseconds.</param>
        public void Invoke(Action action, int delay)
        {
            InvokeDelayedInternal(action, delay).Forget();
        }

        private async UniTaskVoid InvokeDelayedInternal(Action action, int delay)
        {
            await UniTask.Delay(delay);
            SafeInvoke(action);
        }

        /// <summary>
        /// Thread-safe enqueue work. Will be called on the next coroutine update.
        /// </summary>
        public UniTask BeginInvoke(Action action)
        {
            var tcs = new UniTaskCompletionSource();
            current.messageQueue.Add(() =>
            {
                try
                {
                    action();
                    tcs.TrySetResult();
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            });

            return tcs.Task;
        }

        /// <summary>
        /// Thread-safe enqueue work. Will be called on the next coroutine update.
        /// </summary>
        public UniTask<T> BeginInvoke<T>(Func<T> function)
        {
            var tcs = new UniTaskCompletionSource<T>();
            current.messageQueue.Add(() =>
            {
                try
                {
                    T result = function(); // local variable for debugging
                    tcs.TrySetResult(result);
                }
                catch (Exception ex)
                {
                    tcs.TrySetException(ex);
                }
            });

            return tcs.Task;
        }
    }
}
