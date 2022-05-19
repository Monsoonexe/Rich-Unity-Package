
namespace RichPackage.UnityMessages
{
    /// <summary>
    /// Raises UnityEvent on LateUpdate(). Rig it in Inspector.
    /// </summary>
    public sealed class LateUpdateUnityMessage : AUnityLifetimeMessage
    {
        private void LateUpdate()
        {
            lifetimeEvent.Invoke();
        }
    }
}
