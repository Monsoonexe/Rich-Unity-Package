/* Maybe make this an FSM that handles different inputs based on state
 * 
 */

using FSM;
using QFSW.QC;
using RichPackage.InputSystem;
using Sirenix.OdinInspector;
using System;
using UnityEngine;

namespace RichPackage
{
    /// <summary>
    /// The main point of entry for player input and mouse input.
    /// </summary>
    public partial class PlayerInput : RichMonoBehaviour
    {
        #region Static Fields

        private static PlayerInput s_instance;

        public static bool IsActive { get; private set; }

        #region Events

        public static event Action OnQuitGameEvent;

        #endregion Events

        #endregion Static Fields

        /// <summary>
        /// Handles <see cref="Input"/> for a given state of the game.
        /// </summary>
        [Serializable]
        public partial class InputProfile : StateBase
        {
            [ShowInInspector, ReadOnly]
            public bool IsActive { get; private set; }

            #region Events

            /// <summary>
            /// Raised when this profile becomes the active input profile.
            /// </summary>
            public event Action OnActivated;

            /// <summary>
            /// Raised when this profile stops being the active input profile.
            /// </summary>
            public event Action OnDeactivated;

            #endregion Events

            #region Constructors

            public InputProfile() : base(needsExitTime: false) { }

            #endregion Constructors

            #region StateBase

            /// <remarks>Inheritors should call <see cref="OnEnter"/></remarks>
            public override void OnEnter()
            {
                IsActive = true;
                OnActivated?.Invoke();
            }
            
            /// <remarks>Do <see cref="Input"/> logic here.</remarks>
            public override void OnLogic()
            {
                if (IsAnyShiftDown() && Input.GetKeyDown(KeyCode.Escape))
                {
                    OnQuitGameEvent?.Invoke();
                }
            }

            /// <remarks>Inheritors should call <see cref="OnExit"/></remarks>
            public override void OnExit()
            {
                OnDeactivated?.Invoke();
                IsActive = false;
            }

            #endregion StateBase

            #region Input Helpers

            public bool IsAnyShiftDown()
            {
                return s_instance.IsAnyShiftDownInternal();
            }

            #endregion Input Helpers

            /// <summary>
            /// Sets this profile as the active profile.
            /// </summary>
            public void SetActive()
            {
                SetInputProfile(this);
            }

            #region Object

            public override string ToString()
            {
                return name;
            }

            #endregion Object
        }

        [Title("Settings")]
        public bool debug = false;

        [Title("Input Axes")]
        [SerializeField, NaughtyAttributes.InputAxis]
        private string focusPlayer;

        [Title("Profiles")]
        [SerializeField, Required, Tooltip("[0] is the default profile.")]
        private AInputProfileAsset[] inputProfiles;

        [ShowInInspector, LabelText("Active Profile")]
        private string Editor_ActiveProfileName => SafeGetActiveInputProfileName();

        public static string ActiveInputProfileName
        {
            get => s_instance.fsm.ActiveStateName;
            set => SetInputProfile(value);
        }

        private readonly StateMachine fsm = new StateMachine();

        #region Unity Events

        protected override void Awake()
        {
            if (!Singleton.TakeOrDestroy(this, ref s_instance, dontDestroyOnLoad: false))
                return;
        }

        private void Start()
        {
            QuantumConsole.Instance.OnActivate += OnConsoleActivate;
            QuantumConsole.Instance.OnDeactivate += OnConsoleDeactivate;

            InitStateMachine();
        }

        private void OnDestroy()
        {
            QuantumConsole.Instance.OnActivate -= OnConsoleActivate;
            QuantumConsole.Instance.OnDeactivate -= OnConsoleDeactivate;

            Singleton.Release(this, ref s_instance);
        }

        private void OnEnable()
        {
            IsActive = true;
        }

        private void OnDisable()
        {
            IsActive = false;
        }

        private void Update()
        {
            fsm.OnLogic();
        }

        #endregion Unity Events

        #region Initialization

        private void InitStateMachine()
        {
            InitStates();
            fsm.SetStartState(inputProfiles[0].Profile.name);
            fsm.Init();
        }

        private void InitStates()
        {
            int count = inputProfiles?.Length ?? -1;

            // handle null or empty array
            if (count <= 0)
            {
                AddProfile(new InputProfile()
                {
                    name = "Default",
                });

                return;
            }

            for (int i = 0; i < count; ++i)
            {
                AddProfile(inputProfiles[i]);
            }
        }

        private void AddProfile(InputProfile profile)
        {
            fsm.AddState(profile.name, profile);
        }

        #endregion Initialization

        #region Managing Profiles

        [Button, DisableInEditorMode]
        public static void SetInputProfile(AInputProfileAsset inputProfile)
            => SetInputProfile(inputProfile.Profile);

        public static void SetInputProfile(InputProfile inputProfile)
            => SetInputProfile(inputProfile.name);

        public static void SetInputProfile(string profileName)
            => s_instance.fsm.RequestStateChange(profileName, forceInstantly: true);

        public static bool IsInputProfileActive(InputProfile inputProfile)
            => ReferenceEquals(inputProfile, s_instance.fsm.ActiveState);

        #endregion Managing Profiles

        public static bool PollFocusPlayerButton()
        {
            return IsActive && Input.GetButtonDown(s_instance.focusPlayer);
        }

        public static bool IsAnyShiftDown()
            => IsActive && s_instance.IsAnyShiftDownInternal();

        private bool IsAnyShiftDownInternal()
        {
            return Input.GetKey(KeyCode.LeftShift)
                || Input.GetKey(KeyCode.RightShift);
        }

        private void OnConsoleDeactivate()
        {
            enabled = true;
        }

        private void OnConsoleActivate()
        {
            enabled = false;
        }

        private static string SafeGetActiveInputProfileName()
        {
            const string None = "none";

            if (s_instance == null)
                return None;

            if (s_instance.fsm == null)
                return None;

            return s_instance.fsm.ActiveState == null ? None : s_instance.fsm.ActiveStateName;
        }
    }
}
