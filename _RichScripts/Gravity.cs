using UnityEngine;
using Sirenix.OdinInspector;

namespace RichPackage
{
    [RequireComponent(typeof(Rigidbody))]
    public class Gravity : RichMonoBehaviour
    {
        public static readonly Vector3 STANDARD_GRAVITY 
            = new Vector3(0, -9.81f, 0);

        [Title("Settings")]
        public Vector3 gravityVector = STANDARD_GRAVITY;

        [Min(0.1f)]
        public float groundDetectDistance = 0.5f;

        public QueryTriggerInteraction detectTriggersAsGround 
            = QueryTriggerInteraction.Ignore;

        public LayerMask raycastLayerMask = 512;//9 = ground

        public bool IsGrounded { get; private set; }

        [Title("Prefab Refs")]
        [SerializeField, Required]
        private Transform raycastOriginPoint;

        [SerializeField, Required]
        private Rigidbody myRigidbody;

        protected override void Reset()
        {
            base.Reset();
            SetDevDescription("I act as gravity for objects with no non-kinematic rigidbody.");
            raycastOriginPoint = GetComponent<Transform>();
            myRigidbody = GetComponent<Rigidbody>();
        }

        // Update is called once per frame
        private void FixedUpdate()
        {
            HandleGroundDetection();
            if(!IsGrounded)
            {
                var step = gravityVector * App.FixedDeltaTime;
                myRigidbody.MovePosition(gravityVector);
            }
        }

        private void HandleGroundDetection()
        {
            //grounded if hit something on ground layer
            IsGrounded = Physics.Raycast(
                raycastOriginPoint.position, gravityVector,
                out RaycastHit hitInfo,
                groundDetectDistance, raycastLayerMask,
                detectTriggersAsGround);
        }
    }
}
