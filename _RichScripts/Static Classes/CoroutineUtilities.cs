using System;
using System.Collections;
using UnityEngine;
using RichPackage.YieldInstructions;

namespace RichPackage
{
    /// <summary>
    /// Utilities for quickly kicking off common coroutines.
    /// </summary>
    public static class CoroutineUtilities
    {
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

        public static IEnumerator InvokeAfterDelay(
            Action action, float delay)
        {
            var timer = SimpleTimer.StartNew();

            do yield return null;
            while (timer < delay);
            action();
        }

        public static IEnumerator InvokeAfterDelay(Action action,
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
