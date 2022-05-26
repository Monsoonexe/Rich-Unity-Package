using UnityEngine;
using UnityEngine.Events;
using DG.Tweening;
using NaughtyAttributes;

namespace RichPackage.Animation
{
    /// <summary>
    /// Class to facilitate animating UI elements.
    /// </summary>
    public class UIAnimation : RichMonoBehaviour
    {
        [Header("---Settings---")]
        [Min(0)]
        public float animInTime = 0.7f;
        [Min(0)]
        public float animOutTime = 0.7f;

        [Tooltip("[Optional] Uses attached if left null.")]
        public RectTransform targetTransform;

        [Foldout("---Events---")]
        [SerializeField]
        private UnityEvent onAnimInComplete = new UnityEvent();
        public UnityEvent OnAnimInComplete { get => onAnimInComplete; }//readonly

        [Foldout("---Events---")]
        [SerializeField]
        private UnityEvent onAnimOutComplete = new UnityEvent();
        public UnityEvent OnAnimOutComplete { get => onAnimOutComplete; }//readonly

        public bool IsAnimating { get => animTween != null; }

        private Tweener animTween;

        protected override void Reset()
        {
            base.Reset();
            targetTransform = GetComponent<RectTransform>();
            SetDevDescription("I help animate UI elements!");
        }

        [Button(null, EButtonEnableMode.Playmode)]
        public void ZoomIn()
        {
            //if (IsAnimating) return;//prevent spamming

            animTween = myTransform.Animate_ZoomIn(animInTime);
            animTween.onComplete += OnAnimationInComplete;
        }

        [Button(null, EButtonEnableMode.Playmode)]
        public void ZoomOut()
        {
            //if (IsAnimating) return;//prevent spamming

            animTween = myTransform.Animate_ZoomOut(animOutTime);
            animTween.onComplete += OnAnimationOutComplete;
        }

        [Button(null, EButtonEnableMode.Playmode)]
        public void ExpandHorizontal()
        {
            //if (IsAnimating) return;//prevent spamming

            animTween = myTransform.Animate_ExpandHorizontal(animInTime);
            animTween.onComplete += OnAnimationInComplete;
        }

        [Button(null, EButtonEnableMode.Playmode)]
        public void CollapseHorizontal()
        {
            //if (IsAnimating) return;//prevent spamming

            animTween = myTransform.Animate_CollapseHorizontal(animOutTime);
            animTween.onComplete += OnAnimationOutComplete;
        }

        [Button(null, EButtonEnableMode.Playmode)]
        public void ExpandVertical()
        {
            //if (IsAnimating) return;//prevent spamming

            animTween = myTransform.Animate_ExpandVertical(animInTime);
            animTween.onComplete += OnAnimationInComplete;
        }

        [Button(null, EButtonEnableMode.Playmode)]
        public void CollapseVertical()
        {
            //if (IsAnimating) return;//prevent spamming

            animTween = myTransform.Animate_CollapseVertical(animOutTime);
            animTween.onComplete += OnAnimationOutComplete;
        }

        private void OnAnimationInComplete()
        {
            animTween = null;
            onAnimInComplete.Invoke();
        }

        private void OnAnimationOutComplete()
        {
            animTween = null;
            onAnimOutComplete.Invoke();
        }
    }
}
