using RichPackage.GuardClauses;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RichPackage.InputSystem
{
    /// <summary>
    /// Controls input contexts. There is only ever 1 <see cref="IInputContext"/> active at a given time.
    /// </summary>
    public class GameplayInput : RichMonoBehaviour
    {
        protected readonly Stack<IInputContext> stash = new Stack<IInputContext>();
        protected IInputContext context;

        public IInputContext Context { get => context; set => Set(value); }

        [Tooltip("Prints enter logs.")]
        public bool debug;

        [ShowInInspector]
        public string CurrentContextName => Context?.Name ?? "none";

        #region Unity Messages

        protected virtual void Start()
        {
            if (context == null && !TryGetComponent(out context))
                context = new NullInputContext();
            context.OnEnter();
        }

        protected virtual void OnDestroy()
        {
            stash.Clear();
            context = null;
        }

        protected virtual void Update()
        {
            context.Execute();
        }

        #endregion Unity Messages

        #region Context Management

        public void ClearHistory()
        {
            stash.Clear();
        }

        public void Pop()
        {
            Set(stash.Pop());
        }

        public void Push(IInputContext newContext)
        {
            stash.Push(context);
            Set(newContext);
        }

        public void Push(Action function)
        {
            Push(new ActionInputContext(function));
        }

        public void Push(Action<GameplayInput> function)
        {
            Push(() => function(this));
        }

        public void Set(IInputContext context)
        {
            // validate
            GuardAgainst.ArgumentIsNull(context, nameof(context));

            // log
            if (debug)
            {
                DebugLogName($"Context switch from '{CurrentContextName}' to '{context.Name}'");
            }

            // business
            this.context.OnExit();
            this.context = context;
            this.context.OnEnter();
        }

        #endregion Context Management
    }

    /// <summary>
    /// A gameplay context that requires unique input.
    /// </summary>
    public interface IInputContext
    {
        /// <summary>
        /// The state was entered.
        /// </summary>
        void OnEnter();

        /// <summary>
        /// The state was exited.
        /// </summary>
        void OnExit();

        /// <summary>
        /// Run the input logic. Use calls to UnityEngine.Input.
        /// </summary>
        void Execute();

        /// <summary>
        /// Unique identifier. Mostly used for debugging, but could be used for dynamic lookup.
        /// </summary>
        string Name { get; }
    }
}
