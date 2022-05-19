
namespace RichPackage.UnityMessages
{
    /// <summary>
    /// Raises UnityEvent on OnEnable(). Rig it in Inspector.
    /// </summary>
    public sealed class OnEnableUnityMessage : AUnityLifetimeMessage
    {
        private void OnEnable()
        {
            lifetimeEvent.Invoke();
        }
    }
}
