using RichPackage.Events.Signals;
using ScriptableObjectArchitecture;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace RichPackage.LevelManagement
{
    /// <summary>
    /// I control the events and behaviour of this specifc level.
    /// </summary>
    public class LevelController : RichMonoBehaviour
    {
        [SerializeField, Required, Tooltip("The scene data that describes this scene.")]
        private SceneVariable sceneData;
        public SceneVariable SceneData => sceneData;

        #region Unity Messages

        protected override void Reset()
        {
            base.Reset();
            SetDevDescription("I control the events and behaviour of this specifc level.");
        }

        protected override void Awake()
        {
            base.Awake();

            // verify dependencies exist
            if (sceneData == null)
            {
                Debug.LogError($"{nameof(sceneData)} cannot be null!", this);
                enabled = false;
            }
        }

        /// <remarks>Inheritors must call base.Start().</remarks>
        protected virtual void Start()
        {
            GlobalSignals.Get<OnLevelLoadedSignal>().Dispatch(sceneData);
            InvokeAfterDelay(DispatchLateSceneLoadedSignal,
                YieldInstructions.CommonYieldInstructions.WaitForEndOfFrame);
        }

        #endregion Unity Messages

        private void DispatchLateSceneLoadedSignal()
        {
            GlobalSignals.Get<OnLateLevelLoadedSignal>().Dispatch(sceneData);
        }

        [Button, DisableInEditorMode]
        public void LoadLevel(SceneVariable sceneVariable)
            => SceneManager.LoadScene(sceneVariable.Value.SceneIndex);

        [Button, DisableInEditorMode]
        public void LoadLevel(string name)
            => SceneManager.LoadScene(name);

        [Button, DisableInEditorMode]
        public void LoadLevel(int index)
            => SceneManager.LoadScene(index);

        #region Static Interface

        [Button("LoadLevelImmediately"), DisableInEditorMode]//, ConsoleCommand("loadLevel")]
        public static void LoadLevel_(int levelIndex)
        {
            SceneManager.LoadScene(levelIndex);
        }

        [Button, DisableInEditorMode]//, ConsoleCommand("loadNextLevel")]
        public static void LoadNextLevel()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex + 1);
        }

        [Button, DisableInEditorMode]//, ConsoleCommand("loadPreviousLevel")]
        public static void LoadPreviousLevel()
        {
            int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex - 1);
        }

        [Button, DisableInEditorMode]//, ConsoleCommand("reloadLevel")]
        public static void ReloadCurrentLevel()
            => SceneManager.LoadScene(
                SceneManager.GetActiveScene().buildIndex);

        #endregion
    }
}
