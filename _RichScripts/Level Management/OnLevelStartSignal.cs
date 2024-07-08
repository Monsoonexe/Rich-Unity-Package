using RichPackage.Events.Signals;
using ScriptableObjectArchitecture;
using System;

namespace RichPackage.Events
{
    /// <summary>
    /// Called at the start of the level, after loading.
    /// </summary>
    /// <seealso cref="OnLevelLoadedSignal"/>
    public sealed class OnLevelStartSignal : ABaseSignal
    {
        private readonly EventHandlerList onLateSceneLoaded = new();
        private readonly EventHandlerList<SceneVariable> onLateSceneLoadedData = new();

        public void AddListener(Action action) => onLateSceneLoaded.Add(action);

        public void RemoveListener(Action action) => onLateSceneLoaded.Remove(action);

        public void AddListener(Action<SceneVariable> action) => onLateSceneLoadedData.Add(action);

        public void RemoveListener(Action<SceneVariable> action) => onLateSceneLoadedData.Remove(action);

        public void Dispatch() => Dispatch(null);

        public void Dispatch(SceneVariable newScene)
        {
            onLateSceneLoaded?.Invoke();
            onLateSceneLoadedData?.Invoke(newScene);
        }
    }
}
