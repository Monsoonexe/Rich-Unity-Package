﻿using System;
using UnityEngine;
using RichPackage;

namespace RichPackage
{
    /// <summary>
    /// Extension methods for monobehaviours.
    /// </summary>
    /// <seealso cref="Component_Extensions"/>
    /// <seealso cref="Behaviour_Extensions"/>
    public static class MonoBehaviour_Extensions
    {
        #region Extensions

        public static void InvokeAtEndOfFrame(this MonoBehaviour mono,
            Action action)
            => mono.StartCoroutine(CoroutineUtilities.InvokeAtEndOfFrame(action));

        public static void InvokeNextFrame(this MonoBehaviour mono,
            Action action)
            => mono.StartCoroutine(CoroutineUtilities.InvokeNextFrame(action));

        public static void InvokeAfterDelay(this MonoBehaviour mono,
            Action action, float delay)
            => mono.StartCoroutine(CoroutineUtilities.InvokeAfterDelay(action, delay));

        public static void InvokeAfterDelay(this MonoBehaviour mono,
            Action action, YieldInstruction delay)
            => mono.StartCoroutine(CoroutineUtilities.InvokeAfterDelay(action, delay));

        #endregion

    }
}
