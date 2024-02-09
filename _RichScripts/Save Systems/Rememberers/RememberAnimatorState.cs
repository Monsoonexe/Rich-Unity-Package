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
        public Animator myAnimator; // TODO - rename to 'target'

        /// <summary>
        /// The live state of the animator when this was last disabled.
        /// </summary>
        private AnimatorStateInfo? stagedState;

        #region Unity Messages

        protected override void Reset()
        {
            base.Reset();
            SetDevDescription("Remembers the last time and state of the animation.");
            myAnimator = GetComponent<Animator>();
            SaveID = UniqueIdUtilities.CreateIdFrom(this, includeScene: true);
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

        #endregion Unity Messages

        #region Save/Load

        protected override void LoadStateInternal()
        {
            myAnimator.Play(SaveData.stateHash, -1, SaveData.time);
        }

        public override void SaveState(ISaveStore saveFile)
        {
            // it looks like we have nothing to save
            if (stagedState == null && !myAnimator.isActiveAndEnabled)
                return;
            
            base.SaveState(saveFile);
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

        #region Editor
#if UNITY_EDITOR

        [UnityEditor.MenuItem("CONTEXT/" + nameof(Animator) + "/Add Rememberer")]
        private static void AddRememberer(UnityEditor.MenuCommand command)
        {
            var t = (Animator)command.context; // the thing clicked on
            t.gameObject.AddComponent<RememberAnimatorState>()
                .myAnimator = t; // assign this thing as the thing to be saved
        }

#endif
        #endregion Editor
    }
}
