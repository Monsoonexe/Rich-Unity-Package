using Sirenix.OdinInspector;
using UnityEngine;

namespace RichPackage.Animation
{
    public class Rotator : RichMonoBehaviour, IAnimate<Transform>, IAnimate
    {
        public Vector3 rotateVector;
        public Space space = Space.World;
        public Transform xform;

        [ShowInInspector, ReadOnly]
        public bool IsAnimating => enabled;

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
