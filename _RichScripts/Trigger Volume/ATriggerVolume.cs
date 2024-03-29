﻿using UnityEngine;
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

        private const string EVENT_GROUP = "Events";

        #endregion Constants

        [NaughtyAttributes.Tag,
            Tooltip("Will filter collisions that don't have the given tag. Null or empty to filter nothing.")]
        public string reactToTag = GameObjectTags.Player;

        /// <summary>
        /// Running total of times this has been triggered.
        /// </summary>
        [Tooltip("Running total of times this has been triggered.")]
        [ReadOnly]
        public int triggerCount = 0;

        [FoldoutGroup(EVENT_GROUP)]
        [SerializeField]
        protected UnityEvent enterEvent;
        public UnityEvent OnEnterEvent { get => enterEvent; }

        [FoldoutGroup(EVENT_GROUP)]
        [SerializeField]
        protected UnityEvent exitEvent;
        public UnityEvent OnExitEvent { get => exitEvent; }

        [Button, DisableInEditorMode]
        public void ForceEnter() => enterEvent.Invoke();

        [Button, DisableInEditorMode]
        public void ForceExit() => exitEvent.Invoke();

        protected void HandleEnterCollision(GameObject other)
        {
            if (!enabled)
                return;

            if (string.IsNullOrEmpty(reactToTag) || other.CompareTag(reactToTag))
            {
                ++triggerCount;
                enterEvent.Invoke();
            }
        }

        protected void HandleExitCollision(GameObject other)
        {
            if (!enabled)
                return;

            if (string.IsNullOrEmpty(reactToTag) || other.CompareTag(reactToTag))
            {
                exitEvent.Invoke();
            }
        }
    }
}
