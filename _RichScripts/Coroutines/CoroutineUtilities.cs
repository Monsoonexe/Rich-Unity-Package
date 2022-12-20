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
        private static MonoBehaviour _runner;
        
        public static MonoBehaviour Runner
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _runner ? _runner : (_runner = CoroutineRunner.CreateNew(false));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Coroutine StartCoroutine(IEnumerator coroutine)
        {
            return Runner.StartCoroutine(coroutine);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void StopCoroutine(Coroutine coroutine)
        {
            Runner.StopCoroutine(coroutine);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Coroutine StartProcessLoop(Action process,
            YieldInstruction yieldInstruction = null)
        {
            return Runner.StartCoroutine(ProcessLoop(process, yieldInstruction));
        }

        public static IEnumerator ProcessLoop(Action process,
            YieldInstruction yieldInstruction = null)
        {
            while (true)
            {
                process();
                yield return yieldInstruction;
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
        public static void StopCoroutineSafely(Coroutine coroutine)
        {
            if (coroutine != null)
                StopCoroutine(coroutine);
        }
        
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal static void StopCoroutineSafely(ref Coroutine coroutine)
        {
            if (coroutine != null)
            {
                StopCoroutine(coroutine);
                coroutine = null;
            }
        }
    }
}
