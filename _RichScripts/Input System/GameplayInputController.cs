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

        protected virtual void Start()
        {
            if (context == null && !TryGetComponent(out context))
                context = new NullInputContext();
        }

        protected void Update()
        {
            context.Execute();
        }

        public void Set(IInputContext context)
        {
            this.context = context;
        }

        #region Push/Pop

        public void Push(IInputContext context)
        {
            stash.Push(context);
            Set(context);
        }

        public void Pop()
        {
            Set(stash.Pop());
        }

        #endregion Push/Pop
    }

    /// <summary>
    /// A gameplay context that requires unique input.
    /// </summary>
    public interface IInputContext
    {
        /// <summary>
        /// Run the input logic. Use calls to UnityEngine.Input.
        /// </summary>
        void Execute();
    }
}
