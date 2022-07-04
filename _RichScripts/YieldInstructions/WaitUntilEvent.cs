/* TODO - support timeout and/or cancellation
 * 
 */

using System;
using UnityEngine;
using ScriptableObjectArchitecture;
using RichPackage.Events.Signals;
using UnityEngine.Events;

namespace RichPackage.YieldInstructions
{
    public class WaitUntilEvent : CustomYieldInstruction
    {
		#region Constants

		//public const int INFINITE_TIMEOUT = -1;

        #endregion Constants

        /// <summary>
        /// Flag for when the event has been raised.
        /// Held <see langword="false"/> until the event is fired, 
        /// then is set <see langword="true"/>.
        /// </summary>
        public bool EventRaised { get; private set; } = false;
        public override bool keepWaiting { get => !EventRaised; }
        private readonly Action response;

        #region Constructors

        /// <summary>
        /// <see langword="yield"/>s until the <paramref name="_event"/> is fired.
        /// </summary>
        public WaitUntilEvent(Action _event)
        {
            response = () =>
            {
                EventRaised = true;
                _event -= response;
            };

            _event += response;
        }

        /// <summary>
        /// <see langword="yield"/>s until the <paramref name="_event"/> is fired.
        /// </summary>
        public WaitUntilEvent(GameEvent _event)
        {
            response = () =>
            {
                EventRaised = true;
                _event.RemoveListener(response);
            };

            _event.AddListener(response);
        }

        /// <summary>
        /// <see langword="yield"/>s until the <paramref name="_event"/> is fired.
        /// </summary>
        public WaitUntilEvent(ASignal _event)
        {
            response = () =>
            {
                EventRaised = true;
                _event.RemoveListener(response);
            };

            _event.AddListener(response);
        }

        /// <summary>
        /// <see langword="yield"/>s until the <paramref name="_event"/> is fired.
        /// </summary>
        public WaitUntilEvent(UnityEvent _event)
        {
            void Response()
            {
                EventRaised = true;
                _event.RemoveListener(Response);
            }
            _event.AddListener(Response);
            response = Response; // because Action != UnityAction
        }

		#endregion Constructors

        public void Cancel()
		{
            // guard against cancelling after already expired.
            if (!EventRaised)
            {
                response();
                EventRaised = false; // was cancelled, not raised.
            }
		}
    }
}
