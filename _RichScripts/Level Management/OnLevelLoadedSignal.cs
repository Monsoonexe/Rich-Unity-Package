using ScriptableObjectArchitecture;
using System;

namespace RichPackage.Events.Signals
{
    /// <summary>
    /// Dispatched by SceneManager.sceneLoaded when such.
    /// </summary>
    /// <see cref="OnLateLevelLoadedSignal"/>
    public sealed class OnLevelLoadedSignal : ABaseSignal,
        ISignalListener, ISignalListener<SceneVariable>
    {
        private event Action OnSceneLoaded;
        private event Action<SceneVariable> OnSceneLoadedData;

        public void AddListener(Action action)
        {
            OnSceneLoaded += action;
        }

        public void RemoveListener(Action action)
        {
            OnSceneLoaded -= action;
        }

        public void AddListener(Action<SceneVariable> action)
        {
            OnSceneLoadedData += action;
        }

        public void RemoveListener(Action<SceneVariable> action)
        {
            OnSceneLoadedData -= action;
        }

        public void Dispatch(SceneVariable newScene)
        {
            OnSceneLoaded?.Invoke();
            OnSceneLoadedData?.Invoke(newScene);
        }
    }
}
