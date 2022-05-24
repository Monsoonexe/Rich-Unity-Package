using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace RichPackage.UnityMessages
{
	/// <summary>
	/// Invokes <see cref="IfEvent"/> if "UNITY_EDITOR" is a defined symbol,
	/// otherwise invokes <see cref="IfNotEvent"/>.
	/// </summary>
	[RequireComponent(typeof(UnityMessageListener))]
	public sealed class IfUnityEditorMessage : RichMonoBehaviour
	{
		[SerializeField, Required]
		private UnityMessageListener listener;

		[SerializeField, FoldoutGroup(nameof(IfEvent)), HideLabel]
		private UnityEvent ifEvent = new UnityEvent();
		public UnityEvent IfEvent { get => ifEvent; }

		[SerializeField, FoldoutGroup(nameof(IfNotEvent)), HideLabel]
		private UnityEvent ifNotEvent = new UnityEvent();
		public UnityEvent IfNotEvent { get => ifNotEvent; }

		#region Unity Messages

		private void Reset()
		{
			SetDevDescription("Invokes 'ifEvent' if 'UNITY_EDITOR' is defined," +
				" otherwise invokes 'ifNotEvent'.");
			Awake();
		}

		protected override void Awake()
		{
			base.Awake();
			if (listener == null)
				listener = gameObject.GetComponent<UnityMessageListener>();
		}

		private void OnEnable()
		{
			listener.LifetimeEvent.AddListener(InvokeEvent);
		}

		private void OnDisable()
		{
			listener.LifetimeEvent.RemoveListener(InvokeEvent);
		}

		#endregion Unity Messages

		public void InvokeEvent()
		{
#if UNITY_EDITOR
			ifEvent.Invoke();
#else	
			ifNotEvent.Invoke():
#endif
		}
	}

}
