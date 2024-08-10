namespace RichPackage.InputSystem
{
    /// <summary>
    /// A context where input can be directed towards different consumers.
    /// </summary>
    /// <remarks>The ui consuming "x" input to "submit" is different from the gameplay consuming "x" to "jump".</remarks>
    public abstract class BasicInputContext : IInputContext
    {
        public abstract void Execute();
        public void OnEnter() { }
        public void OnExit() { }
        public string Name => GetType().Name;
    }
}
