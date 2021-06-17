using UnityEngine;
using UnityEngine.Events;

public class EventUtility : RichMonoBehaviour
{
    #region Fields
    [Header("---Events---")]
    [SerializeField] private UnityEvent startEvent = new UnityEvent();
    [SerializeField] private UnityEvent destroyEvent = new UnityEvent();
    [SerializeField] private UnityEvent enableEvent = new UnityEvent();
    [SerializeField] private UnityEvent disableEvent = new UnityEvent();
    #endregion

    #region Methods

    void Start()
    {
        startEvent.Invoke();
    }

    void OnDestroy()
    {
        destroyEvent.Invoke();
    }

    void OnEnable()
    {
        enableEvent.Invoke();
    }

    private void OnDisable()
    {
        disableEvent.Invoke();
    }

    #endregion
}
