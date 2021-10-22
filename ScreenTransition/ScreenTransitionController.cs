using System;
using UnityEngine;
using NaughtyAttributes;
using Sirenix.OdinInspector;

//clarifications
using Button = Sirenix.OdinInspector.ButtonAttribute;
using Required = Sirenix.OdinInspector.RequiredAttribute;

/* Callback system for OnTransitionEnd
 * 
 */ 

namespace RichPackage.UI
{
    /// <summary>
    /// Controls screen transitions and fades. Default animation duration is 1 second.
    /// </summary>
    [RequireComponent(typeof(Animator))]
    public class ScreenTransitionController : RichMonoBehaviour
    {
        private static ScreenTransitionController instance;
        public static ScreenTransitionController Instance { get => instance; }

        private const float DEFAULT_DURATION = 1.0f;

        [Header("---Prefab Refs---")]
        [SerializeField, Required]
        private Animator myAnimator;

        /// <summary>
        /// If you reeaaallly need it, here it is. But prefer to use API.
        /// </summary>
        public static Animator MyAnimator { get => instance.myAnimator; }

        /// <summary>
        /// Duration in seconds.
        /// </summary>
        public static float TransitionDuration
        {
            get => DEFAULT_DURATION / MyAnimator.speed; //speed is inverse of duration
            set => SetAnimationSpeed(value);
        }

        #region Animator Params

        //parameters
        [FoldoutGroup("---Parameters---")]
        [SerializeField, AnimatorParam("myAnimator")]
        private int FADE_IN;
        public int FadeInMessage { get => FADE_IN; }

        [FoldoutGroup("---Parameters---")]
        [SerializeField, AnimatorParam("myAnimator")]
        private int FADE_OUT;
        public int FadeOutMessage { get => FADE_OUT; }

        [FoldoutGroup("---Parameters---")]
        [SerializeField, AnimatorParam("myAnimator")]
        private int HIDE;
        public int HideMessage { get => HIDE; }

        [FoldoutGroup("---Parameters---")]
        [SerializeField, AnimatorParam("myAnimator")]
        private int HORIZONTAL_WIPE_IN;
        public int HorizontalWipeInMessage { get => HORIZONTAL_WIPE_IN; }

        [FoldoutGroup("---Parameters---")]
        [SerializeField, AnimatorParam("myAnimator")]
        private int HORIZONTAL_WIPE_OUT;
        public int HorizontalWipeOutMessage { get => HORIZONTAL_WIPE_OUT; }

        [FoldoutGroup("---Parameters---")]
        [SerializeField, AnimatorParam("myAnimator")]
        private int SHOW;
        public int ShowMessage { get => SHOW; }

        [FoldoutGroup("---Parameters---")]
        [SerializeField, AnimatorParam("myAnimator")]
        private int SPIN_LOCK_IN;
        public int SpinLockInMessage { get => SPIN_LOCK_IN; }

        [FoldoutGroup("---Parameters---")]
        [SerializeField, AnimatorParam("myAnimator")]
        private int SPIN_LOCK_OUT;
        public int SpinLockOutMessage { get => SPIN_LOCK_OUT; }

        [FoldoutGroup("---Parameters---")]
        [SerializeField, AnimatorParam("myAnimator")]
        private int VERTICAL_WIPE_IN;
        public int VerticalWipeInMessage { get => VERTICAL_WIPE_IN; }

        [FoldoutGroup("---Parameters---")]
        [SerializeField, AnimatorParam("myAnimator")]
        private int VERTICAL_WIPE_OUT;
        public int VerticalWipeOutMessage { get => VERTICAL_WIPE_OUT; }

		#endregion

		//public static event Action TransitionEndEvent;

        private void Reset()
        {
            SetDevDescription("Controls screen transitions and fades.");
            myAnimator = GetComponent<Animator>();
        }

        protected override void Awake()
        {
            base.Awake();

            //gather refs
            if(!myAnimator)
                myAnimator = GetComponent<Animator>();

            //singleton
            InitSingleton(this, ref instance, 
                dontDestroyOnLoad: false);
        }

        /// <summary>
        /// Safely set animation speed based on desired duration.
        /// </summary>
        /// <param name="duration"></param>
        public static void SetAnimationSpeed(float duration)
        {
            //guard against division by zero
            if (duration <= 0)
                MyAnimator.speed = 999999; // instant
            else
                MyAnimator.speed = DEFAULT_DURATION / duration;
        }

        public void ResetAnimationSpeed() => myAnimator.speed = DEFAULT_DURATION;

        #region Transition API

        /// <summary>
        /// Can call string directly if known. Otherwise, just use a function.
        /// </summary>
        [Button, DisableInEditorMode]
        public void TriggerTransition(string trigger)
            => myAnimator.SetTrigger(trigger);

        /// <summary>
        /// Can call string directly if known. Otherwise, just use a function.
        /// </summary>
        public void TriggerTransition(int trigger)
            => myAnimator.SetTrigger(trigger);

        /// <summary>
        /// Can call string directly if known. Otherwise, just use a function.
        /// </summary>
        /// <remarks>Easier to use this than add a shortcut for each new animation.</remarks>
        public void TriggerTransition(string trigger, float duration)
        {
            SetAnimationSpeed(duration);
            myAnimator.SetTrigger(trigger);
        }

        /// <summary>
        /// Can call string directly if known. Otherwise, just use a function.
        /// </summary>
        public void TriggerTransition(int trigger, float duration)
        {
            SetAnimationSpeed(duration);
            myAnimator.SetTrigger(trigger);
        }

        [Button, DisableInEditorMode, ButtonGroup("IN")]
        public void FadeIn()
            => myAnimator.SetTrigger(FADE_IN);

        [Button, DisableInEditorMode, ButtonGroup("OUT")]
        public void FadeOut()
            => myAnimator.SetTrigger(FADE_OUT);

        [Button("HorWipeIn"), DisableInEditorMode, ButtonGroup("IN")]
        public void HorizontalWipeIn()
            => myAnimator.SetTrigger(HORIZONTAL_WIPE_IN);

        [Button("HorWipeOut"), DisableInEditorMode, ButtonGroup("OUT")]
        public void HorizontalWipeOut()
            => myAnimator.SetTrigger(HORIZONTAL_WIPE_OUT);

        [Button, DisableInEditorMode, ButtonGroup("OUT")]
        public void HideScene()
            => myAnimator.SetTrigger(HIDE);

        [Button, DisableInEditorMode, ButtonGroup("IN")]
        public void ShowScene()
            => myAnimator.SetTrigger(SHOW);

        [Button, DisableInEditorMode, ButtonGroup("IN")]
        public void SpinLockIn()
            => myAnimator.SetTrigger(SPIN_LOCK_IN);

        [Button, DisableInEditorMode, ButtonGroup("OUT")]
        public void SpinLockOut()
            => myAnimator.SetTrigger(SPIN_LOCK_OUT);

        [Button("VertWipeIn"), DisableInEditorMode,
            ButtonGroup("IN")]
        public void VerticalWipeIn()
            => myAnimator.SetTrigger(VERTICAL_WIPE_IN);

        [Button("VertWipeOut"), DisableInEditorMode,
            ButtonGroup("OUT")]
        public void VerticalWipeOut()
            => myAnimator.SetTrigger(VERTICAL_WIPE_OUT);

        //public void RandomTransitionIn()
        //public void RandomTransitionOut()

        #endregion
    }
}
