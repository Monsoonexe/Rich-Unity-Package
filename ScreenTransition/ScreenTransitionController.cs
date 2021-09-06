using UnityEngine;
using NaughtyAttributes;

/* TODO - Spawn from Resources
 * Fix [Button] attributes not working
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
        [SerializeField]
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
        [Header("---Parameters---")]
        [SerializeField, AnimatorParam("myAnimator")]
        private int FADE_IN;
        public int FadeInMessage { get => FADE_IN; }

        [SerializeField, AnimatorParam("myAnimator")]
        private int FADE_OUT;
        public int FadeOutMessage { get => FADE_OUT; }

        [SerializeField, AnimatorParam("myAnimator")]
        private int HIDE;
        public int HideMessage { get => HIDE; }

        [SerializeField, AnimatorParam("myAnimator")]
        private int HORIZONTAL_WIPE_IN;
        public int HorizontalWipeInMessage { get => HORIZONTAL_WIPE_IN; }

        [SerializeField, AnimatorParam("myAnimator")]
        private int HORIZONTAL_WIPE_OUT;
        public int HorizontalWipeOutMessage { get => HORIZONTAL_WIPE_OUT; }

        [SerializeField, AnimatorParam("myAnimator")]
        private int SHOW;
        public int ShowMessage { get => SHOW; }

        [SerializeField, AnimatorParam("myAnimator")]
        private int SPIN_LOCK_IN;
        public int SpinLockInMessage { get => SPIN_LOCK_IN; }

        [SerializeField, AnimatorParam("myAnimator")]
        private int SPIN_LOCK_OUT;
        public int SpinLockOutMessage { get => SPIN_LOCK_OUT; }

        [SerializeField, AnimatorParam("myAnimator")]
        private int VERTICAL_WIPE_IN;
        public int VerticalWipeInMessage { get => VERTICAL_WIPE_IN; }

        [SerializeField, AnimatorParam("myAnimator")]
        private int VERTICAL_WIPE_OUT;
        public int VerticalWipeOutMessage { get => VERTICAL_WIPE_OUT; }

        #endregion

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
        /// Use at own risk. Prefer to use explicit functions instead.
        /// </summary>
        /// <param name="trigger"></param>
        public void TriggerTransition(string trigger)
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

        [Button(null, EButtonEnableMode.Editor)]
        public void FadeIn()
            => myAnimator.SetTrigger(FADE_IN);

        [Button(null, EButtonEnableMode.Playmode)]
        public void FadeOut()
            => myAnimator.SetTrigger(FADE_OUT);

        [Button(null, EButtonEnableMode.Playmode)]
        public void HideScene()
            => myAnimator.SetTrigger(HIDE);

        [Button(null, EButtonEnableMode.Playmode)]
        public void HorizontalWipeIn()
            => myAnimator.SetTrigger(HORIZONTAL_WIPE_IN);

        [Button(null, EButtonEnableMode.Playmode)]
        public void HorizontalWipeOut()
            => myAnimator.SetTrigger(HORIZONTAL_WIPE_OUT);

        [Button(null, EButtonEnableMode.Playmode)]
        public void ShowScene()
            => myAnimator.SetTrigger(SHOW);

        [Button(null, EButtonEnableMode.Playmode)]
        public void SpinLockIn()
            => myAnimator.SetTrigger(SPIN_LOCK_IN);

        [Button(null, EButtonEnableMode.Playmode)]
        public void SpinLockOut()
            => myAnimator.SetTrigger(SPIN_LOCK_OUT);

        [Button(null, EButtonEnableMode.Playmode)]
        public void VerticalWipeIn()
            => myAnimator.SetTrigger(VERTICAL_WIPE_IN);

        [Button(null, EButtonEnableMode.Playmode)]
        public void VerticalWipeOut()
            => myAnimator.SetTrigger(VERTICAL_WIPE_OUT);

        //public void RandomTransitionIn()
        //public void RandomTransitionOut()

        #endregion
    }
}
