
namespace RichPackage.UnityMessages
{
    /// <summary>
    /// Raises UnityEvent on FixedUpdate(). Rig it in Inspector.
    /// </summary>
    public sealed class FixedUpdateUnityMessage : AUnityLifetimeMessage
    {
        protected override void Reset()
        {
            base.Reset();
            SetDevDescription($"Invokes {nameof(lifetimeEvent)} on {nameof(FixedUpdate)}.");
        }

        private void FixedUpdate()
        {
            lifetimeEvent.Invoke();
        }
    }
}
