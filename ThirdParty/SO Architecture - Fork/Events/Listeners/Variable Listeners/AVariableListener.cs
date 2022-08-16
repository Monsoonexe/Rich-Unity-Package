using UnityEngine.Events;

namespace ScriptableObjectArchitecture
{
	public abstract class AVariableListener<TVariable, TUnderlying, TEvent> : SOArchitectureBaseMonobehaviour
		where TVariable : BaseVariable<TUnderlying>
		where TEvent : UnityEvent<TUnderlying>
	{
		public TVariable variable;

		public TEvent response = default;

		protected virtual void OnEnable()
		{
			variable.AddListener(response.Invoke);
			response.Invoke(variable); //give current state
		}

		protected virtual void OnDisable()
		{
			variable.RemoveListener(response.Invoke);
		}
	}
}
