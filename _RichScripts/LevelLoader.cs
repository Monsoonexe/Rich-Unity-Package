using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ScriptableObjectArchitecture;
using Sirenix.OdinInspector;
using RichPackage.UI;
using RichPackage.Events.Signals;

//TODO - handle additive scenes.

namespace RichPackage
{
    /// <summary>
    /// I help control the flow of changing between Levels.
    /// </summary>
    public class LevelLoader : RichMonoBehaviour
    {
        [Title("---Settings---")]

        [SerializeField]
        private string inTransitionMessage = "VerticalWipeIn";

        [SerializeField]
        private string outTransitionMessage = "VerticalWipeOut";

        [Header("---Audio---")]
        [SerializeField]
        private AudioClipReference transitionOUTClip 
            = new AudioClipReference();

        [SerializeField]
        private AudioClipReference transitionINClip
            = new AudioClipReference();

        //public static event Action<SceneVariable> OnSceneLoaded;

        public static SceneVariable CurrentScene { get; private set; }

        //runtime data
        [ShowInInspector, ReadOnly]
        public static bool IsTransitioning { get; private set; } = false;
        private bool levelHasLoaded;

        /// <summary>
        /// Predicate to pass to WaitUntil
        /// </summary>
        /// <returns></returns>
        private bool LevelHasFinishedLoading() => levelHasLoaded;

        private static Stack<SceneVariable> levelHistory = new Stack<SceneVariable>();

        protected override void Reset()
        {
            base.Reset();
            SetDevDescription("I help control the flow of changing between Levels.");
        }

        private void Start()
        {
            // TODO - get current scene
        }

        /// <summary>
        /// The best way to load a level, as it can be tracked.
        /// </summary>
        /// <param name="sceneVariable"></param>
        /// <exception cref="ArgumentNullException"></exception>
        [Button, DisableInEditorMode]
		public void LoadLevel(SceneVariable sceneVariable)
		{
            //validate
            if (sceneVariable == null)
                throw new ArgumentNullException(nameof(sceneVariable));

            //work
            levelHistory.Push(CurrentScene);
            CurrentScene = sceneVariable; //track
            LoadLevel(CurrentScene.Value.SceneIndex); //load
        }

        /// <summary>
        /// Load the level that brought us to this one.
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        [Button, DisableInEditorMode]
        public void LoadPreviousLevel()
		{
            //validate
            if (levelHistory.Count == 0)
                throw new InvalidOperationException("There is no previous level to load.");

            //work
            CurrentScene = levelHistory.Pop(); //track
            LoadLevel(CurrentScene.Value.SceneIndex); //load
        }

        [Button, DisableInEditorMode]
        private void LoadLevel(string name)
        {
            if (!IsTransitioning)//prevent spamming
            {
                StartCoroutine(LoadLevelRoutine(
                    () => SceneManager.LoadScene(name))); //load scene by string
            }
        }

        [Button, DisableInEditorMode]
        private void LoadLevel(int index)
        {
            if (!IsTransitioning)//prevent spamming
            {
                StartCoroutine(LoadLevelRoutine(
                    () => SceneManager.LoadScene(index))); //load scene by index
            }
        }

        private IEnumerator LoadLevelRoutine(Action LoadSceneAction)
        {
            //reset flags
            IsTransitioning = true;
            levelHasLoaded = false;

            var transitionWait = new WaitForSeconds(
                ScreenTransitionController.TransitionDuration);

            //preserve coroutine with immortality!
            myTransform.parent = null;// dontdestroyonload only works on objects at root
            DontDestroyOnLoad(gameObject); //don't destroy while we are loading the level

            //fade out
            ScreenTransitionController.Instance
                .TriggerTransition(outTransitionMessage);//hide scene
            transitionOUTClip.Value.PlaySFX();

            //wait for the darkness to envelope you, and then a bit longer
            yield return transitionWait;

            //last call before a scene is destroyed.
            GlobalSignals.Get<ScenePreUnload>().Dispatch();

            //perform scene load
            SceneManager.sceneLoaded += OnLevelLoaded; //subscribe to event
            //SceneManager.LoadScene(index);
            LoadSceneAction(); //many ways to load a scene, but use this one.

            //wait
            yield return new WaitUntil(LevelHasFinishedLoading); //levelHasLoaded = true

            levelHasLoaded = false; //reset flag

            //clean up while screen still black
            SceneManager.sceneLoaded -= OnLevelLoaded; //unsubscribe to event
            GC.Collect(); //why not? screen is totally black now.

            //wait until next frame
            yield return null;

            //fade into new scene
            ScreenTransitionController.Instance.
                TriggerTransition(inTransitionMessage);

            transitionINClip.Value.PlaySFX();

            //wait for scene to fully open, and just a bit longer
            yield return transitionWait;

            //finalize
            IsTransitioning = false;
            Destroy(gameObject);//left over from previous level
        }

        /// <summary>
        /// Rigged to SceneManager.levelLoaded
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="mode"></param>
        private void OnLevelLoaded(Scene scene, LoadSceneMode mode)
        {
            levelHasLoaded = true;//set flag
        }

        #region Static Interface

        [Button("LoadLevelImmediately"), DisableInEditorMode]
        public static void LoadLevel_(int levelIndex)
		{
            SceneManager.LoadScene(levelIndex);
		}

        //[Button, DisableInEditorMode]
        //public static void LoadNextLevel()
        //{
        //    var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        //    SceneManager.LoadScene(currentSceneIndex + 1);
        //}

        //[Button, DisableInEditorMode]
        //public static void LoadPreviousLevel()
        //{
        //    var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        //    SceneManager.LoadScene(currentSceneIndex - 1);
        //}

        [Button, DisableInEditorMode]
        public static void ReloadCurrentLevel()
            => SceneManager.LoadScene(
                SceneManager.GetActiveScene().buildIndex);

        #endregion
    }
}
