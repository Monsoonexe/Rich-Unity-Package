using System;
using System.Diagnostics;
using UnityEngine;
using RichPackage.Tweening;
using Sirenix.OdinInspector;
using System.Collections;

namespace RichPackage
{
    /// <summary>
    /// Common base class for scriptable objects.
    /// </summary>
    /// <seealso cref="RichMonoBehaviour"/>
    public abstract class RichScriptableObject : ScriptableObject
    {
#if UNITY_EDITOR
        [SerializeField, TextArea]
        [PropertyOrder(-5)]
#pragma warning disable IDE0052 // Remove unread private members
        private string developerDescription = "Please enter a description or a note.";
#pragma warning restore IDE0052 // Remove unread private members
#endif

		#region Coroutines

		protected Coroutine StartCoroutine(IEnumerator routine)
		{
            return CoroutineUtilities.StartCoroutine(routine);
		}

        protected void StopCoroutine(Coroutine routine)
		{
            CoroutineUtilities.StopCoroutine(routine);
		}

        protected Coroutine InvokeAtEndOfFrame(Action action)
        {
            return StartCoroutine(CoroutineUtilities.InvokeAtEndOfFrame(action));
        }

        protected Coroutine InvokeNextFrame(Action action)
        {
            return StartCoroutine(CoroutineUtilities.InvokeNextFrame(action));
        }

        protected Coroutine InvokeAfterDelay(Action action, float delay_s)
        {
            return StartCoroutine(CoroutineUtilities.InvokeAfter(action, delay_s));
        }

        protected Coroutine InvokeAfterDelay(Action action, YieldInstruction yieldInstruction)
        {
            return StartCoroutine(CoroutineUtilities.InvokeAfter(action, yieldInstruction));
        }

        protected Coroutine InvokeAfterFrameDelay(Action action, int frameDelay)
        {
            return StartCoroutine(CoroutineUtilities.InvokeAfterFrameDelay(action, frameDelay));
        }

		#endregion Coroutines

		#region Editor

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

    public abstract class RichScriptableObject<T> : RichScriptableObject
    {
        [SerializeField]
        protected T _value;

        public virtual T Value { get => _value; set => _value = value; }

        public static implicit operator T(RichScriptableObject<T> a)
            => a.Value;
    }
}
