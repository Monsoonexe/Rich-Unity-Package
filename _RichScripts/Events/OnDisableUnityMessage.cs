
namespace RichPackage.UnityMessages
{
    /// <summary>
    /// Raises UnityEvent on OnDisable(). Rig it in Inspector.
    /// </summary>
    public sealed class OnDisableUnityMessage : AUnityLifetimeMessage
    {
        protected override void Reset()
        {
            base.Reset();
            SetDevDescription($"Invokes {nameof(lifetimeEvent)} on {nameof(OnDisable)}.");
        }

        private void OnDisable()
        {
            lifetimeEvent.Invoke();
        }
    }
}
