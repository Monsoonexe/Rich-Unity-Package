using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using ScriptableObjectArchitecture;
using Sirenix.OdinInspector;
using RichPackage.UI;
using Signals;

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

        //runtime data
        [ShowInInspector, ReadOnly]
        public static bool IsTransitioning { get; private set; } = false;
        private bool levelHasLoaded;

        /// <summary>
        /// Predicate to pass to WaitUntil
        /// </summary>
        /// <returns></returns>
        private bool LevelHasFinishedLoading() => levelHasLoaded;

        /// <summary>
        /// Supports asynchronous loading.
        /// </summary>
        private WaitUntil waitUntilFinishedLoading;

        private void Reset()
        {
            SetDevDescription("I help control the flow of changing between Levels.");
        }

        private void Start()
        {
            waitUntilFinishedLoading = new WaitUntil(LevelHasFinishedLoading);
        }

        [Button, DisableInEditorMode]
        public void LoadLevel(SceneVariable sceneVariable)
            => LoadLevel(sceneVariable.Value.SceneIndex);

        [Button, DisableInEditorMode]
        public void LoadLevel(string name)
        {
            if (!IsTransitioning)//prevent spamming
            {
                StartCoroutine(LoadLevelRoutine(
                    () => SceneManager.LoadScene(name))); //load scene by string
            }
        }

        [Button, DisableInEditorMode]
        public void LoadLevel(int index)
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
            transitionOUTClip.PlaySFX();

            //wait for the darkness to envelope you
            yield return transitionWait;

            //last call before a scene is destroyed.
            GlobalSignals.Get<ScenePreUnload>().Dispatch();

            //perform scene load
            SceneManager.sceneLoaded += OnLevelLoaded; //subscribe to event
            //SceneManager.LoadScene(index);
            LoadSceneAction(); //many ways to load a scene, but use this one.

            //wait
            yield return waitUntilFinishedLoading; //levelHasLoaded = true

            levelHasLoaded = false; //reset flag

            //clean up while screen still black
            SceneManager.sceneLoaded -= OnLevelLoaded; //unsubscribe to event
            GC.Collect(); //why not? screen is totally black now.

            //wait until next frame
            yield return null;

            //fade into new scene
            ScreenTransitionController.Instance.
                TriggerTransition(inTransitionMessage);

            transitionINClip.PlaySFX();

            //wait
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

        [Button, DisableInEditorMode]
        public static void LoadNextLevel()
        {
            var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex + 1);
        }

        [Button, DisableInEditorMode]
        public static void LoadPreviousLevel()
        {
            var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex - 1);
        }

        [Button, DisableInEditorMode]
        public static void ReloadCurrentLevel()
            => SceneManager.LoadScene(
                SceneManager.GetActiveScene().buildIndex);

        #endregion
    }
}
