/*	TODO - only subscribe events if flag is set
 * 
 */

using System.Runtime.CompilerServices;
using Sirenix.OdinInspector;

namespace RichPackage.UnityMessages
{
	/// <summary>
	/// "Invokes a <see cref="UnityEngine.Events.UnityEvent"/> when 
	/// any of the given UnityMessages are called.
	/// </summary>
	/// <seealso cref="ManagedBehaviourEngine"/>
	public sealed class UnityMessageListener : AUnityLifetimeMessage
    {
		[Title(nameof(messages)), HideLabel,
			OnValueChanged(nameof(ClearBitsIfNoneIsSet))]
		[EnumToggleButtons, PropertyOrder(-2)]
        public EUnityMessage messages = EUnityMessage.Start;

		#region Unity Messages

		protected override void Reset()
		{
			base.Reset();
			SetDevDescription("Invokes a UnityEvent when any of the given" +
				" UnityMessages are called.");
		}

		protected override void Awake()
		{
			base.Awake();
			ClearBitsIfNoneIsSet();

			//awake
			InvokeIfFlagSet(EUnityMessage.Awake);
		}

        private void Start()
        {
            InvokeIfFlagSet(EUnityMessage.Start);
        }

        private void OnDestroy()
        {
            InvokeIfFlagSet(EUnityMessage.OnDestroy);
        }

        #endregion Unity Messages

        #region Managed Messages

        public void FixedUpdate()
		{
			InvokeIfFlagSet(EUnityMessage.FixedUpdate);
		}

		public void LateUpdate()
		{
			InvokeIfFlagSet(EUnityMessage.LateUpdate);
		}

		public void Update()
		{
			InvokeIfFlagSet(EUnityMessage.Update);
		}

		public void OnApplicationQuit()
		{
			InvokeIfFlagSet(EUnityMessage.OnApplicationQuit);
		}

		public void OnApplicationPause(bool pause)
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
		private void ClearBitsIfNoneIsSet()
		{
			if (messages.HasFlag(EUnityMessage.None))
				messages = 0x0; //clear all bits.
		}
	}
}
