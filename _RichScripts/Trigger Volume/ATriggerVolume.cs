using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// 
/// </summary>
public abstract class ATriggerVolume : RichMonoBehaviour
{
    [SerializeField]
    protected string reactToTag = "Player";

    [SerializeField]
    protected UnityEvent enterEvent = new UnityEvent();

    [SerializeField]
    protected UnityEvent exitEvent = new UnityEvent();

}
