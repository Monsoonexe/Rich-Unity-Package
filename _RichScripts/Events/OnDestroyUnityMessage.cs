
namespace RichPackage.UnityMessages
{
    /// <summary>
    /// Raises UnityEvent on OnDestroy(). Rig it in Inspector.
    /// </summary>
    public sealed class OnDestroyUnityMessage : AUnityLifetimeMessage
    {
        protected override void Reset()
        {
            base.Reset();
            SetDevDescription($"Invokes {nameof(lifetimeEvent)} on {nameof(OnDestroy)}.");
        }

        private void OnDestroy()
        {
            lifetimeEvent.Invoke();
            lifetimeEvent.RemoveAllListeners();
        }
    }
}
