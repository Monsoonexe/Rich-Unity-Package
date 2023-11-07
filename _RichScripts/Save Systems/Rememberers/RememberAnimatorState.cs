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

        /// <summary>
        /// The live state of the animator when this was last disabled.
        /// </summary>
        private AnimatorStateInfo? stagedState;

        protected override void Reset()
        {
            base.Reset();
            SetDevDescription("Remembers the last time and state of the animation.");
            myAnimator = GetComponent<Animator>();
            SaveID = UniqueIdUtilities.CreateIdFrom(this, includeScene: false);
        }

        protected override void OnDisable()
        {
            base.OnDisable();
            stagedState = myAnimator.GetCurrentAnimatorStateInfo(0); // cache the current state in case we need to save
        }

        protected override void OnEnable()
        {
            base.OnEnable();
            stagedState = null; // use live state instead when enabled
        }

        #region Save/Load

        protected override void LoadStateInternal()
        {
            myAnimator.Play(SaveData.stateHash, -1, SaveData.time);
        }

        protected override void SaveStateInternal()
        {
            AnimatorStateInfo state = stagedState ?? myAnimator.GetCurrentAnimatorStateInfo(0);
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
