using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

/// <summary>
/// Base class for Trigger Volume behaviour. 
/// Fires an event on Enter/Exit when tag matches
/// </summary>
public abstract class ATriggerVolume : RichMonoBehaviour
{
    [SerializeField]
    [Tag]
    protected string reactToTag = "Player";

    /// <summary>
    /// Running total of times this has been triggered.
    /// </summary>
    [Tooltip("Running total of times this has been triggered.")]
    [ReadOnly]
    public int triggerCount = 0;

    [Foldout("---Events---")]
    [SerializeField]
    protected UnityEvent enterEvent = new UnityEvent();

    [Foldout("---Events---")]
    [SerializeField]
    protected UnityEvent exitEvent = new UnityEvent();

    [Button(null, EButtonEnableMode.Playmode)]
    public void ForceCollide()
    {
        ++triggerCount;
        enterEvent.Invoke();
    }

    protected void HandleEnterCollision(GameObject other)
    {
        if (string.IsNullOrEmpty(reactToTag) || other.CompareTag(reactToTag))
        {
            ++triggerCount;
            enterEvent.Invoke();
        }
    }

    protected void HandleExitCollision(GameObject other)
    {
        if (string.IsNullOrEmpty(reactToTag) || other.CompareTag(reactToTag))
        {
            exitEvent.Invoke();
        }
    }
}
