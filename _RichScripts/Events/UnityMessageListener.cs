using System.Runtime.CompilerServices;
using RichPackage.Managed;
using Sirenix.OdinInspector;

namespace RichPackage.UnityMessages
{
	/// <summary>
	/// "Invokes a <see cref="UnityEngine.Events.UnityEvent"/> when 
	/// any of the given UnityMessages are called.
	/// </summary>
	public sealed class UnityMessageListener : AUnityLifetimeMessage,
        IManagedFixedUpdate, IManagedEarlyUpdate, IManagedLateUpdate,
		IManagedUpdate, IManagedOnApplicationPause, IManagedOnApplicationQuit
    {
		[Title(nameof(messages)), HideLabel,
			OnValueChanged(nameof(ClearBitsIfNoneSet))]
		[EnumToggleButtons, PropertyOrder(-2)]
        public EUnityMessage messages = EUnityMessage.Start;

		#region Unity Messages

		private void Reset()
		{
			SetDevDescription("Invokes a UnityEvent during any of the given" +
				" UnityMessages are called.");
		}

		protected override void Awake()
		{
			base.Awake();
			ClearBitsIfNoneSet();
			ManagedBehaviourEngine.RegisterManagedBehavior(this);
			InvokeIfFlagSet(EUnityMessage.Awake);
		}

		#endregion Unity Messages

		#region Managed Messages

		public void ManagedEarlyUpdate()
		{
			InvokeIfFlagSet(EUnityMessage.EarlyUpdate);
		}

		public void ManagedFixedUpdate()
		{
			InvokeIfFlagSet(EUnityMessage.FixedUpdate);
		}

		public void ManagedLateUpdate()
		{
			InvokeIfFlagSet(EUnityMessage.LateUpdate);
		}

		public void ManagedUpdate()
		{
			InvokeIfFlagSet(EUnityMessage.Update);
		}

		public void ManagedOnApplicationQuit()
		{
			InvokeIfFlagSet(EUnityMessage.OnApplicationQuit);
		}

		public void ManagedOnApplicationPause(bool pause)
		{
			InvokeIfFlagSet(EUnityMessage.OnApplicationPause);
		}

		#endregion Managed Messages

		[Button]
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void InvokeIfFlagSet(EUnityMessage mask)
		{
			//ClearBitsIfNoneSet(); //if this becomes a problem...
			if (messages.HasFlag(mask))
			{
				lifetimeEvent.Invoke();
			}
		}

		/// <summary>
		/// Clear all bits if <see cref="EUnityMessage.None"/> is set.
		/// </summary>
		[MethodImpl(MethodImplOptions.AggressiveInlining)]
		private void ClearBitsIfNoneSet()
		{
			if (messages.HasFlag(EUnityMessage.None))
				messages &= 0; //clear all bits.
		}
	}
}
