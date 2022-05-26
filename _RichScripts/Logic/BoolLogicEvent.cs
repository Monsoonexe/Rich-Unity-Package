using UnityEngine.Events;
using Sirenix.OdinInspector;
using ScriptableObjectArchitecture;
using UnityEngine;

namespace RichPackage.Logic
{
    /// <summary>
    /// 
    /// </summary>
    public sealed class BoolLogicEvent : RichMonoBehaviour
    {
        private const string TAG = "Unity Events";
        private const string REF_TAG = "Refs";

        [Title("Operation"), HideLabel]
        [EnumToggleButtons]
        public EBoolComparisonLogic operation = EBoolComparisonLogic.Not;

        [Space(10)]
        [PropertyOrder(1), FoldoutGroup(TAG)]
        public UnityEvent trueUnityEvent;

        [PropertyOrder(1), FoldoutGroup(TAG)]
        public UnityEvent falseUnityEvent;

        [PropertyOrder(-1), ReadOnly, ShowInInspector]
        public bool Result { get; private set; }

        [BoxGroup(REF_TAG)]
        public BoolReference InputA;

        [BoxGroup(REF_TAG)]
        public BoolReference InputB;

		#region Unity Messages

		private void OnValidate()
		{
            Evaluate();
		}

        protected override void Reset()
        {
            base.Reset();
            SetDevDescription($"Compares {nameof(InputA)} to {nameof(InputB)} and raises events based on the result.");
        }

        private void OnEnable()
        {
            InputA.AddListener(EvaluateInternal);
            InputB.AddListener(EvaluateInternal);

            EvaluateInternal();
        }

        private void OnDisable()
        {
            InputA.RemoveListener(EvaluateInternal);
            InputB.RemoveListener(EvaluateInternal);
        }

		#endregion Unity Messages

		[Button(nameof(Evaluate))]
        private void EvaluateInternal()
        {
			switch (operation)
			{
				case EBoolComparisonLogic.None:
					break;
				case EBoolComparisonLogic.And:
					Result = InputA && InputB;
					break;
				case EBoolComparisonLogic.Nand:
					Result = InputA.Value.Nand(InputB);
					break;
                case EBoolComparisonLogic.Or:
                    Result = InputA || InputB;
                    break;
                case EBoolComparisonLogic.Nor:
                    Result = InputA.Value.Nor(InputB);
                    break;
				case EBoolComparisonLogic.Not:
                    Result = !InputA;
					break;
                case EBoolComparisonLogic.Xor:
                    Result = InputA.Value.Xor(InputB.Value);
                    break;
                case EBoolComparisonLogic.XNor:
                    Result = InputA.Value.Xnor(InputB.Value);
                    break;
				default:
					Debug.LogError($"{operation} not implemented!");
					break;
			}
		}

		public bool Evaluate()
		{
            EvaluateInternal();
            return Result;
		}
	}
}
