using UnityEngine;

namespace RichPackage.Animation
{
    public class Rotator : RichMonoBehaviour, IAnimate<Transform>, IAnimate
    {
        public Vector3 rotateVector;
        public Space relativeSpace = Space.World;
        public TransformLine xform;

        public bool IsAnimating = enabled;

        protected override void Awake()
        {
            base.Awake();
            if (xform == null)
                xform = myTransform;
        }

        private void Update()
        {
            transform.Rotate(rotateVector * Time.deltaTime,
                relativeSpace);
        }

        public void Play()
            => enabled = true;

        public void Play(Transform xform)
        {
            this.xform = xform;
            enabled = true;
        }

        public void Stop()
            => enabled = false;
    }

}
