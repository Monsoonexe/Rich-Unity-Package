using System;
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
    public class RichAppController : RichMonoBehaviour
    {
        private static RichAppController instance;
        public static RichAppController Instance => instance;

        /// <summary>
        /// Time.deltaTime that has been cached and un-marshalled.
        /// </summary>
        public static float DeltaTime { get; private set; }

        /// <summary>
        /// Time.time that has been cached and un-marshalled.
        /// </summary>
        public static float Time { get; private set; }
        
        public static float FixedDeltaTime { get; private set; }

        private void Reset()
        {
            SetDevDescription("I control the application-level behaviour.");
        }

        protected override void Awake()
        {
            base.Awake();
            instance = this;//low-key singleton
            GlobalSignals.Get<RequestQuitGameSignal>().AddListener(QuitGame);
            SceneManager.sceneLoaded += SceneLoadedHandler;
            DG.Tweening.DOTween.Init();
            DG.Tweening.DOTween.SetTweensCapacity(500, 20);
        }

        private void OnDestroy()
        {
            GlobalSignals.Get<RequestQuitGameSignal>().RemoveListener(QuitGame);
            SceneManager.sceneLoaded -= SceneLoadedHandler;
        }

        public void QuitGame()
        {
            GlobalSignals.Get<GameIsQuittingSignal>().Dispatch();
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #endif
            Application.Quit();
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

        private void SceneLoadedHandler(Scene scene, LoadSceneMode mode)
            => GlobalSignals.Get<SceneLoadedSignal>().Dispatch();

        public static void ReloadCurrentLevel()
            => SceneManager.LoadScene(
                SceneManager.GetActiveScene().buildIndex);

        public static RichAppController Construct()
            => new GameObject(nameof(RichAppController)).AddComponent<RichAppController>();
    }

    /// <summary>
    /// Request that the app be closed.
    /// </summary>
    public class RequestQuitGameSignal : ASignal { }

    /// <summary>
    /// Dispatched when the Player has requested to close the app.
    /// </summary>
    public class GameIsQuittingSignal : ASignal { }

    /// <summary>
    /// Dispatched by SceneManager.sceneLoaded when such.
    /// </summary>
    public class SceneLoadedSignal : ASignal { }

    /// <summary>
    /// The scene is about to be unloaded. Last call before your possible demise! <br/>
    /// NOTE: If multiple scenes exist, it may not be 'your' scene that is being unloaded.
    /// See also: <seealso cref="SceneManager.sceneUnloaded"/>
    /// </summary>
    public class ScenePreUnload : ASignal { }
}
