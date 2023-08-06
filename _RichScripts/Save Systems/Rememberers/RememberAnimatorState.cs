using Sirenix.OdinInspector;
using UnityEngine;

namespace RichPackage.SaveSystem
{
    /// <summary>
    /// Remembers the last time and state of the animation.
    /// </summary>
    public sealed class RememberAnimatorState : ASaveableMonoBehaviour<RememberAnimatorState.Memento>
    {
        [Required]
        public Animator myAnimator;

        protected override void Reset()
        {
            base.Reset();
            SetDevDescription("Remembers the last time and state of the animation.");
            myAnimator = GetComponent<Animator>();
            SaveID = $"{name}-{nameof(RememberAnimatorState)}";
        }

        #region Save/Load

        protected override void LoadStateInternal()
        {
            myAnimator.Play(SaveData.stateHash, -1, SaveData.time);
        }

        protected override void SaveStateInternal()
        {
            AnimatorStateInfo state = myAnimator.GetCurrentAnimatorStateInfo(0);
            SaveData.time = state.normalizedTime;
            SaveData.stateHash = state.fullPathHash;
        }

        [System.Serializable]
        public class Memento : AState
        {
            public float time;
            public int stateHash;
        }

        #endregion Save/Load
    }
}
