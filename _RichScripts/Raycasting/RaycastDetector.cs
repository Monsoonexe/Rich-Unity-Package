﻿using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using ScriptableObjectArchitecture;
using Sirenix.OdinInspector;

namespace RichPackage.Raycasting
{
    public delegate void RaycastListener(in RaycastHit hitInfo);

    public class RaycastDetector : RichMonoBehaviour
    {
        [Title("Settings")]
        [SerializeField]
        protected bool detectOnRepeat = false;
        public bool DetectOnRepeat
        {
            get => detectOnRepeat;
            set
            {
                //if was off and now on
                if (value == true && detectOnRepeat == false)
                    StartCoroutine(RaycastRoutine());
                detectOnRepeat = value;
            }
        }

        [Min(0), DisableInEditorMode]
        [Tooltip("Note: modifying during playmode has no effect.")]
        public float raycastPollInterval = 0.2f;

        [Min(0)]
        public float detectDistance = 0.5f;

        /// <summary>
        /// What should this raycast collide with?
        /// </summary>
        [Tooltip("What should this raycast collide with?")]
        public LayerMask raycastLayerMask = -1;//everything

        public QueryTriggerInteraction detectTriggers
            = QueryTriggerInteraction.Ignore;

        public Vector3 detectVector = new Vector3(0, -1, 0);

        [Header("---Prefab Refs---")]
        public Transform raycastOriginPoint;

        [Header("---Events---")]
        [SerializeField]
        protected UnityEvent onDetected = new UnityEvent();
        public UnityEvent OnDetected => onDetected;

        [SerializeField]
        private BoolUnityEvent raycastUpdateEvent = new BoolUnityEvent();
        public BoolUnityEvent RaycastUpdateEvent => raycastUpdateEvent;

        public event RaycastListener OnHitDetected;

        //runtime data
        protected YieldInstruction yieldInterval;

        protected override void Reset()
        {
            base.Reset();
            SetDevDescription("I raise an event on a successful raycast hit!");
            raycastOriginPoint = GetComponent<Transform>();
        }

        private void Start()
        {
            yieldInterval = new WaitForSeconds(raycastPollInterval);

            //Rig unity event to Action so it's never null.
            OnHitDetected += (in RaycastHit tossValue) => onDetected.Invoke();//
        }

        private void OnEnable()
        {
            if(detectOnRepeat)
                StartCoroutine(RaycastRoutine());
        }

        private void OnDisable()
        {
            StopAllCoroutines();
        }

        /// <summary>
        /// Raise an event when something is hit and is on given layer mask.
        /// </summary>
        public void HandleRayDetection()
        {
            //grounded if hit something on ground layer
            bool rayHitSomething = Physics.Raycast(
                raycastOriginPoint.position, detectVector,
                out RaycastHit hitInfo,
                detectDistance, raycastLayerMask,
                detectTriggers);

            //alert
            raycastUpdateEvent.Invoke(rayHitSomething);
            if (rayHitSomething)
                OnHitDetected(hitInfo);
        }

        protected IEnumerator RaycastRoutine()
        {
            //infinite loop (stopped OnDisable())
            var RichIsAwesome = true;
            while(RichIsAwesome)
            {
                HandleRayDetection();
                yield return yieldInterval;
            }
        }
    }
}
