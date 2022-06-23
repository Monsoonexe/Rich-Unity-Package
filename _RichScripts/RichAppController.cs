using UnityEngine;
using UnityEngine.SceneManagement;
using RichPackage.Events.Signals;
using RichPackage.FunctionalProgramming;

namespace RichPackage
{
    /// <summary>
    /// I control the application-level behaviour.
    /// </summary>
    /// <seealso cref="GameController"/>
    public class RichAppController : RichMonoBehaviour
    {
        private static RichAppController instance;
        public static RichAppController Instance => instance;

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
            if (InitSingleton(this, ref instance, dontDestroyOnLoad: true)
                .IsFalse())
                return;

            instance = this;//low-key singleton
            GlobalSignals.Get<RequestQuitGameSignal>().AddListener(QuitGame);
            SceneManager.sceneLoaded += SceneLoadedHandler;
            DG.Tweening.DOTween.Init();
            DG.Tweening.DOTween.SetTweensCapacity(250, 20);
        }

        private void OnDestroy()
        {
            GlobalSignals.Get<RequestQuitGameSignal>().RemoveListener(QuitGame);
            SceneManager.sceneLoaded -= SceneLoadedHandler;

            //release singleton
            if (instance == this)
                instance = null;
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
            => new GameObject(nameof(RichAppController)).AddComponent<RichAppController>();

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
