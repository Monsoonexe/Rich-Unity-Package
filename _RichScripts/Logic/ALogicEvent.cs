using UnityEngine.Events;
using ScriptableObjectArchitecture;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace RichPackage.Logic
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ALogicEvent<TReference, TVariable, TComparable> : RichMonoBehaviour
        where TReference : BaseReference<TComparable, TVariable>
        where TVariable : BaseVariable<TComparable>
        where TComparable : IComparable<TComparable>
    {
        private const string TAG = "Unity Events";
        private const string REF_TAG = "Refs";

        [Title("Operation"), HideLabel]
        [EnumToggleButtons]
        public ENumericComparisonLogic operation = ENumericComparisonLogic.EqualTo;

        [Space(10)]
        [PropertyOrder(1), FoldoutGroup(TAG)]
        public UnityEvent trueUnityEvent;

        [PropertyOrder(1), FoldoutGroup(TAG)]
        public UnityEvent falseUnityEvent;

        [PropertyOrder(-1), ReadOnly, ShowInInspector]
        public bool Result { get; protected set; }

        [BoxGroup(REF_TAG)]
        public TReference InputA;

        [BoxGroup(REF_TAG)]
        public TReference InputB;

		#region Unity Messages

		private void OnValidate()
        {
            Evaluate();
        }

        protected virtual void Reset()
		{
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
        protected void EvaluateInternal()
		{
            int compareValue = InputA.Value.CompareTo(InputB.Value);
            switch (operation)
            {
                //case EComparisonLogic.None:
                //                Result = null;
                //                return; //do not continue
                case ENumericComparisonLogic.GreaterThan:
                    Result = (compareValue > 0);
                    break;
                case ENumericComparisonLogic.LesserThan:
                    Result = (compareValue < 0);
                    break;
                case ENumericComparisonLogic.EqualTo:
                    Result = (compareValue == 0);
                    break;
                case ENumericComparisonLogic.NotEqualTo:
                    Result = (compareValue != 0);
                    break;
                case ENumericComparisonLogic.GreaterThanOrEqual:
                    Result = (compareValue >= 0);
                    break;
                case ENumericComparisonLogic.LesserThanOrEqual:
                    Result = (compareValue <= 0);
                    break;
                default:
                    Debug.LogError($"{operation} not implemented!");
                    break;
            }

            if (Result)
                trueUnityEvent.Invoke();
            else
                falseUnityEvent.Invoke();
        }

        public bool Evaluate()
        {
            EvaluateInternal();
            return Result;
        }
    }
}
