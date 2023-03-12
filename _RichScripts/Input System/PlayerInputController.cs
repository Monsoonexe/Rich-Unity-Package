using QFSW.QC;
using Sirenix.OdinInspector;
using System.Collections.Generic;
using UnityEngine;

namespace RichPackage.InputSystem
{
    /// <summary>
    /// Controls which input profiles are active based on the game state.
    /// </summary>
    public class PlayerInputController : RichMonoBehaviour
    {
        private static PlayerInputController s_instance;

        [Title("Settings")]
        [SerializeField, Required]
        private AInputProfileAsset startingProfile;

        [Title("Prefab Refs")]
        [SerializeField, Required]
        private PlayerInput input;

        [Title("Profiles")]
        [SerializeField, Required]
        private AInputProfileAsset defaultProfile;

        [SerializeField, Required]
        private AInputProfileAsset consoleProfile;

        private readonly Stack<string> profileHistory = new Stack<string>();

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

            input.Init(EnumerateProfiles(), startingProfile);
        }

        private void Start()
        {
            QuantumConsole.Instance.OnActivate += OnConsoleActivate;
            QuantumConsole.Instance.OnDeactivate += OnConsoleDeactivate;
        }

        private void OnDestroy()
        {
            Singleton.Release(this, ref s_instance);
            QuantumConsole.Instance.OnActivate -= OnConsoleActivate;
            QuantumConsole.Instance.OnDeactivate -= OnConsoleDeactivate;
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

        private IEnumerable<PlayerInput.Profile> EnumerateProfiles()
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