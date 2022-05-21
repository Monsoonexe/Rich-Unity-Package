
namespace RichPackage.UnityMessages
{
    /// <summary>
    /// Raises UnityEvent on Awake(). Rig it in Inspector.
    /// </summary>
    public sealed class AwakeUnityMessage : AUnityLifetimeMessage
    {
        protected override void Awake()
        {
            base.Awake();
            lifetimeEvent.Invoke();
        }
    }
}
