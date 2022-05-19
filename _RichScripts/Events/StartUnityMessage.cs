
namespace RichPackage.UnityMessages
{
    /// <summary>
    /// Raises UnityEvent on Start(). Rig it in Inspector.
    /// </summary>
    public sealed class StartUnityMessage : AUnityLifetimeMessage
    {
        private void Start()
        {
            lifetimeEvent.Invoke();
        }
    }
}
