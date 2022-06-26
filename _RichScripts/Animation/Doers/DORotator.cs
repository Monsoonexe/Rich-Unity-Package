using UnityEngine;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace RichPackage.Animation
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="DoMover"/>
    public class DORotator : ADoer
    {
        [Title("Animation Settings")]
        public Space space = Space.World;
        public Ease ease = Ease.OutQuart;
        public Vector3 destination = new Vector3 (0, 180, 0);
        public RotateMode mode = RotateMode.Fast;

		public override void Play()
		{
            if (space == Space.World)
                Tween = target.DORotate(destination, duration, mode);
            else
                Tween = target.DOLocalRotate(destination, duration, mode);

            //configure
            Tween.SetEase(ease);
            SubscribeTweenEvents(Tween);
        }

        /*note: Rotate90Right() and Left() work the best (do not suffer from gimbal lock).
        * if you need to rotate on the x-axis, give it an offset in the z-axis, and make it a 
        * child of an object with the opposite offset in the z-axis. Then use Rotate90Left() and Right()
        * as normal.  
        */

        /// <summary>
        /// Shortcut to Yaw right.
        /// </summary>
        [Button, DisableInEditorMode]
        public void Rotate90Right() => RotateLocalBy(new Vector3(0, 90, 0));

        /// <summary>
        /// Shortcut to Yaw left.
        /// </summary>
        [Button, DisableInEditorMode]
        public void Rotate90Left() => RotateLocalBy(new Vector3(0, -90, 0));

        [Button, DisableInEditorMode]
        public void Rotate90Forward() => RotateLocalBy(new Vector3(0, 0, 90));

        [Button, DisableInEditorMode]
        public void Rotate90Backward() => RotateLocalBy(new Vector3(0, 0, -90));

        [Button, DisableInEditorMode]
        public void Rotate90Clockwise() => RotateWorldBy(new Vector3(90, 0, 0));

        [Button, DisableInEditorMode]
        public void Rotate90CounterClockwise() => RotateWorldBy(new Vector3(-90, 0, 0));

        public void RotateLocalX(int x) => RotateLocalBy(new Vector3(x, 0, 0));
        public void RotateLocalY(int y) => RotateLocalBy(new Vector3(0, y, 0));
        public void RotateLocalZ(int z) => RotateLocalBy(new Vector3(0, 0, z));

        public void RotateLocalBy(Vector3 rotationVector)
        {
            if (IsAnimating) return; //prevent spamming

            Tween = target.DOLocalRotate(
                rotationVector, duration, RotateMode.LocalAxisAdd);
            Tween.SetEase(ease);
            SubscribeTweenEvents(Tween);
        }

        public void RotateWorldBy(Vector3 rotationVector)
        {
            if (IsAnimating) return; //prevent spamming

            Tween = target.DORotate(
                rotationVector, duration, RotateMode.WorldAxisAdd);
            Tween.SetEase(ease);
            SubscribeTweenEvents(Tween);
        }
    }
}
