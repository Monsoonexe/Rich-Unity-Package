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

    [Header("---Events---")]
    [SerializeField]
    protected UnityEvent enterEvent = new UnityEvent();

    [SerializeField]
    protected UnityEvent exitEvent = new UnityEvent();

    protected void HandleEnterCollision(GameObject other)
    {
        if (other.CompareTag(reactToTag))
        {
            ++triggerCount;
            enterEvent.Invoke();
        }
    }

    protected void HandleExitCollision(GameObject other)
    {
        if (other.CompareTag(reactToTag))
        {
            ++triggerCount;
            exitEvent.Invoke();
        }
    }
}
