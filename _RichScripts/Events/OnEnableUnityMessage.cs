
namespace RichPackage.UnityMessages
{
    /// <summary>
    /// Raises UnityEvent on OnEnable(). Rig it in Inspector.
    /// </summary>
    public sealed class OnEnableUnityMessage : AUnityLifetimeMessage
    {
        protected override void Reset()
        {
            base.Reset();
            SetDevDescription($"Invokes {nameof(lifetimeEvent)} on {nameof(OnEnable)}.");
        }

        private void OnEnable()
        {
            lifetimeEvent.Invoke();
        }
    }
}
