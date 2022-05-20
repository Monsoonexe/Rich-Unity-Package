using UnityEngine;
using UnityEngine.Events;
using Sirenix.OdinInspector;

namespace RichPackage
{
    public class InputListener : RichMonoBehaviour
    {
        public enum ButtonInteractType
        {
            Down,
            Up,
            Hold
        }

        [Title("Settings")]
        [SerializeField]
        public bool useKeyCode = true;

        [ShowIf("useKeyCode")]
        [SerializeField]
        public KeyCode keycode = KeyCode.Space;

        [SerializeField]
        public bool useButton = false;

        [ShowIf("useButton")]
        [NaughtyAttributes.InputAxis]
        [SerializeField]
        public string button = "Fire1";

        [SerializeField]
        public ButtonInteractType buttonType = ButtonInteractType.Down;

        [SerializeField, FoldoutGroup("Events")]
        private UnityEvent action = new UnityEvent();

        protected void Update()
        {
            ReactToButtonPress();
        }

        [Button("Force()", DisableInEditorMode)]
        public void PerformAction()
        {
            action.Invoke();
        }

        protected void ReactToButtonPress()
        {
            //listen for button
            switch (buttonType)
            {
                case ButtonInteractType.Down:
                    if (useKeyCode && Input.GetKeyDown(keycode)
                        || useButton && Input.GetButtonDown(button))
                        PerformAction();
                    break;
                case ButtonInteractType.Up:
                    if (useKeyCode && Input.GetKeyUp(keycode)
                        || useButton && Input.GetButtonUp(button))
                        PerformAction();
                    break;
                case ButtonInteractType.Hold:
                    if (useKeyCode && Input.GetKey(keycode)
                        || useButton && Input.GetButton(button))
                        PerformAction();
                    break;
            }
        }
    }
}
