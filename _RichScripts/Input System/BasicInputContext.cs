namespace RichPackage.InputSystem
{
    public abstract class BasicInputContext : IInputContext
    {
        public abstract void Execute();
        public void OnEnter() { }
        public void OnExit() { }
        public string Name => GetType().Name;
    }
}
