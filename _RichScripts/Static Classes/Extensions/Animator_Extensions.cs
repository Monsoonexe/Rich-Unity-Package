using System.Collections;
using UnityEngine;

namespace RichPackage
{
    /// <summary>
    /// Helper methods for <see cref="Animator"/>.
    /// </summary>
    public static class Animator_Extensions
    {
        public static IEnumerator WaitForEnd(this Animator animator)
        {
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

            Debug.Assert(!state.loop, "I'll wait forever if the animation loops. looping is unsupported.", animator);

            yield return CoroutineUtilities.Delay(state.length);
        }

        public static IEnumerator WaitForExit(this Animator animator)
        {
            AnimatorStateInfo state = animator.GetCurrentAnimatorStateInfo(0);

            yield return CoroutineUtilities.WaitUntil(() =>
            {
                AnimatorStateInfo currentState = animator.GetCurrentAnimatorStateInfo(0);
                return currentState.fullPathHash != state.fullPathHash;
            });
        }
    }
}
