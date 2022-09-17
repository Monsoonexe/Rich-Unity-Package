
namespace RichPackage.UnityMessages
{
    /// <summary>
    /// Raises UnityEvent on Start(). Rig it in Inspector.
    /// </summary>
    public sealed class StartUnityMessage : AUnityLifetimeMessage
    {
		protected override void Reset()
		{
			base.Reset();
            SetDevDescription($"Invokes {nameof(lifetimeEvent)} on {nameof(Start)}.");
		}

		private void Start()
        {
            lifetimeEvent.Invoke();

#if !UNITY_EDITOR
            Destroy(this);
#endif
        }
    }
}
