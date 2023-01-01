using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using ScriptableObjectArchitecture;
using Sirenix.OdinInspector;
using RichPackage.UI;
using RichPackage.Events.Signals;
using RichPackage.YieldInstructions;
using RichPackage.Audio;

//TODO - handle additive scenes.

namespace RichPackage
{
    /// <summary>
    /// I help control the flow of changing between Levels.
    /// </summary>
    public class LevelLoader : RichMonoBehaviour
    {
        private delegate AsyncOperation LoadLevelAsyncOperation();

        [Title("---Settings---")]
        [SerializeField]
        private string inTransitionMessage = "VerticalWipeIn";

        [SerializeField]
        private string outTransitionMessage = "VerticalWipeOut";

        [SerializeField, Required]
        private ScreenTransitionController transitioner;

        [Header("---Audio---")]
        [SerializeField]
        private RichAudioClipReference transitionOUTClip 
            = new RichAudioClipReference();

        [SerializeField]
        private RichAudioClipReference transitionINClip
            = new RichAudioClipReference();

        public static SceneVariable CurrentScene { get; private set; }

        // runtime data
        [ShowInInspector, ReadOnly]
        public static bool IsTransitioning { get; private set; } = false;

        private static readonly Stack<SceneVariable> levelHistory = new Stack<SceneVariable>();

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

        public void ClearLevelHistory()
        {
            levelHistory.Clear();
        }

        [Button, DisableInEditorMode]
        private void LoadLevel(string name)
        {
            if (!IsTransitioning)//prevent spamming
            {
                StartCoroutine(LoadLevelRoutine(
                    () => SceneManager.LoadSceneAsync(name))); //load scene by string
            }
        }

        [Button, DisableInEditorMode]
        private void LoadLevel(int index)
        {
            if (!IsTransitioning)//prevent spamming
            {
                StartCoroutine(LoadLevelRoutine(
                    () => SceneManager.LoadSceneAsync(index))); //load scene by index
            }
        }

        private IEnumerator LoadLevelRoutine(LoadLevelAsyncOperation LoadSceneAction)
        {
            // set flags
            IsTransitioning = true;

            YieldInstruction transitionWait = transitioner.TransitionWaitInstruction;

            // preserve coroutine with immortality!
            myTransform.parent = null;// dontdestroyonload only works on objects at root
            DontDestroyOnLoad(gameObject); //don't destroy while we are loading the level

            // fade out
            transitioner.TriggerTransition(outTransitionMessage);//hide scene
            transitionOUTClip.Clip.PlayOneShot();

            yield return null;
            // make sure frame has been fully issued so state can be saved.
            yield return CommonYieldInstructions.WaitForEndOfFrame; //for event call
            RichAppController.EnsureInstance(); //just to be sure.

            // wait for the darkness to envelope you, and then a bit longer
            yield return transitionWait;

            // if an error occurs in an event handler, the scene can't open 
            // the curtains and blacks out.
            try
            {
                //last call before a scene is destroyed.
                GlobalSignals.Get<ScenePreUnloadSignal>().Dispatch();
            }
            catch (Exception ex)
            {
                Debug.LogException(ex, this);
            }

            // perform scene load SceneManager.LoadSceneAsync(index);
            yield return LoadSceneAction(); //many ways to load a scene, but use this one.

            // wait
            yield return new WaitUntilEvent(GlobalSignals.Get<SceneLoadedSignal>()); //levelHasLoaded = true

            // clean up while screen still black
            GC.Collect(); //why not? screen is totally black now.

            yield return null;

            // fade into new scene
            transitioner.TriggerTransition(inTransitionMessage);
            transitionINClip.Clip.PlayOneShot();

            // wait for scene to fully open, and just a bit longer
            yield return transitionWait;
            yield return CommonYieldInstructions.WaitForEndOfFrame;

            // finalize
            IsTransitioning = false;
            Destroy(gameObject);//left over from previous level
        }

        #region Static Interface

        [Button("LoadLevelImmediately"), DisableInEditorMode]
        public static void LoadLevel_(int levelIndex)
		{
            SceneManager.LoadScene(levelIndex);
		}

        [Button, DisableInEditorMode]
        public static void ReloadCurrentLevel()
            => SceneManager.LoadScene(
                SceneManager.GetActiveScene().buildIndex);

        #endregion
    }
}
