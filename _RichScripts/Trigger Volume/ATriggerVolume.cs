using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace RichPackage.TriggerVolumes
{
    /// <summary>
    /// Base class for Trigger Volume behaviour. 
    /// Fires an event on Enter/Exit when tag matches
    /// </summary>
    public abstract class ATriggerVolume : RichMonoBehaviour
    {
        #region Constants

        private const string EVENT_GROUP = "EG";

        #endregion Constants

        [SerializeField, NaughtyAttributes.Tag]
        protected string reactToTag = ConstStrings.TAG_PLAYER;

        /// <summary>
        /// Running total of times this has been triggered.
        /// </summary>
        [Tooltip("Running total of times this has been triggered.")]
        [ReadOnly]
        public int triggerCount = 0;

        [FoldoutGroup(EVENT_GROUP)]
        [SerializeField]
        protected UnityEvent enterEvent = new UnityEvent();

        [FoldoutGroup(EVENT_GROUP)]
        [SerializeField]
        protected UnityEvent exitEvent = new UnityEvent();

        [Button, DisableInEditorMode]
        public void ForceEnter() => enterEvent.Invoke();

        [Button, DisableInEditorMode]
        public void ForceExit() => exitEvent.Invoke();

        protected void HandleEnterCollision(GameObject other)
        {
            if (string.IsNullOrEmpty(reactToTag) || other.CompareTag(reactToTag))
            {
                ++triggerCount;
                enterEvent.Invoke();
            }
        }

        protected void HandleExitCollision(GameObject other)
        {
            if (string.IsNullOrEmpty(reactToTag) || other.CompareTag(reactToTag))
            {
                exitEvent.Invoke();
            }
        }
    }
}
