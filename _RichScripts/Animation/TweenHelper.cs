using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using Sirenix.OdinInspector;

namespace RichPackage.Animation
{
    /// <summary>
    /// DOMove etc are extension methods. this helps rig UnityEvents to static methods.
    /// </summary>
    public class TweenHelper : RichMonoBehaviour
    {
        [Title("Settings")]
        [Min(0)]
        public float duration = 0.85f;

        public Ease ease = Ease.OutQuart;

        [FoldoutGroup("Events")]
        [SerializeField]
        protected UnityEvent onAnimationBegin = new UnityEvent();
        public UnityEvent OnAnimationBegin { get => onAnimationBegin; }//readonly

        [FoldoutGroup("Events")]
        [SerializeField]
        protected UnityEvent onAnimationComplete = new UnityEvent();
        public UnityEvent OnAnimationComplete { get => onAnimationComplete; }//readonly

        public bool IsAnimating { get => animTween != null; }

        protected Tweener animTween;

        public void DoMove(Transform place)
        {
            animTween = transform.DOMove(place.position, duration)
                .SetEase(ease)
                .OnComplete(OnAnimationCompleteHandler);
            onAnimationBegin.Invoke();
        }

        public void DoMoveFrom(Transform place)
        {
            animTween = transform.DOMove(place.position, duration)
                .From()
                .SetEase(ease)
                .OnComplete(OnAnimationCompleteHandler);
            onAnimationBegin.Invoke();
        }

        public void DoLocalMoveX(float x)
        {
            animTween = transform.DOLocalMoveX(x, duration)
                .SetEase(ease)
                .OnComplete(OnAnimationCompleteHandler);
            onAnimationBegin.Invoke();
        }

        public void DoLocalMoveY(float y)
        {
            animTween = transform.DOLocalMoveY(y, duration)
                .SetEase(ease)
                .OnComplete(OnAnimationCompleteHandler);
            onAnimationBegin.Invoke();
        }

        public void DoLocalMoveZ(float z)
        {
            animTween = transform.DOLocalMoveZ(z, duration)
                .SetEase(ease)
                .OnComplete(OnAnimationCompleteHandler);
            onAnimationBegin.Invoke();
        }

        public void DoRotate(Transform place)
        {
            animTween = transform.DORotate(place.eulerAngles, duration)
                .SetEase(ease)
                .OnComplete(OnAnimationCompleteHandler);
            onAnimationBegin.Invoke();
        }

        public void ResetRotation()
        {
            transform.localEulerAngles = Vector3.zero;
        }

        public void RotateLocalBy(Vector3 rotationVector)
        {
            if (IsAnimating) return; //prevent spamming

            animTween = transform.DOLocalRotate(
                rotationVector, duration, RotateMode.LocalAxisAdd);
            animTween.SetEase(ease)
                .OnComplete(OnAnimationCompleteHandler);
            onAnimationBegin.Invoke();
        }

        public void RotateWorldBy(Vector3 rotationVector)
        {
            if (IsAnimating) return; //prevent spamming

            animTween = transform.DORotate(
                rotationVector, duration, RotateMode.WorldAxisAdd);
            animTween.SetEase(ease)
                .OnComplete(OnAnimationCompleteHandler);
            onAnimationBegin.Invoke();
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

        protected void OnAnimationCompleteHandler()
        {
            animTween = null;
            onAnimationComplete.Invoke();
        }
    }
}
