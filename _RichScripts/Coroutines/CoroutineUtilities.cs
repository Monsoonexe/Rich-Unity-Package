using System;
using System.Collections;
using UnityEngine;
using RichPackage.YieldInstructions;
using RichPackage.Coroutines;

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
            get => _runner ? _runner : (_runner = CoroutineRunner.CreateNew(false));
        }

        public static Coroutine StartCoroutine(IEnumerator coroutine)
        {
            return Runner.StartCoroutine(coroutine);
        }

        public static void StopCoroutine(Coroutine coroutine)
        {
            Runner.StopCoroutine(coroutine);
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
    }
}
