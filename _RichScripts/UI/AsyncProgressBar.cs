using System.Collections;
using UnityEngine;

namespace RichPackage.UI
{
    /// <summary>
    /// Untested. Use SceneLoaded event
    /// </summary>
    public class AsyncProgressBar : MonoBehaviour
    {
        public RectTransform Bar;
        
        public void SetAsyncOperation(AsyncOperation operation)
        {
            StartCoroutine(UpdateProgessBar(operation));
        }

        private IEnumerator UpdateProgessBar(AsyncOperation operation)
        {
            while (!operation.isDone)
            {
                Bar.anchorMax = new Vector2(operation.progress, Bar.anchorMax.y);
                yield return null;
            }
        }
    }
}
