using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

public class InputListener : RichMonoBehaviour
{
    public enum ButtonInteractType
    {
        Down,
        Up,
        Hold
    }

    [Header("---Settings---")]
    [SerializeField]
    private bool useKeyCode = false;

    [ShowIf("useKeyCode")]
    [SerializeField]
    private KeyCode keycode = KeyCode.Space;

    [SerializeField]
    private bool useButton = true;

    [ShowIf("useButton")]
    [InputAxis]
    [SerializeField]
    private string button = "Fire1";

    [SerializeField]
    private ButtonInteractType buttonType = ButtonInteractType.Down;

    [SerializeField]
    private UnityEvent action = new UnityEvent();

    protected void Update()
    {
        ReactToButtonPress();
    }

    [Button("Force()", EButtonEnableMode.Playmode)]
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
