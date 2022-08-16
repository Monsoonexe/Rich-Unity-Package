using ScriptableObjectArchitecture;
using Sirenix.OdinInspector;

namespace RichPackage.Logic
{
	/// <summary>
	/// Say something!
	/// </summary>
	/// <seealso cref="FloatLogicEvent"/>
	/// <seealso cref="IntLogicEvent"/>
	public class ArithmeticEvent : RichMonoBehaviour
	{
		[Title(nameof(Operation)), HideLabel]
		public EArithmeticOperation Operation = EArithmeticOperation.Add;

		[Title("Operands")]
		public FloatReference InputA = new FloatReference(0);
		public FloatReference InputB = new FloatReference(0);

		[ShowInInspector, ReadOnly]
		public float Result { get; private set; }

		[FoldoutGroup(nameof(ResultEvent)), HideLabel]
		public FloatUnityEvent ResultEvent = new FloatUnityEvent();

		#region Unity Messages

		// when A or B changes, update Q
		protected void OnEnable()
		{
			// sub
			InputA.AddListener(DoEvaluation);
			InputB.AddListener(DoEvaluation);
		}

		private void OnDisable()
		{
			// unsub
			InputA.RemoveListener(DoEvaluation);
			InputB.RemoveListener(DoEvaluation);
		}

		#endregion Unity Messages

		public void DoEvaluation()
		{
			switch (Operation)
			{
				case EArithmeticOperation.None:
					return;
				case EArithmeticOperation.Add:
					Result = InputA.Value + InputB.Value;
					break;
				case EArithmeticOperation.Sub:
					Result = InputA.Value - InputB.Value;
					break;
				case EArithmeticOperation.Mul:
					Result = InputA.Value * InputB.Value;
					break;
				case EArithmeticOperation.Div:
					Result = InputA.Value / InputB.Value;
					break;
				default:
					throw ExceptionUtilities.GetEnumNotAccountedException(Operation);
			}
			ResultEvent.Invoke(Result);
		}

		public float Evaluate()
		{
			DoEvaluation();
			return Result;
		}
	}

}
