
namespace RichPackage.UnityMessages
{
    /// <summary>
    /// Raises UnityEvent on LateUpdate(). Rig it in Inspector.
    /// </summary>
    public sealed class LateUpdateUnityMessage : AUnityLifetimeMessage
    {
        protected override void Reset()
        {
            base.Reset();
            SetDevDescription($"Invokes {nameof(lifetimeEvent)} on {nameof(LateUpdate)}.");
        }

        private void LateUpdate()
        {
            lifetimeEvent.Invoke();
        }
    }
}
