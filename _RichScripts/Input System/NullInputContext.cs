namespace RichPackage.InputSystem
{
    /// <summary>
    /// Does nothing.
    /// </summary>
    public sealed class NullInputContext : BasicInputContext
    {
        public override void Execute() { } // do nothing
    }
}
