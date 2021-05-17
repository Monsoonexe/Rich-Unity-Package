using UnityEngine;
using UnityEngine.Events;

namespace ColdironTools.Events
{
    public class EventUtility : MonoBehaviour
    {
        #region Fields
        [SerializeField] private UnityEvent enableEvent = new UnityEvent();
        [SerializeField] private UnityEvent disableEvent = new UnityEvent();
        [SerializeField] private UnityEvent startEvent = new UnityEvent();
        [SerializeField] private UnityEvent destroyEvent = new UnityEvent();
        #endregion

        #region Methods
        void OnEnable()
        {
            enableEvent.Invoke();
        }

        private void OnDisable()
        {
            disableEvent.Invoke();
        }

        void Start()
        {
            startEvent.Invoke();
        }

        void OnDestroy()
        {
            destroyEvent.Invoke();
        }
        #endregion
    }
}