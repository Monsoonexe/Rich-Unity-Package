using RichPackage.GuardClauses;
using System;

namespace RichPackage.InputSystem
{
    public class ActionInputContext : IInputContext
    {
        private readonly string name;
        private readonly Action onEnter;
        private readonly Action onExit;
        private readonly Action onUpdate;

        public ActionInputContext(string name, Action onEnter = null, Action onExit = null, Action onUpdate = null)
        {
            GuardAgainst.ArgumentIsNull(name, nameof(name));
            this.name = name;
            this.onEnter = onEnter;
            this.onExit = onExit;
            this.onUpdate = onUpdate;
        }

        #region IInputContext

        public string Name { get => name; }
        public void Execute() => onUpdate?.Invoke();
        public void OnEnter() => onEnter?.Invoke();
        public void OnExit() => onExit?.Invoke();

        #endregion IInputContext
    }
}
