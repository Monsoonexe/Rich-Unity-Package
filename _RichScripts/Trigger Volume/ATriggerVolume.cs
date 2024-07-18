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

        protected const string EVENT_GROUP = "Events";

        #endregion Constants

        [NaughtyAttributes.Tag,
            Tooltip("Will filter collisions that don't have the given tag. Null or empty to filter nothing.")]
        public string reactToTag = GameObjectTags.Player;

        [FoldoutGroup(EVENT_GROUP)]
        [SerializeField]
        private UnityEvent onEnter;

        [FoldoutGroup(EVENT_GROUP)]
        [SerializeField]
        private UnityEvent onExit;

        public UnityEvent OnEnter { get => onEnter; }
        public UnityEvent OnExit { get => onExit; }

        [Button, DisableInEditorMode]
        public void ForceEnter() => onEnter.Invoke();

        [Button, DisableInEditorMode]
        public void ForceExit() => onExit.Invoke();

        protected bool ApplyCollisionFilter(GameObject other)
        {
            return enabled
                && (string.IsNullOrEmpty(reactToTag)
                    || other.CompareTag(reactToTag));
        }
    }
}
