using RichPackage.Events.Signals;
using ScriptableObjectArchitecture;
using System;

namespace RichPackage.Events
{
    /// <summary>
    /// Raised when the level has finished loading and can be initialized.
    /// </summary>
    /// <see cref="OnLevelReadySignal"/>
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

        public void Dispatch() => Dispatch(null);

        public void Dispatch(SceneVariable newScene)
        {
            OnSceneLoadedData?.Invoke(newScene);
            OnSceneLoaded?.Invoke();
        }
    }
}
