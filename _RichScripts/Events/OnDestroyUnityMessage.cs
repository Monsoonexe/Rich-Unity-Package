
namespace RichPackage.UnityMessages
{
    /// <summary>
    /// Raises UnityEvent on OnDestroy(). Rig it in Inspector.
    /// </summary>
    public sealed class OnDestroyUnityMessage : AUnityLifetimeMessage
    {
        private void OnDestroy()
        {
            lifetimeEvent.Invoke();
        }
    }
}
