using System;
using System.Threading;
using UnityEngine.Events;

namespace Cysharp.Threading.Tasks
{
    /// <summary>
    /// 
    /// </summary>
    public static class UniTaskExtensions2
    {
        public static UniTask ToUniTask(this UnityEvent e)
        {
            var tcs = new UniTaskCompletionSource();
            e.AddListener(OnInvoke);
            void OnInvoke()
            {
                e.RemoveListener(OnInvoke);
                tcs.TrySetResult();
            }

            return tcs.Task;
        }

        public static UniTask<T> ToUniTask<T>(this UnityEvent<T> e)
        {
            var tcs = new UniTaskCompletionSource<T>();
            e.AddListener(OnInvoke);
            void OnInvoke(T r)
            {
                e.RemoveListener(OnInvoke);
                tcs.TrySetResult(r);
            }

            return tcs.Task;
        }

        public static UniTask OnInvokeAsync(this UnityEvent e)
        {
            return e.OnInvokeAsync(CancellationToken.None);
        }

        public static async UniTask WaitUntil(this Func<bool> condition,
            CancellationToken cancellationToken = default)
        {
            while (!(cancellationToken.IsCancellationRequested || condition()))
                await UniTask.Yield();

            cancellationToken.ThrowIfCancellationRequested();
        }

        public static UniTask LoopAsync(Action action, Func<bool> condition, float interval,
            CancellationToken cancellationToken = default)
            => LoopAsync(action, condition, TimeSpan.FromSeconds(interval), cancellationToken);

        public static async UniTask LoopAsync(Action action, Func<bool> condition,
            TimeSpan interval, CancellationToken cancellationToken = default)
        {
            cancellationToken.ThrowIfCancellationRequested();

            while (condition() && !cancellationToken.IsCancellationRequested)
            {
                action();
                await UniTask.Delay(interval, cancellationToken: cancellationToken);
            }

            cancellationToken.ThrowIfCancellationRequested();
        }
    }
}
