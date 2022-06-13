using UnityEngine;
using Sirenix.OdinInspector;

namespace RichPackage.UI
{
    /// <summary>
    /// Common base class for UI elements. Comes with some a handy Hide/Show interface.
    /// </summary>
    /// <seealso cref="Editor.RichUIElement_Inspector"/>
    /// <seealso cref="RichUIButton"/>
    [SelectionBase]
    public class RichUIElement : RichMonoBehaviour
    {
        public RectTransform rectTransform { get => (RectTransform)myTransform; }

        protected virtual void OnEnable()
        {
            UpdateUI();
        }

        /// <summary>
        /// Definitely override this with custom implmentation (canvas.enabled recommended). 
        /// No need for base.ToggleVisuals().
        /// </summary>
        /// <param name="active"></param>
        public virtual void ToggleVisuals(bool active)
        {
            //do show if not showing and should be -- and stop showing if are and should not be
            if (gameObject.activeSelf != active)//ignore if already in requested state
                gameObject.SetActive(active);
        }

        /// <summary>
        /// Show if hidden, hide if shown.
        /// </summary>
        [Button, HorizontalGroup("A")]
        public virtual void ToggleVisuals() => ToggleVisuals(!gameObject.activeSelf);

        [Button, HorizontalGroup("A")]
        public void Show() => ToggleVisuals(true);

        [Button, HorizontalGroup("A")]
        public void Hide() => ToggleVisuals(false);

        /// <summary>
        /// Used to refresh UI with current values.
        /// </summary>
        public virtual void UpdateUI()
        {
            //nada
        }
    }

    public abstract class RichUIElement<T> : RichUIElement
    {
        /// <summary>
        /// The data that this UI Element should show.
        /// </summary>
        [Required]
        public T targetData;

        protected override void OnEnable()
        {
            SubscribeToEvents();
            UpdateUI();
        }

        protected virtual void OnDisable()
        {
            UnsubscribeFromEvents();
        }

        protected abstract void SubscribeToEvents();

        protected abstract void UnsubscribeFromEvents();
    }
}
