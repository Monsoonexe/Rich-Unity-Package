using UnityEngine;

namespace RichPackage.DiceSystem
{
    /// <summary>
    /// Contains data to be querried upon a raycast hit.
    /// </summary>
    /// <seealso cref="DieBehaviour"/>
    [RequireComponent(typeof(Collider))]
    public class DieFaceData : RichMonoBehaviour
    {
        [SerializeField]
        [Tooltip("[EulerAngles] Rotation which when applied to parent Die " +
            "will show this face up.")]
        private Vector3 upRotation = new Vector3();

        public Vector3 UpRotation { get => upRotation; }

        public int faceValue;

        //member Components
        private Collider myCollider;
        public Collider Collider { get => myCollider; }

        protected override void Awake()
        {
            base.Awake();
            myCollider = GetComponent<Collider>();
        }

        public static implicit operator int (DieFaceData a)
            => a.faceValue;
    }
}
