using UnityEngine;
using UnityEngine.SceneManagement;

namespace RichPackage.Tweening
{
    /// <summary>
    /// I am a utility class to hold tweens so you don't have to!
    /// </summary>
    /// <seealso cref="ApexTweens"/>
    public class TweenHolder : RichMonoBehaviour
    {
        protected override void Reset()
        {
            base.Reset();
            SetDevDescription("I am a utility class to hold tweens" +
                " so you don't have to!");
        }

        private void Start()
        {
            SceneManager.sceneLoaded += OnLevelLoadedHandler;
        }

        private void OnDestroy()
        {
            SceneManager.sceneLoaded -= OnLevelLoadedHandler;
        }

		[System.Obsolete("This always kills all routines. Maybe some need to persist.")]
        private void OnLevelLoadedHandler(
            Scene scene, LoadSceneMode mode)
        {
            StopAllCoroutines();
        }

        public static TweenHolder Construct()
        {
            var tweenObj = new GameObject("RichTween TweenHolder");
            var tweenHolder = tweenObj.AddComponent<TweenHolder>();
            DontDestroyOnLoad(tweenObj);
            return tweenHolder;
        }
    }
}
