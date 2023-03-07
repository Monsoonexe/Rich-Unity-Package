using UnityEngine;
using UnityEngine.SceneManagement;
using RichPackage.Events.Signals;
using RichPackage.Audio;
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
        [SerializeField, MinValue(10), MaxValue(90)]
        private int targetFrameRate = 60;

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
        /// Is the application in the process of quitting? I.e. <see cref="Application.Quit"/>
        /// has been called.
        /// </summary>
        public static bool IsQuitting { get; private set; }

        #region Unity Messages

        protected override void Reset()
        {
            base.Reset();
            SetDevDescription("I control the application-level behaviour.");
        }

        protected override void Awake()
        {
            base.Awake();
            if (!Singleton.TakeOrDestroyGameObject(
                this, ref instance, dontDestroyOnLoad: true))
            {
                return;
            }

            MainThreadId = System.Threading.Thread.CurrentThread.ManagedThreadId;
            GlobalSignals.Get<RequestQuitGameSignal>().AddListener(QuitApplication);
            ApplyAppSettings();
            DG.Tweening.DOTween.Init();
            UnityServiceLocator.Instance.RegisterProvider<AudioManager>(AudioManager.Init);
        }

        private void OnApplicationQuit()
        {
            // check if was called through this api
            if (IsQuitting)
                return;

            // we were called by some other means, like exiting playmode in the editor
            OnApplicationQuitInternal();
        }

        private void OnDestroy()
        {
            if (Singleton.Release(this, ref instance))
            {
                GlobalSignals.Get<RequestQuitGameSignal>().RemoveListener(QuitApplication);
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
            Application.Quit();
        }

        private static void OnApplicationQuitInternal()
        {
            IsQuitting = true;
            GlobalSignals.Get<GameIsQuittingSignal>().Dispatch();
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
    public class GameIsQuittingSignal : ASignal { }

}
