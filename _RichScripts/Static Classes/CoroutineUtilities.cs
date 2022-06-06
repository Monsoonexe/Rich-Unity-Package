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
            //TODO - used unmarshalled Time.
            float endTime = Time.time + delay;

            do yield return null;
            while (Time.time < endTime);
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
            while (frameDelay > 0)
                yield return null;
            action();
        }
    }
}
