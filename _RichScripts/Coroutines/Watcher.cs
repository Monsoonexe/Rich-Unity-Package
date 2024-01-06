using RichPackage.GuardClauses;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RichPackage
{
    /// <summary>
    /// Raises events when a variable changes value.
    /// </summary>
    public abstract class AWatcher<T>
        where T : IComparable<T>
    {
        public event Action<T> OnChanged;
        protected virtual YieldInstruction PollInterval { get; set; }

        private Coroutine routine;
        public bool repeat;

        #region Constructors

        #endregion

        public void Start()
        {
            CoroutineUtilities.StartCoroutine(WatchRoutine(GetValue()));
        }

        public void Stop()
        {
            CoroutineUtilities.StopCoroutineSafely(ref routine);
        }

        private IEnumerator WatchRoutine(T prevState = default)
        {
            EqualityComparer<T> comparer = EqualityComparer<T>.Default;
            
            do
            {
                // check for state changed
                T nextState = GetValue();
                if (!comparer.Equals(prevState, nextState))
                {
                    Invoke(nextState);
                    prevState = nextState;
                }

                yield return PollInterval;
            }
            while (ShouldRepeat());
        }

        protected virtual void Invoke(T newValue) => OnChanged?.Invoke(newValue);
        protected virtual bool ShouldRepeat() => repeat;
        public abstract T GetValue();
    }

    /// <summary>
    /// Raises events when the value of an assigned function changes.
    /// </summary>
    public class FuncWatcher<T> : AWatcher<T>
        where T : IComparable<T>
    {
        protected Func<T> getter;

        #region Constructors

        protected FuncWatcher() { }

        public FuncWatcher(Func<T> getter)
        {
            GuardAgainst.ArgumentIsNull(getter);
            this.getter = getter;
        }

        #endregion Constructors

        public override T GetValue() => getter();
    }

    /// <summary>
    /// Raises events when an animator's parameter value changes.
    /// </summary>
    public abstract class AnimatorParameterWatcher<T> : FuncWatcher<T>
        where T : IComparable<T>
    {
        public Animator animator;

        #region Constructors

        protected AnimatorParameterWatcher(Animator animator)
        {
            GuardAgainst.ArgumentIsNull(animator, nameof(animator));
            this.animator = animator;
        }

        #endregion Constructors

        protected override bool ShouldRepeat() => repeat && animator; // stop checking when the animator is destroyed as it can never change vaules again
    }

    public sealed class BoolAnimatorParameterWatcher : AnimatorParameterWatcher<bool>
    {
        public BoolAnimatorParameterWatcher(Animator animator, string parameterName)
            : base(animator)
        {
            GuardAgainst.ArgumentIsNull(parameterName, parameterName);
            this.getter = () => animator.GetBool(parameterName);
        }
    }

    public sealed class FloatAnimatorParameterWatcher : AnimatorParameterWatcher<float>
    {
        public FloatAnimatorParameterWatcher(Animator animator, string parameterName)
            : base(animator)
        {
            GuardAgainst.ArgumentIsNull(parameterName, parameterName);
            this.getter = () => animator.GetFloat(parameterName);
        }
    }
}
