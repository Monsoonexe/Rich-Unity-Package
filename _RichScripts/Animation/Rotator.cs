using Sirenix.OdinInspector;
using UnityEngine;

namespace RichPackage.Animation
{
    /// <summary>
    /// Rotates the target transform.
    /// </summary>
    public class Rotator : RichMonoBehaviour, IAnimate<Transform>, IAnimate
    {
        [Title("Options")]
        public Vector3 rotateVector;
        public Space space = Space.Self;

        [Title("References")]
        public Transform xform;

        [ShowInInspector, ReadOnly]
        public bool IsAnimating => enabled;

		private void Reset()
		{
            SetDevDescription("Rotates the target transform.");
            Awake();
        }

		protected override void Awake()
        {
            base.Awake();
            if (xform == null)
                xform = myTransform;
        }

        private void Update()
        {
            xform.Rotate(rotateVector * Time.deltaTime,
                space);
        }

        public void Play() => enabled = true;

        public void Play(Transform xform)
        {
            this.xform = xform;
            enabled = true;
        }

        public void Stop() => enabled = false;
    }
}
