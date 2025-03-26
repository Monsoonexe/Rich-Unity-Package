using QFSW.QC;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace RichPackage.InputSystem
{
    /// <summary>
    /// Controls which input profiles are active based on the game state.
    /// </summary>
    /// <seealso cref="PlayerInput"/>
    public class PlayerInputController : RichMonoBehaviour
    {
        private static PlayerInputController s_instance;

        [Title("Prefab Refs")]
        [SerializeField, Required]
        protected PlayerInput input;

        [Title("Profiles")]
        [SerializeField, Required]
        protected AInputProfileAsset defaultProfile;

        [SerializeField, Required]
        protected AInputProfileAsset consoleProfile;

        private readonly Stack<string> profileHistory = new Stack<string>();

        #region Events

        /// <summary>
        /// Subscribe an input event to run in the current context.
        /// </summary>
        public event Action Update
        {
            add => input.CurrentProfile.Update += value;
            remove => input.CurrentProfile.Update -= value;
        }

        #endregion Events

        #region Unity Messages

        protected override void Reset()
        {
            SetDevDescription("Controls which input profiles are active based on the game state.");
            input = GetComponent<PlayerInput>();
        }

        protected override void Awake()
        {
            if (!Singleton.TakeOrDestroy(this, ref s_instance, dontDestroyOnLoad: false))
                return;

            input.Init(EnumerateProfiles(), defaultProfile);
        }

        protected virtual void Start()
        {
            // events
            QuantumConsole.Instance.OnActivate += OnConsoleActivate;
            QuantumConsole.Instance.OnDeactivate += OnConsoleDeactivate;
        }

        protected virtual void OnDestroy()
        {
            // events
            QuantumConsole.Instance.OnActivate -= OnConsoleActivate;
            QuantumConsole.Instance.OnDeactivate -= OnConsoleDeactivate;

            // singleton
            Singleton.Release(this, ref s_instance);
        }

        #endregion Unity Messages

        #region Profile Management

        public static void StashAndSetProfile(PlayerInput.Profile inputProfile)
            => s_instance.StashAndSetProfileInternal(inputProfile);

        private void StashAndSetProfileInternal(PlayerInput.Profile inputProfile)
        {
            profileHistory.Push(input.ActiveProfileName); // stash
            input.SetProfile(inputProfile.name); // set
        }

        public static void PopProfile()
            => s_instance.PopProfileInternal();

        private void PopProfileInternal()
        {
            input.SetProfile(profileHistory.Pop());
        }

        protected virtual IEnumerable<PlayerInput.Profile> EnumerateProfiles()
        {
            yield return defaultProfile;
            yield return consoleProfile;
        }

        #endregion Profile Management

        #region Console Event Handlers

        private void OnConsoleDeactivate()
        {
#if UNITY_EDITOR
            // is exiting playmode check
            if (App.IsQuitting)
                return;
#endif
            PopProfile();
        }

        private void OnConsoleActivate()
        {
            StashAndSetProfile(consoleProfile);
        }

        #endregion Console Event Handlers
    }
}
