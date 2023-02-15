using UnityEngine;
using UnityEngine.SceneManagement;

namespace RichPackage.Tweening
{
    /// <summary>
    /// I am a utility class to hold tweens so you don't have to!
    /// </summary>
    public class TweenHolder : RichMonoBehaviour
    {
        protected override void Reset()
        {
            base.Reset();
            SetDevDescription("I am a utility class to hold tweens" +
                " so you don't have to!");
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
