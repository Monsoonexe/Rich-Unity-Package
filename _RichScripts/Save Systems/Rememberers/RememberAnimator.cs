/*
 *  Inspired by: Adventure Creator
 */

using RichPackage;
using RichPackage.SaveSystem;
using Sirenix.OdinInspector;
using System.Text;
using UnityEngine;

namespace RichPackage.SaveSystem
{
    /// <summary>
    /// Remembers the parameters, layers, and state transitions of an animator.
    /// </summary>
    public class RememberAnimator : ASaveableMonoBehaviour
    {
        private const string Pipe = "|";

        [SerializeField]
        private UniqueID saveId;

        [Required]
        public Animator animator;

        public override UniqueID SaveID
        {
            get => saveId;
            protected set => saveId = value;
        }

        [SerializeField]
        private bool setDefaultParameterValues = false;

        [SerializeField]
        private DefaultAnimParameter[] defaultAnimParameters = System.Array.Empty<DefaultAnimParameter>();

        protected override void Reset()
        {
            SetDevDescription("Remembers the parameters, layers, and state transitions of an animator.");
            SaveID = SaveID = UniqueIdUtilities.CreateIdFrom(this, includeScene: true, includeName: true, includeType: true);
            animator = GetComponentInChildren<Animator>();
        }

        protected override void Awake()
        {
            base.Awake();
            if (setDefaultParameterValues && isActiveAndEnabled)
            {
                for (int i = 0; i < animator.parameters.Length; i++)
                {
                    if (i < defaultAnimParameters.Length)
                    {
                        string parameterName = animator.parameters[i].name;

                        switch (animator.parameters[i].type)
                        {
                            case AnimatorControllerParameterType.Bool:
                                animator.SetBool(parameterName, defaultAnimParameters[i].intValue != 0);
                                break;

                            case AnimatorControllerParameterType.Float:
                                animator.SetFloat(parameterName, defaultAnimParameters[i].floatValue);
                                break;

                            case AnimatorControllerParameterType.Int:
                                animator.SetInteger(parameterName, defaultAnimParameters[i].intValue);
                                break;
                        }
                    }
                }
            }
        }

        public override void SaveState(ISaveStore saveFile)
        {
            var memento = new Memento();

            // TODO optimize
            memento.parameterData = ParameterValuesToString(animator.parameters).ToString();
            memento.layerWeightData = LayerWeightsToString().ToString();
            memento.stateData = StatesToString().ToString();

            saveFile.Save(saveId, memento);
        }

        public override void LoadState(ISaveStore saveFile)
        {
            Memento memento = saveFile.Load<Memento>(SaveID);

            StringToParameterValues(animator.parameters, memento.parameterData);
            StringToLayerWeights(memento.layerWeightData);
            StringToStates(memento.stateData);
        }

        // TODO - optimize
        private StringBuilder ParameterValuesToString(AnimatorControllerParameter[] parameters)
        {
            var stateString = new StringBuilder();

            foreach (AnimatorControllerParameter parameter in parameters)
            {
                switch (parameter.type)
                {
                    case AnimatorControllerParameterType.Bool:
                        stateString.Append(animator.GetBool(parameter.name) ? "1" : "0");
                        break;

                    case AnimatorControllerParameterType.Float:
                        stateString.Append(animator.GetFloat(parameter.name));
                        break;

                    case AnimatorControllerParameterType.Int:
                        stateString.Append(animator.GetInteger(parameter.name));
                        break;

                    default:
                        stateString.Append("0");
                        break;
                }

                stateString.Append(Pipe);
            }

            return stateString;
        }

        // TODO - optimize
        private StringBuilder LayerWeightsToString()
        {
            var stateString = new StringBuilder();

            if (animator.layerCount > 1)
            {
                for (int i = 1; i < animator.layerCount; i++)
                {
                    float weight = animator.GetLayerWeight(i);
                    stateString.Append(weight).Append(Pipe);
                }
            }

            return stateString;
        }

        // TODO - optimize
        private StringBuilder StatesToString()
        {
            var stateString = new StringBuilder();

            for (int i = 0; i < animator.layerCount; i++)
            {
                if (animator.IsInTransition(i))
                {
                    ProcessState(stateString, animator.GetNextAnimatorStateInfo(i));
                }
                else
                {
                    ProcessState(stateString, animator.GetCurrentAnimatorStateInfo(i));
                }

                stateString.Append(Pipe);
            }

            return stateString;
        }

        private void ProcessState(StringBuilder stateString, AnimatorStateInfo stateInfo)
        {
            int nameHash = stateInfo.shortNameHash;
            float timeAlong = stateInfo.normalizedTime;

            if (timeAlong > 1f)
            {
                if (stateInfo.loop)
                {
                    timeAlong = timeAlong % 1;
                }
                else
                {
                    timeAlong = 1f;
                }
            }

            stateString.Append(nameHash)
                .Append(",")
                .Append(timeAlong);
        }

        private void StringToParameterValues(AnimatorControllerParameter[] parameters, string valuesString)
        {
            if (string.IsNullOrEmpty(valuesString))
            {
                return;
            }

            string[] valuesArray = valuesString.Split(Pipe[0]);

            for (int i = 0; i < parameters.Length; i++)
            {
                if (i < valuesArray.Length && valuesArray[i].Length > 0)
                {
                    string parameterName = parameters[i].name;

                    switch (parameters[i].type)
                    {
                        case AnimatorControllerParameterType.Bool:
                            animator.SetBool(parameterName, valuesArray[i] == "1");
                            break;

                        case AnimatorControllerParameterType.Float:
                            float floatValue = 0f;
                            if (float.TryParse(valuesArray[i], out floatValue))
                            {
                                animator.SetFloat(parameterName, floatValue);
                            }

                            break;

                        case AnimatorControllerParameterType.Int:
                            int intValue = 0;
                            if (int.TryParse(valuesArray[i], out intValue))
                            {
                                animator.SetInteger(parameterName, intValue);
                            }

                            break;
                    }
                }
            }
        }

        private void StringToLayerWeights(string valuesString)
        {
            if (string.IsNullOrEmpty(valuesString) || animator.layerCount <= 1)
            {
                return;
            }

            string[] valuesArray = valuesString.Split(Pipe[0]);

            for (int i = 1; i < animator.layerCount; i++)
            {
                if (i < (valuesArray.Length + 1) && valuesArray[i - 1].Length > 0)
                {
                    float weight = 1f;
                    if (float.TryParse(valuesArray[i - 1], out weight))
                    {
                        animator.SetLayerWeight(i, weight);
                    }
                }
            }
        }

        private void StringToStates(string valuesString)
        {
            if (string.IsNullOrEmpty(valuesString))
            {
                return;
            }

            string[] valuesArray = valuesString.Split(Pipe[0]);

            for (int i = 0; i < animator.layerCount; i++)
            {
                if (i < valuesArray.Length && valuesArray[i].Length > 0)
                {
                    string[] stateInfoArray = valuesArray[i].Split(","[0]);

                    if (stateInfoArray.Length >= 2)
                    {
                        float timeAlong = 0f;

                        if (int.TryParse(stateInfoArray[0], out int nameHash))
                        {
                            if (float.TryParse(stateInfoArray[1], out timeAlong))
                            {
                                animator.Play(nameHash, i, timeAlong);
                            }
                        }
                    }
                }
            }
        }

        [System.Serializable]
        private struct DefaultAnimParameter
        {
            public int intValue;
            public float floatValue;

            public DefaultAnimParameter(int _intValue)
            {
                intValue = _intValue;
                floatValue = 0f;
            }

            public DefaultAnimParameter(float _floatValue)
            {
                intValue = 0;
                floatValue = _floatValue;
            }
        }

        private class Memento
        {
            /** The unique identified of the Animator Controller */
            public string controllerID;
            /** The values of the parameters, separated by a pipe (|) character. */
            public string parameterData;
            /** The weights of each layer, separated by a pipe (|) character. */
            public string layerWeightData;
            /** Data for each layer's animation state. */
            public string stateData;

            /** The default Constructor. */
            public Memento() { }
        }
    }
}
