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

        public abstract void Execute();

        public void OnEnter() => enabled = true; // mostly just indicator in Inspector

        public void OnExit() => enabled = false;

        #endregion IInputContext

        #region Unity Messages

        protected override void Reset()
        {
            enabled = false; // to avoid overlap
            SetDevDescription("An input context that exists in the scene.");
        }

#if UNITY_EDITOR

        /// <remarks>You're not allowed to use this directly.</remarks>
#pragma warning disable UNT0001 // Empty Unity message
        protected void Update() { }
#pragma warning restore UNT0001 // Empty Unity message

#endif

        #endregion Unity Messages
    }
}
