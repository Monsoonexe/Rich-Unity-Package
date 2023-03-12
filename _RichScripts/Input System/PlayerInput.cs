using FSM;
using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RichPackage.InputSystem
{
    /// <summary>
    /// The main point of entry for player input and mouse input.
    /// </summary>
    public sealed partial class PlayerInput : RichMonoBehaviour
    {
        [Title("Settings")]
        public bool debug = false;

        [ShowInInspector, LabelText("Active Profile")]
        private string Editor_ActiveProfileName => SafeGetActiveInputProfileName();

        public string ActiveProfileName => fsm.ActiveStateName;

        public Profile CurrentProfile => (Profile)fsm.ActiveState;

        public StateBase<string> ActiveProfile
        {
            get => fsm.ActiveState;
            set => SetProfile(value.name);
        }

        private readonly StateMachine fsm = new StateMachine();

        #region Unity Messages

        private void Update()
        {
            fsm.OnLogic();
        }

        #endregion Unity Messages

        #region Initialization

        public void Init(IEnumerable<Profile> profiles, Profile startingProfile)
        {
            InitStates(profiles);
            fsm.SetStartState(startingProfile.name);
            fsm.Init();
        }

        private void InitStates(IEnumerable<Profile> profiles)
        {
            // handle null or empty collection
            int count = profiles?.Count() ?? -1;
            if (count <= 0)
            {
                AddProfile(new Profile()
                {
                    name = "Default",
                });

                return;
            }

            // init each state
            foreach (var p in profiles)
            {
                AddProfile(p);
            }
        }

        private void AddProfile(Profile profile)
        {
            fsm.AddState(profile.name, profile);
        }

        #endregion Initialization

        #region Managing Profiles

        [Button, DisableInEditorMode]
        public void SetInputProfile(AInputProfileAsset inputProfile)
            => SetInputProfile(inputProfile.Profile);

        public void SetInputProfile(Profile inputProfile)
            => SetProfileInternal(inputProfile.name);

        public void SetProfile(string profileName)
            => SetProfileInternal(profileName);

        private void SetProfileInternal(string profileName)
            => fsm.RequestStateChange(profileName, forceInstantly: true);

        public bool IsProfileActive(Profile inputProfile)
            => ReferenceEquals(inputProfile, ActiveProfile);

        #endregion Managing Profiles

        private string SafeGetActiveInputProfileName()
        {
            const string None = "none";

#if UNITY_EDITOR
            if (!Application.isPlaying)
                return None;
#endif

            if (fsm == null)
                return None;

            if (fsm.ActiveState == null)
                return None;

            return fsm.ActiveStateName;
        }
        
        /// <summary>
        /// Handles <see cref="Input"/> for a given state of the game.
        /// </summary>
        [Serializable]
        public partial class Profile : StateBase
        {
            [Title("Input Profile")]
            [ShowInInspector, ReadOnly]
            public bool IsActive { get; private set; }

            /// <summary>
            /// Additional querries to make while in this context.
            /// </summary>
            public event Action Update;

            #region Constructors

            public Profile() : base(needsExitTime: false) { }

            #endregion Constructors

            #region StateBase

            /// <remarks>Inheritors should call <see cref="OnEnter"/></remarks>
            public override void OnEnter()
            {
                IsActive = true;
            }

            /// <remarks>Do <see cref="Input"/> logic here.</remarks>
            public override void OnLogic()
            {
                Update?.Invoke();
            }

            /// <remarks>Inheritors should call <see cref="OnExit"/></remarks>
            public override void OnExit()
            {
                IsActive = false;
            }

            protected void UpdateLogic()
            {
                Update?.Invoke();
            }

            #endregion StateBase

            #region Input Helpers

            protected bool IsAnyShiftDown()
            {
                return PlayerInput.IsAnyShiftDown();
            }

            #endregion Input Helpers

            #region Object

            public override string ToString()
            {
                return name;
            }

            #endregion Object
        }

    }
}
