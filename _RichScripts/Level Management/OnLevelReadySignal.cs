using RichPackage.Events.Signals;
using ScriptableObjectArchitecture;
using System;

namespace RichPackage.Events
{
    /// <summary>
    /// Called at the start of the level, after loading.
    /// </summary>
    /// <seealso cref="OnLevelLoadedSignal"/>
    public sealed class OnLevelReadySignal : ABaseSignal
    {
        private event Action OnLateSceneLoaded;
        private event Action<SceneVariable> OnLateSceneLoadedData;

        public void AddListener(Action action)
        {
            OnLateSceneLoaded += action;
        }

        public void RemoveListener(Action action)
        {
            OnLateSceneLoaded -= action;
        }

        public void AddListener(Action<SceneVariable> action)
        {
            OnLateSceneLoadedData += action;
        }

        public void RemoveListener(Action<SceneVariable> action)
        {
            OnLateSceneLoadedData -= action;
        }

        public void Dispatch() => Dispatch(null);

        public void Dispatch(SceneVariable newScene)
        {
            OnLateSceneLoaded?.Invoke();
            OnLateSceneLoadedData?.Invoke(newScene);
        }
    }
}
