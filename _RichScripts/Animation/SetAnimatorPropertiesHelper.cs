using System;
using UnityEngine;
using NaughtyAttributes;

namespace RichPackage.Animation
{
    /// <summary>
    /// Can set an Animator's Parameter using a single argument (or none).
    /// Useful for rigging UnityEvents to Animators and delegates.
    /// </summary>
    public class SetAnimatorPropertiesHelper : RichMonoBehaviour
    {
        #region DataStructures

        [Serializable]
        private enum ParameterType
        {
            TRIGGER,
            BOOL,
            INT,
            FLOAT
        }

        [Serializable]
        private struct PropertyStruct
        {
            public string identifier;
            public ParameterType parameterType;

            [ShowIf("parameterType", ParameterType.BOOL)]
            [AllowNesting]
            public bool boolParameter;

            [ShowIf("parameterType", ParameterType.INT)]
            [AllowNesting]
            public int integerParameter;

            [ShowIf("parameterType", ParameterType.FLOAT)]
            [AllowNesting]
            public float floatParameter;
        }

        #endregion

        [Tooltip("[Auto-located if null.]")]
        public Animator animator;

        [SerializeField]
        private PropertyStruct[] properties = null;

        protected override void Reset()
        {
            base.Reset();
            SetDevDescription("I help rig an Animator to UnityEvents! Rig me to SetBool, or SetFloat.");
            animator = GetComponent<Animator>();
        }
        
        protected override void Awake()
        {
            base.Awake();

            if (animator == null)
                animator = GetComponent<Animator>();

            //validate
            Debug.Assert(animator != null,
                "[SetAnimatorPropertiesHelper] 'animator' is null.", this);
            Debug.Assert(properties != null,
                "[SetAnimationPropertiesHelper] 'properties' is null.", this);
        }

        private PropertyStruct GetProperty(string targetIdentifier)
        {
            var len = properties.Length;
            var foundProperty = new PropertyStruct();
            for (var i = len - 1; i >= 0; --i)
            {
                var property = properties[i];
                if (property.identifier == targetIdentifier)
                {
                    foundProperty = property;
                    break;
                }
            }
            return foundProperty;
        }

        private void SetAnimationParameter(ref PropertyStruct property)
        {
            //send corresponding animator message
            switch (property.parameterType)
            {   //all cases are present
                case ParameterType.TRIGGER:
                    animator.SetTrigger(property.identifier);
                    break;
                case ParameterType.BOOL:
                    animator.SetBool(property.identifier, property.boolParameter);
                    break;
                case ParameterType.INT:
                    animator.SetInteger(property.identifier, property.integerParameter);
                    break;
                case ParameterType.FLOAT:
                    animator.SetFloat(property.identifier, property.floatParameter);
                    break;
            }
        }

        /// <summary>
        /// Shortcut for SetAnimationParameter(0).
        /// </summary>
        public void SetAnimationParameter()
            => SetAnimationParameter(ref properties[0]);

        public void SetAnimationParameter(int i)
            => SetAnimationParameter(ref properties[i]);

        /// <summary>
        /// 
        /// </summary>
        /// <remarks>Prefer SetAnimatorParamter(int) due to string equality comparison.</remarks>
        /// <param name="targetIdentifier"></param>
        public void SetAnimationParameter(string targetIdentifier)
        {
            var p = GetProperty(targetIdentifier);
            SetAnimationParameter(ref p);
        }

        public void SetBool(bool value)
            => properties[0].boolParameter = value;

        public void SetBool(int index, bool value)
            => properties[index].boolParameter = value;

        public void SetInteger(int value)
            => properties[0].integerParameter = value;

        public void SetInteger(int index, int value)
            => properties[index].integerParameter = value;

        public void SetFloat(float value)
            => properties[0].floatParameter = value;

        public void SetFloat(int index, float value)
            => properties[index].floatParameter = value;
    }
}
