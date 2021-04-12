using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Base class for Trigger Volume behaviour. 
/// Fires an event on Enter/Exit when tag matches
/// </summary>
public abstract class ATriggerVolume : RichMonoBehaviour
{
    [SerializeField]
    protected string reactToTag = "Player";

    /// <summary>
    /// Running total of times this has been triggered.
    /// </summary>
    [Tooltip("Running total of times this has been triggered.")]
    public int triggerCount = 0;

    [SerializeField]
    protected UnityEvent enterEvent = new UnityEvent();

    [SerializeField]
    protected UnityEvent exitEvent = new UnityEvent();

    protected void HandleEnterCollision(GameObject other)
    {
        if (other.CompareTag(reactToTag))
        {
            enterEvent.Invoke();
            ++triggerCount;
        }
    }

    protected void HandleExitCollision(GameObject other)
    {
        if (other.CompareTag(reactToTag))
        {
            exitEvent.Invoke();
            ++triggerCount;
        }
    }
}
