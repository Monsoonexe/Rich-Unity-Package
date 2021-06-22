using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class DelayEvent : RichMonoBehaviour
{
    public enum MessageTrigger
    {
        None = 0,
        Awake,
        Start
    }

    public MessageTrigger messageTrigger = MessageTrigger.None;
    [SerializeField] private float delaySeconds = 1f;
    [SerializeField] private UnityEvent uEvent = new UnityEvent();

    protected override void Awake()
    {
        if(messageTrigger = MessageTrigger.Awake)
            CallEvent();
    }

    private void Start()
    {
        if(messageTrigger = MessageTrigger.Start)
            CallEvent();        
    }

    public void CallEvent()
    {
        StartCoroutine(Delay());
    }

    public void CancelEvent()
    {
        StopAllCoroutines();
    }

    private IEnumerator Delay()
    {
        yield return new WaitForSeconds(delaySeconds);
        uEvent.Invoke();
    }
}
