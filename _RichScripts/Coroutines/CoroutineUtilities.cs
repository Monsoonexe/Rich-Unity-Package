using System;
using System.Collections;
using UnityEngine;
using RichPackage.YieldInstructions;
using RichPackage.Coroutines;
using System.Runtime.CompilerServices;

namespace RichPackage
{
    /// <summary>
    /// Utilities for quickly kicking off common coroutines.
    /// </summary>
    public static class CoroutineUtilities
    {
        private static CoroutineRunner _runner;
        
        public static MonoBehaviour Runner
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _runner ? _runner : (_runner = CoroutineRunner.CreateNew(false));
        }

        public static Coroutine Delay(float delay)
        {
            return Runner.StartCoroutine(DelayLoop(delay));
        }

        public static IEnumerator DelayLoop(float _delay)
        {
            var timer = SimpleTimer.StartNew();
            while (timer < _delay)
                yield return null;
        }

        public static IEnumerator ProcessLoop(Action process,
            YieldInstruction yieldInstruction = null)
        {
            while (true)
            {
                yield return yieldInstruction;
                process();
            }
        }

        public static IEnumerator InvokeAtEndOfFrame(Action action)
        {
            yield return CommonYieldInstructions.WaitForEndOfFrame;
            action();
        }

        public static IEnumerator InvokeNextFrame(Action action)
        {
            yield return null;
            action();
        }

        /// <summary>
        /// Non-allocating.
        /// </summary>
        public static IEnumerator InvokeAfter(
            Action action, float delay)
        {
            var timer = SimpleTimer.StartNew();

            do yield return null;
            while (timer < delay);
            action();
        }

        public static IEnumerator InvokeAfter(Action action,
            YieldInstruction yieldInstruction)
        {
            yield return yieldInstruction;
            action();
        }
        
        public static IEnumerator InvokeAfterFrameDelay(Action action, int frameDelay)
        {
            while (frameDelay-- > 0)
                yield return null;
            action();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Coroutine StartCoroutine(IEnumerator coroutine)
        {
            return Runner.StartCoroutine(coroutine);
        }

        /// <summary>
        /// Routinely runs <paramref name="action"/> like Update at the specified <paramref name="interval"/>.
        /// </summary>
        public static Coroutine Update(Action action, float interval = 0)
        {
            return Runner.StartCoroutine(ProcessLoop(action, new WaitForSeconds(interval)));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Coroutine StartProcessLoop(Action process,
            YieldInstruction yieldInstruction = null)
        {
            return Runner.StartCoroutine(ProcessLoop(process, yieldInstruction));
        }

        public static Coroutine StartInvokeAfter(Action action, YieldInstruction yieldInstruction)
        {
            return Runner.StartCoroutine(InvokeAfter(action, yieldInstruction));
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void StopCoroutine(Coroutine coroutine)
        {
            Runner.StopCoroutine(coroutine);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void StopCoroutineSafely(Coroutine coroutine)
        {
            if (coroutine != null)
                StopCoroutine(coroutine);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void StopCoroutineSafely(ref Coroutine coroutine)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
        }

        /// <summary>
        /// Starts a coroutine that completes when <paramref name="query"/> returns <see langword="true"/>.
        /// </summary>
        public static Coroutine WaitUntil(Func<bool> query)
        {
            IEnumerator Wait(Func<bool> _query)
            {
                yield return new WaitUntil(_query);
            }

            return Runner.StartCoroutine(Wait(query));
        }
    }
}
