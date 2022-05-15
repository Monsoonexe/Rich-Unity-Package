using UnityEngine;
using ScriptableObjectArchitecture;
using Sirenix.OdinInspector;

namespace RichPackage
{
    /// <summary>
    /// Sets the timeScale.
    /// </summary>
    public class TimeScaleModifier : RichMonoBehaviour
    {
        [SerializeField]
        private FloatReference timeScale = new FloatReference(1);

        /// <summary>
        /// Setting this value updates Time.timeScale immediately.
        /// </summary>
        public float TimeScale
        {
            get => timeScale;
            set => timeScale.Value = value;
        }

        public void SetTimeScale(FloatVariable newValue)
        {
            TimeScale = newValue;
        }

        private void OnEnable()
        {
            UpdateTimeScale();
            //only works if using a FloatVariable, not Reference
            timeScale.AddListener(UpdateTimeScale);
        }

        private void OnDisable()
        {
            //only works if using a FloatVariable, not Reference
            timeScale.RemoveListener(UpdateTimeScale);
        }

        [Button, DisableInEditorMode]
        public void UpdateTimeScale()
        {
            Time.timeScale = timeScale;
        }
    }
}
