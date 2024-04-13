using RichPackage.RandomExtensions;
using Sirenix.OdinInspector;
using System;
using System.Collections;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RichPackage.DiceSystem
{
    /// <summary>
    /// Representation / model of an n-sided die.
    /// Can subscribe to OnResultReadyEvent{int} to get Result.
    /// Or an manually poll Result to not be -1. Or call GetResult after some time.
    /// </summary>
    public class DieBehaviour : RichMonoBehaviour
    {
        /* This should be hard-wired to an n-sided die. 
        * Should dynamically load a new die with this behaviour, rather than 
        * 'hot swap' dice between Behaviours.
        */

        [Header("---Settings---")]
        [ReadOnly]
        [SerializeField]
        private Die myDie;

        /// <summary>
        /// Die data: sides, face-up value.
        /// </summary>
        public Die Die { get => myDie; }

        [ReadOnly, ShowInInspector]
        public int Result
        {
            get => myDie.faceUpValue;
            private set => myDie.faceUpValue = value;
        }

        /// <summary>
        /// Should this die reset to 0,0,0 local when rolled?
        /// </summary>
        [Tooltip("Should this die reset to 0,0,0 local when rolled?")]
        public bool rollFromOrigin = true;

        [Header("---Direction Settings---")]
        public Vector3 defaultRollDirection = new Vector3(0, 1, 0);

        public Vector3 defaultTorqueDirection = new Vector3(0.85f, 0.75f, 0.85f);

        [MinMaxSlider(0, 30)]
        public Vector2 randomForceBounds = new Vector2(8, 12);

        [MinMaxSlider(0, 30)]
        public Vector2 randomTorqueBounds = new Vector2(8, 12);

        [Header("---Force Settings---")]
        public ForceMode forceMode = ForceMode.VelocityChange;

        [Tooltip("Only used for RollDieSimple(). How high should the drop be?")]
        [Min(0)]
        public float simpleRollHeight = 4.55f;

        [Header("---Raycast Settings---")]
        [Min(1)]
        public float raycastHeight = 4.55f;

        [Tooltip("RaycastLayer")]
        public LayerMask dieCheckLayerMask = -1;

        [Header("---Prefab Refs---")]
        [SerializeField]
        private DieFaceData[] faces = new DieFaceData[0];

        [Tooltip("[Self if null] Physics object of die.")]
        [SerializeField]
        private Rigidbody dieRigidbody = null;
        private Transform dieTransform = null;

        public bool IsRolling { get => rollReadyCheckRoutine != null; }

        // events
        public event Action<int> OnResultReadyEvent;

        private Coroutine rollReadyCheckRoutine;
        private readonly static YieldInstruction resultCheckPollInterval =
            new WaitForSeconds(0.3f);

        private void OnValidate()
        {
            UpdateDieFaces();
        }

        protected override void Awake()
        {
            base.Awake();

            if (dieRigidbody == null)
                dieRigidbody = GetComponent<Rigidbody>();

            if (dieRigidbody == null)
                dieRigidbody = GetComponentInChildren<Rigidbody>();

            Debug.Assert(dieRigidbody != null,
                "[DieBehaviour] Rigidbody cannot be found!",
                this);

            dieTransform = dieRigidbody.GetComponent<Transform>();
        }

        private void OnDisable()
        {
            dieRigidbody.isKinematic = true;//clear lingering physics forces
        }

        private IEnumerator CheckDieStable()
        {
            // wait until stopped moving
            do
            {
                yield return resultCheckPollInterval; //wait...
            } while (dieRigidbody.velocity != Vector3.zero);

            int result = GetResult(); //cehck which side is up
            OnResultReadyEvent?.Invoke(result);
            rollReadyCheckRoutine = null;
        }

        /// <summary>
        /// Updates Die data from children Face GameObjects.
        /// </summary>
        private void UpdateDieFaces()
        {
            int len = faces.Length;

            // create a new array only if null or wrong size.
            if (myDie.faceValues == null
                || myDie.faceValues.Length != len)
            {
                myDie.faceValues = new int[len];
            }

            //add die values from face values
            for (int i = 0; i < len; ++i)
            {
                myDie.faceValues[i] = faces[i].faceValue;
            }
            Result = -1;//flag invalid.
        }

        /// <summary>
        /// Apply Physics forces onto the die to cast it and get a result.
        /// Uses default throw/torque directions.
        /// </summary>
        [Button, DisableInEditorMode]
        public void RollDie()
            => RollDie(defaultRollDirection, defaultTorqueDirection);

        public void RollDie(Vector3 throwDirection)
            => RollDie(throwDirection, defaultTorqueDirection);

        /// <summary>
        /// For a permament set, set rollDirection directly.
        /// </summary>
        /// <param name="directionOverride">Temporary direction to use.</param>
        /// <remarks>For a permament set, set rollDirection directly.</remarks>
        public void RollDie(Vector3 throwDirection,
            Vector3 spinDirection)
        {
            if (rollFromOrigin)
                MoveDieLocal(Vector3.zero);

            Result = -1; //flag result not valid.

            RandomizeRotation();
            rollReadyCheckRoutine = StartCoroutine(CheckDieStable());

            // enable physics
            dieRigidbody.useGravity = true;
            dieRigidbody.isKinematic = false;

            // calculate throw force
            float rollForceScaler = randomForceBounds.RandomRange();
            Vector3 rollForce = throwDirection * rollForceScaler;
            dieRigidbody.AddForce(rollForce, forceMode);

            // handle random rotation
            float torqueForceScaler = randomTorqueBounds.RandomRange();
            Vector3 torqueForce = spinDirection * torqueForceScaler;
            dieRigidbody.AddTorque(torqueForce, forceMode);
        }

        [Button, DisableInPlayMode]
        private void GatherChildrenFaceReferences()
        {
            faces = dieRigidbody.transform.GetComponentsInChildren<DieFaceData>();

            Debug.Assert(faces.Length > 0,
                "[DieBehaviour] No faces found on children.", this);
            UpdateDieFaces();
        }

        /// <summary>
        /// Querries the result (which side is face-up) of a dice roll after being rolled.
        /// </summary>
        /// <returns></returns>
        public int GetResult()
        {
            // determine distance of raycast
            Vector3 endPoint = dieRigidbody.position;
            Vector3 originPoint = endPoint.WithY(raycastHeight);
            Vector3 direction = endPoint - originPoint;
            RaycastHit hitInfo;

            bool rayHitSomething = Physics.Raycast
                (
                    originPoint, direction,
                    out hitInfo, raycastHeight,
                    dieCheckLayerMask,
                    QueryTriggerInteraction.Collide
                );

            Debug.Assert(rayHitSomething,
                "[DieBehaviour] Could not hit die from raycast! " +
                "Check layer and collider settings and no obstructions.",
                this);

            DieFaceData diceFace = hitInfo.collider.GetComponent<DieFaceData>();

            Debug.AssertFormat(diceFace != null,
                "[{0}] Raycast hit a collider {1}," +
                "but no DieFaceData mono found. " +
                "Check layer and dieCheckLayerMask.",
            name, hitInfo.collider.name);

            return Result = diceFace.faceValue; //store for later querry.faceUpValue;
        }

        /// <summary>
        /// Get a die result using odds instead of physics.
        /// </summary>
        /// <returns></returns>
        public int GetResultRandom()
            => Result = faces.GetRandomElement();

        public void MoveDie(Transform here)
            => MoveDie(here.position);

        public void MoveDie(Vector3 destination)
        {
            // effectively disable physics to allow teleport
            dieRigidbody.useGravity = false;
            dieRigidbody.isKinematic = true;
            dieTransform.position = destination;
            //dieRigidbody.MovePosition(destination);
        }

        public void MoveDieLocal(Vector3 destination)
        {
            // effectively disable physics to allow teleport
            dieRigidbody.useGravity = false;
            dieRigidbody.isKinematic = true;
            dieTransform.localPosition = destination;
            //dieRigidbody.MovePosition(destination);
        }

        /// <summary>
        /// Set die rotation to completely random value.
        /// </summary>
        [Button]
        public void RandomizeRotation()
        {
            // disable physics
            dieRigidbody.useGravity = false;
            dieRigidbody.isKinematic = true;
            dieTransform.rotation = Random.rotation;
            //dieRigidbody.MoveRotation(Random.rotationUniform); //better at 40% cost.
        }

        /// <summary>
        /// Randomly select a side and face it up. 
        /// Like a random roll without the rolling.
        /// </summary>
        [Button]
        public void RandomizeDieSideUp()
        {
            // disable physics
            dieRigidbody.useGravity = false;
            dieRigidbody.isKinematic = true;

            // pick a random side
            DieFaceData randomSide = faces.GetRandomElement();

            // orient that side up
            dieTransform.eulerAngles = randomSide.UpRotation;

            // update face up value
            Result = randomSide.faceValue;
        }

        //[Button]
        //private void TestRotateTo(int x = 3)
        //{
        //    RotateTo(x);
        //    UpdateResult();
        //    Debug.Assert(Die.faceUpValue == x);
        //}

        /// <summary>
        /// Rotate DieTransform so the targetFaceValue side is facing up.
        /// </summary>
        /// <param name="targetFaceValue"></param>
        public void RotateTo(int targetFaceValue)
        {
            // disable physics
            dieRigidbody.useGravity = false;
            dieRigidbody.isKinematic = true;

            // rotate
            DieFaceData face = null;

            // find target
            for (int i = 0; i < faces.Length; ++i)
            {
                if (faces[i].faceValue == targetFaceValue)
                {
                    face = faces[i];
                    break;
                }
            }

            // validate
            Debug.Assert(face != null,
                "[DieBehaviour] Die could not find face value of: "
                + targetFaceValue,
                this);

            //dieTransform.rotation = targetRotation;
            dieTransform.eulerAngles = face.UpRotation; //rotate
            Result = face.faceValue; //match result
        }

        public static implicit operator Die(DieBehaviour a) => a.myDie;
        
        public static implicit operator int(DieBehaviour a) => a.Result;
    }
}
