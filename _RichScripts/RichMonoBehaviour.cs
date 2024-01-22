using System;
using System.Diagnostics;
using UnityEngine;
using Sirenix.OdinInspector;
using System.Runtime.CompilerServices;
using System.Collections;

namespace RichPackage
{
    /// <summary>
    /// Base class that includes helper methods for MonoBehaviour.
    /// </summary>
    /// <seealso cref="RichScriptableObject"/>
    [SelectionBase]
    public abstract partial class RichMonoBehaviour : MonoBehaviour
    {
#if UNITY_EDITOR
        [SerializeField, TextArea]
        [PropertyOrder(-5)]
#pragma warning disable IDE0052 // Remove unread private members
        private string developerDescription = "Please enter a description or a note.";
#pragma warning restore IDE0052 // Remove unread private members
#endif

        /// <summary>
        /// This call will be stripped out of Builds. Use anywhere.
        /// </summary>
        /// <param name="newDes"></param>
        [Conditional(ConstStrings.UNITY_EDITOR)]
        public void SetDevDescription(string newDes)
        {
#if UNITY_EDITOR
            developerDescription = newDes;
#endif
        }

        /// <summary>
        /// Cached Transform.
        /// </summary>
        [SerializeField, HideInInspector]
        protected Transform myTransform;

        /// <summary>
        /// Cached Transform  
        /// (because native 'transform' is secretly <see cref="GameObject.GetComponent{Transform}"/>' which is expensive on repeat).
        /// </summary>
        public new Transform transform
        {
            get => myTransform ? myTransform : (myTransform = base.transform);
        }

        #region Unity Messages

        protected virtual void Reset()
        {
            myTransform = gameObject.transform;
        }

        protected virtual void Awake()
        {
            if (!myTransform)
                myTransform = gameObject.transform;
        }

        #endregion Unity Messages

        #region Invokation Timing Helpers

        protected Coroutine InvokeAtEndOfFrame(Action action)
        {
            return StartCoroutine(CoroutineUtilities.InvokeAtEndOfFrame(action));
        }

        protected Coroutine InvokeNextFrame(Action action)
        {
            return StartCoroutine(CoroutineUtilities.InvokeNextFrame(action));
        }

        protected Coroutine Invoke(Action action, float delay_s)
        {
            return StartCoroutine(CoroutineUtilities.InvokeAfter(action, delay_s));
        }

        /// <summary>
        /// Runs <paramref name="action"/> after waiting on <paramref name="yieldInstruction"/>.
        /// </summary>
        protected Coroutine Invoke(Action action, YieldInstruction yieldInstruction)
        {
            return StartCoroutine(CoroutineUtilities.InvokeAfter(action, yieldInstruction));
        }

        /// <summary>
        /// Runs <paramref name="action"/> after waiting on <paramref name="yieldInstruction"/>.
        /// </summary>
        protected Coroutine InvokeAfterDelay(Action action, YieldInstruction yieldInstruction)
        {
            return StartCoroutine(CoroutineUtilities.InvokeAfter(action, yieldInstruction));
        }

        protected Coroutine InvokeAfterFrameDelay(Action action, int frameDelay)
        {
            return StartCoroutine(CoroutineUtilities.InvokeAfterFrameDelay(action, frameDelay));
        }

        #endregion Invokation Timing Helpers

        #region GetComponent Helpers

        protected void FindComponentIfNull<T>(ref T component, bool includeInactive = true, bool addIfNotFound = false)
            where T : class // don't constrain to Component so we can look for interfaces, too
        {
            if (component != null)
                return;

            // null-coalesce should be okay here because we're not using the result of functions, not serialized properties
            component = GetComponent<T>()
                ?? GetComponentInChildren<T>(includeInactive)
                ?? GetComponentInParent<T>(includeInactive)
                ?? FindObjectOfType(typeof(T), includeInactive) as T;
        }
 
        protected T GetComponentInChildrenIfNull<T>(Maybe<T> maybeComponent)
            where T : Component
        {
            T comp;
            return ((comp = maybeComponent.GetValueOrDefault()) != null)
                ? comp : gameObject.GetComponentInChildren<T>();
        }

        /// <summary>
        /// Try to get component on self, in children, and then add it on self if not found.
        /// </summary>
        protected T GetComponentOnPrefabAtAllCosts<T>(Maybe<T> maybeComponent)
            where T : Component
        {
            T comp;
            if ((comp = maybeComponent.GetValueOrDefault()) != null)
                return comp;
            if ((comp = gameObject.GetComponent<T>()) != null)
                return comp;
            if ((comp = GetComponentInChildren<T>()) != null)
                return comp;
            if ((comp = GetComponentInParent<T>()) != null)
                return comp;

            return gameObject.AddComponent<T>();
        }

        #endregion GetComponent Helpers

        #region Coroutine Helpers

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void StopCoroutineSafely(Coroutine coroutine)
        {
            CoroutineUtilities.StopCoroutineSafely(coroutine);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        protected void StopCoroutineSafely(ref Coroutine coroutine)
        {
            CoroutineUtilities.StopCoroutineSafely(ref coroutine);
        }

        protected Coroutine InvokeWhen(Action action, Func<bool> condition)
        {
            IEnumerator Wait(Action _action, Func<bool> _condition)
            {
                yield return new WaitUntil(_condition);
                _action();
            }

            return StartCoroutine(Wait(action, condition));
        }

        #endregion Coroutine Helpers

        protected async Cysharp.Threading.Tasks.UniTask WaitUntil(Func<bool> condition)
        {
            while (this != null && !condition())
                await Cysharp.Threading.Tasks.UniTask.Yield();
        }

        public override string ToString() => $"{name} ({GetType().Name})";

        /// <summary>
        /// Creates a new <see cref="GameObject"/> with the given 
        /// <typeparamref name="T"/> component attached.
        /// </summary>
        /// <returns>The newly created instance.</returns>
        public static T Construct<T>() where T : RichMonoBehaviour
            => Construct<T>(typeof(T).Name);

        /// <summary>
        /// Creates a new <see cref="GameObject"/> with the given 
        /// <typeparamref name="T"/> component attached.
        /// </summary>
        /// <param name="name">The name of the new <see cref="GameObject"/>.</param>
        /// <returns>The newly created instance.</returns>
        public static T Construct<T>(string name) where T : RichMonoBehaviour
            => new GameObject(name).AddComponent<T>();

        #region Logging

        [Conditional(ConstStrings.UNITY_EDITOR)]
        protected void LogNotSerializedWarning(string property)
        {
            UnityEngine.Debug.LogWarning($"Property '{property}' is not serialized. Using GetComponent for now.", this);
        }

        #endregion Logging

        #region Editor

        [Conditional(ConstStrings.UNITY_EDITOR)]
        public void Editor_MarkDirty()
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
                UnityEditor.EditorUtility.SetDirty(this);
#endif
        }

        #endregion Editor
    }
}
