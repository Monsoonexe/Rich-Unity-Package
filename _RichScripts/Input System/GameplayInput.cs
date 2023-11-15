using System.Collections.Generic;

namespace RichPackage.InputSystem
{
    /// <summary>
    /// Controls input contexts. There is only ever 1 <see cref="IInputContext"/> active at a given time.
    /// </summary>
    public class GameplayInput : RichMonoBehaviour
    {
        protected readonly Stack<IInputContext> stash = new Stack<IInputContext>();
        private IInputContext context;

        public IInputContext Context { get => context; set => Set(value); }

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

        public void Set(IInputContext context)
        {
            this.context.OnExit();
            this.context = context;
            this.context.OnEnter();
        }

        public void Push(IInputContext context)
        {
            stash.Push(context);
            Set(context);
        }

        public void Pop()
        {
            Set(stash.Pop());
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
    }

    public abstract class BasicInputContext : IInputContext
    {
        public abstract void Execute();
        public void OnEnter() { }
        public void OnExit() { }
    }
}
