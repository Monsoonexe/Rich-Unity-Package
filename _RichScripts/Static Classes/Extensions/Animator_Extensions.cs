using UnityEngine;

namespace RichPackage
{
    /// <summary>
    /// Helper methods for <see cref="Animator"/>.
    /// </summary>
    public static class Animator_Extensions
    {
        public static Coroutine WaitForEnd(this Animator animator)
        {
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

            Debug.Assert(!state.loop, "I'll wait forever if the animation loops. looping is unsupported.", animator);

            return CoroutineUtilities.Delay(state.length);
        }

        public static Coroutine WaitForExit(this Animator animator)
        {
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

            if (state.loop)
            {
                return CoroutineUtilities.WaitUntil(() =>
                {
                    AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
                    return currentState.fullPathHash != state.fullPathHash;
                });
            }
            else
            {
                return WaitForEnd(animator);
            }
        }

        public static Coroutine PlayThenDisable(this Animator animator, string stateName)
        {
            animator.Play(stateName);
            return CoroutineUtilities.StartInvokeAfter(() => animator.enabled = false,
                WaitForExit(animator));
        }

        public static void PlayLastFrame(this Animator animator, string stateName)
        {
            animator.Play(stateName);
            AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
            animator.playbackTime = currentState.length;
        }

        public static void SkipToEnd(this Animator animator)
        {
            AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
            animator.Play(currentState.fullPathHash, -1, currentState.length);
        }
    }
}
