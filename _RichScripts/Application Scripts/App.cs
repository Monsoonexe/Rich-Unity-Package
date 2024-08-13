using UnityEngine;
using UnityEngine.SceneManagement;
using RichPackage.Events.Signals;
using Sirenix.OdinInspector;

namespace RichPackage
{
    /// <summary>
    /// I control the application-level behaviour.
    /// </summary>
    /// <seealso cref="GameController"/>
    public sealed partial class App : RichMonoBehaviour
    {
        private static App instance;
        
        public static App Instance
        {
            get
            {
#if UNITY_EDITOR
                if (!instance)
                {
                    Debug.LogWarning($"[{nameof(App)}] " +
                        "Instance is being created. In a build, this would be a " +
                        $"{nameof(System.NullReferenceException)}, " +
                        "but in the editor you are protected.");

                    EnsureInstance();
                }
#endif
                return instance;
            }
        }

        [Title("App Settings")]
        [SerializeField, MinValue(10), MaxValue(120)]
        private int targetFrameRate = 60;

        [SerializeField, Tooltip("Determines whether the hardware pointer is visible or not.")]
        private bool cursorVisible = false;

        [SerializeField]
        private bool alwaysAdminInEditor = true;

        /// <summary>
        /// <see cref="Time.deltaTime"/> that has been cached and un-marshalled.
        /// </summary>
        public static float DeltaTime { get; private set; }

        /// <summary>
        /// <see cref="Time.time"/> that has been cached and un-marshalled.
        /// </summary>
        public static float Time { get; private set; }

        /// <summary>
        /// <see cref="Time.fixedDeltaTime"/> that has been cached and un-marshalled.
        /// </summary>
        public static float FixedDeltaTime { get; private set; }

        /// <summary>
        /// The thread id of Unity's main thread.
        /// </summary>
        public static int MainThreadId { get; private set; } // probably '1'

        /// <summary>
        /// Is this code running on Unity's main thread?
        /// </summary>
        public static bool IsMainThread => System.Threading.Thread.CurrentThread.ManagedThreadId == MainThreadId;

        /// <summary>
        /// Backing value for <see cref="IsQuitting"/>.
        /// </summary>
        private static bool _isQuitting;

        /// <summary>
        /// Is the application in the process of quitting? I.e. <see cref="Application.Quit"/>
        /// has been called.
        /// </summary>
        public static bool IsQuitting
        {
#if UNITY_EDITOR
            get => _isQuitting 
                || (!UnityEditor.EditorApplication.isPlayingOrWillChangePlaymode &&
                    UnityEditor.EditorApplication.isPlaying);
#else
            get => _isQuitting;
#endif
            private set => _isQuitting = value;
        }

        #region Unity Messages

        protected override void Reset()
        {
            base.Reset();
            SetDevDescription("I control the application-level behaviour.");
        }

        protected override void Awake()
        {
            _isQuitting = false;
            base.Awake();
            if (!Singleton.TakeOrDestroyGameObject(
                this, ref instance, dontDestroyOnLoad: true))
            {
                return;
            }

            MainThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
            ApplyAppSettings();
            DG.Tweening.DOTween.Init();
        }

        private void OnApplicationQuit()
        {
            // we were called by some other means, like exiting playmode in the editor
            OnApplicationQuitInternal();
        }

        private void OnDestroy()
        {
            if (Singleton.Release(this, ref instance))
            {
                // 
            }
        }

        private void Update()
        {
            // cache time values to de-marshal them once
            DeltaTime = UnityEngine.Time.deltaTime;
            Time = UnityEngine.Time.time;
        }
        
        private void FixedUpdate()
        {
            // cache time values to de-marshal them once
            FixedDeltaTime = UnityEngine.Time.fixedDeltaTime;
        }

        #endregion Unity Messages

        private void AttemptAutoAdminLogin()
        {
            // this also forces Admin to be JITed
            if (!Administration.Admin.IsAdmin && alwaysAdminInEditor && Application.isEditor)
                Administration.Admin.Login("whatever");
        }
        
        private void ApplyAppSettings()
        {
            Application.targetFrameRate = targetFrameRate;
            Cursor.visible = cursorVisible;
            AttemptAutoAdminLogin();
        }

        /// <summary>
        /// Quits the entire Unity application.
        /// </summary>
        public static void QuitApplication() => QuitApplication(0);

        /// <summary>
        /// Quits the entire Unity application.
        /// </summary>
		public static void QuitApplication(int exitCode)
        {
            OnApplicationQuitInternal();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit(exitCode);
        }

        private static void OnApplicationQuitInternal()
        {
            IsQuitting = true;
            GlobalSignals.Get<GameIsQuittingSignal>().Dispatch(); // should be a fire-once event
        }

        #region Static Interface

        public static void ReloadCurrentLevel()
        {
            SceneManager.LoadScene(
                SceneManager.GetActiveScene().buildIndex);
        }

        public static App Construct()
            => Construct<App>(nameof(App));

        public static App EnsureInstance()
		{
            if (!instance) Construct();
            return instance;
		}

        #endregion Static Interface
	}

    /// <summary>
    /// Dispatched when the Player has requested to close the app.
    /// </summary>
    /// <remarks>Listeners are removed after this event is dispatched (a game only quits once).</remarks>
    public class GameIsQuittingSignal : ABaseSignal
    {
        private System.Action callback;

        public void AddListener(System.Action handler) => callback += handler;
        public void RemoveListener(System.Action handler) => callback -= handler;
        public void Dispatch()
        {
            callback?.Invoke();
            callback = null; // should only ever be called once -- supports lambdas
        }
    }
}
