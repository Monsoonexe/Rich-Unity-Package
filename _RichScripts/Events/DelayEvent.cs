using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;
//using RichPackage.GuardClauses;

namespace RichPackage.Events
{
    public sealed class DelayEvent : RichMonoBehaviour
    {
        private enum MessageTrigger
        {
            None = 0,
            Awake = 1,
            Start = 2,
        }

        [Title("Options"), SerializeField, EnumToggleButtons]
        private MessageTrigger messageTrigger = MessageTrigger.None;

        [SerializeField, Min(float.Epsilon)]
        private float delaySeconds = 1f;

        [SerializeField, Title("Unity Events"),
            FoldoutGroup("Event")]
        private UnityEvent uEvent = new UnityEvent();

        //runtime data
        [ShowInInspector, ReadOnly, HideInEditorMode, PropertyTooltip("Is the event pending.")]
        public bool IsPending => delayRoutine != null;

        private Coroutine delayRoutine;

        #region Unity Messages

        private void Reset()
        {
            SetDevDescription("I invoke a UnityEvent after a delay.");
        }

        protected override void Awake()
        {
            if (messageTrigger == MessageTrigger.Awake)
                CallEvent();
        }

        private void Start()
        {
            if (messageTrigger == MessageTrigger.Start)
                CallEvent();
        }

        #endregion Unity Messages

        [Button, DisableInEditorMode]
        public void CallEvent() => StartCoroutine(Delay());

        [Button, DisableInEditorMode]
        public void CancelEvent() => StopAllCoroutines();

        private IEnumerator Delay()
        {
            //validate
            //GuardAgainst.IsZeroOrNegative(delaySeconds);

            //prevent duplicates
            if (delayRoutine != null)
                StopCoroutine(delayRoutine);

            //do work
            yield return new WaitForSeconds(delaySeconds);
            uEvent.Invoke();
            delayRoutine = null;
        }
    }
}
