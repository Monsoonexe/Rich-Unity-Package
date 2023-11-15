namespace RichPackage.InputSystem
{
    /// <summary>
    /// Does nothing.
    /// </summary>
    public sealed class NullInputContext : IInputContext
    {
        public void Execute() { } // do nothing
    }
}
