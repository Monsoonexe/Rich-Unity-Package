namespace RichPackage.InputSystem
{
    /// <summary>
    /// An input context that exists in the scene. <br/>
    /// Allows self-driving through 'Update'.
    /// </summary>
    public abstract class MonoInputContext : RichMonoBehaviour, IInputContext
    {
        #region IInputContext

        public virtual string Name => GetType().Name;

        /// <remarks>Override 'Update' instead.</remarks>
        public void Execute() { }

        public void OnEnter() => enabled = true;

        public void OnExit() => enabled = false;

        #endregion IInputContext

        #region Unity Messages

        protected override void Reset()
        {
            enabled = false; // to avoid overlap
            SetDevDescription("An input context that exists in the scene.");
        }

        protected abstract void Update();

        #endregion Unity Messages
    }
}
