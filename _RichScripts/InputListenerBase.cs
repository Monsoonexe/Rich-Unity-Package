using UnityEngine;
using UnityEngine.Events;

namespace ProjectEmpiresEdge
{
    public enum ButtonInteractType
    {
        Down,
        Up,
        Hold
    }

    public class InputListenerBase : RichMonoBehaviour
    {
        [SerializeField]
        private KeyCode keycode;

        [SerializeField]
        private ButtonInteractType buttonType;

        [SerializeField]
        private UnityEvent action = new UnityEvent();

        protected virtual void Update()
        {
            ReactToButtonPress();
        }

        public void PerformAction()
        {
            action.Invoke();
        }

        protected virtual void ReactToButtonPress()
        {
            //listen for button
            switch (buttonType)
            {
                case ButtonInteractType.Down:
                    if (Input.GetKeyDown(keycode))
                    {
                        PerformAction();
                    }
                    break;
                case ButtonInteractType.Up:
                    if (Input.GetKeyDown(keycode))
                    {
                        PerformAction();
                    }
                    break;
                case ButtonInteractType.Hold:
                    if (Input.GetKeyDown(keycode))
                    {
                        PerformAction();
                    }
                    break;
            }
        }
    }
}
