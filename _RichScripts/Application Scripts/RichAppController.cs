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
    public class RichAppController : RichMonoBehaviour
    {
        private static RichAppController instance;
        
        public static RichAppController Instance
        {
            get
            {
#if UNITY_EDITOR
                if (!instance)
                {
                    Debug.LogWarning($"[{nameof(RichAppController)}] " +
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

            GlobalSignals.Get<RequestQuitGameSignal>().AddListener(QuitGame);
            SceneManager.sceneLoaded += SceneLoadedHandler;
            ApplyAppSettings();
            DG.Tweening.DOTween.Init();
            UnityServiceLocator.Instance.RegisterProvider<AudioManager>(AudioManager.Init);
        }

        private void OnDestroy()
        {
            if (Singleton.Release(this, ref instance))
            {
                GlobalSignals.Get<RequestQuitGameSignal>().RemoveListener(QuitGame);
                SceneManager.sceneLoaded -= SceneLoadedHandler;
            }
        }

        public void Update()
        {
            //cache time values to de-marshal them once
            DeltaTime = UnityEngine.Time.deltaTime;
            Time = UnityEngine.Time.time;
        }
        
        private void FixedUpdate()
        {
            //cache time values to de-marshal them once
            FixedDeltaTime = UnityEngine.Time.fixedDeltaTime;
        }

		#endregion Unity Messages
        
        private void ApplyAppSettings()
        {
            Application.targetFrameRate = targetFrameRate;

            // this also forces Admin to be JITed
            if (!Administration.Admin.IsAdmin && alwaysAdminInEditor && Application.isEditor)
                Administration.Admin.Login("whatever");
        }

		public void QuitGame()
        {
            GlobalSignals.Get<GameIsQuittingSignal>().Dispatch();
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#endif
            Application.Quit();
        }

		#region Static Interface

		private static void SceneLoadedHandler(Scene scene, LoadSceneMode mode)
            => GlobalSignals.Get<SceneLoadedSignal>().Dispatch();

        public static void ReloadCurrentLevel()
            => SceneManager.LoadScene(
                SceneManager.GetActiveScene().buildIndex);

        public static RichAppController Construct()
            => Construct<RichAppController>(nameof(RichAppController));

        public static void EnsureInstance()
		{
            if (!instance) Construct();
		}

		#endregion Static Interface
	}

    /// <summary>
    /// Dispatched when the Player has requested to close the app.
    /// </summary>
    public class GameIsQuittingSignal : ASignal { }

}
