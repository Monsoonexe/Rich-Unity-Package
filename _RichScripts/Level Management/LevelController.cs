using UnityEngine.SceneManagement;
using ScriptableObjectArchitecture;
using Sirenix.OdinInspector;
using UnityEngine;
using RichPackage.Events.Signals;

namespace RichPackage.LevelManagement
{
    /// <summary>
    /// I control the events and behaviour of this specifc level.
    /// </summary>
    public class LevelController : RichMonoBehaviour
    {
        [SerializeField, Required, Tooltip("The scene data that describes this scene.")]
        private SceneVariable sceneVariable;
        #region Unity Messages

        protected override void Reset()
        {
            base.Reset();
            SetDevDescription("I control the events and behaviour of this specifc level.");
        }
        
        /// <remarks>Inheritors must call base.Start().</remarks>
        protected virtual void Start()
        {
            GlobalSignals.Get<OnLevelLoadedSignal>().Dispatch(sceneVariable);
            InvokeAfterDelay(DispatchLateSceneLoadedSignal,
                YieldInstructions.CommonYieldInstructions.WaitForEndOfFrame);
        }

        #endregion Unity Messages

        private void DispatchLateSceneLoadedSignal()
        {
            GlobalSignals.Get<OnLateLevelLoadedSignal>().Dispatch(sceneVariable);
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
            var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex + 1);
        }

        [Button, DisableInEditorMode]//, ConsoleCommand("loadPreviousLevel")]
        public static void LoadPreviousLevel()
        {
            var currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
            SceneManager.LoadScene(currentSceneIndex - 1);
        }

        [Button, DisableInEditorMode]//, ConsoleCommand("reloadLevel")]
        public static void ReloadCurrentLevel()
            => SceneManager.LoadScene(
                SceneManager.GetActiveScene().buildIndex);

        #endregion
    }
}
