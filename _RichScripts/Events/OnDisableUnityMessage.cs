
namespace RichPackage.UnityMessages
{
    /// <summary>
    /// Raises UnityEvent on OnDisable(). Rig it in Inspector.
    /// </summary>
    public sealed class OnDisableUnityMessage : AUnityLifetimeMessage
    {
        private void OnDisable()
        {
            lifetimeEvent.Invoke();
        }
    }
}
