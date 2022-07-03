
namespace ScriptableObjectArchitecture
{
	public sealed class BoolVariableListener : AVariableListener<BoolVariable, bool, BoolUnityEvent>
	{
		//exists
		public BoolUnityEvent invertedResponse = default;

		protected override void OnDisable()
		{
			base.OnDisable();
			variable.RemoveInvertedListener(invertedResponse.Invoke);
		}

		protected override void OnEnable()
		{
			base.OnEnable();
			variable.AddInvertedListener(invertedResponse.Invoke);
		}
	}
}
