using System.Threading;
using UnityEngine;

namespace RichPackage
{
#if UNITY_EDITOR
    [UnityEditor.InitializeOnLoad]
#endif
    public static class ThreadingUtility
    {
        private static CancellationTokenSource quitCTS;

        public static CancellationToken QuitToken { get => quitCTS.Token; }

        public static SynchronizationContext UnityContext { get; private set; }

        static ThreadingUtility()
        {
            quitCTS = new CancellationTokenSource();
        }

#if UNITY_EDITOR
        [UnityEditor.InitializeOnLoadMethod]
#endif
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void MainThreadInitialize()
        {
            UnityContext = SynchronizationContext.Current;

#if UNITY_EDITOR
            UnityEditor.EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
#else
            Application.quitting += quitSource.Cancel;
#endif
        }

#if UNITY_EDITOR

        private static void OnPlayModeStateChanged(UnityEditor.PlayModeStateChange state)
        {
            if (state == UnityEditor.PlayModeStateChange.ExitingPlayMode)
            {
                quitCTS.Cancel();
                quitCTS.Dispose();
            }
            else if (state == UnityEditor.PlayModeStateChange.EnteredPlayMode)
            {
                // ensure new in case domain not reloaded
                quitCTS = new CancellationTokenSource();
            }
        }
#endif
    }
}
