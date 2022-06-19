using System;
using UnityEngine;
using ScriptableObjectArchitecture;
using RichPackage.Events.Signals;
using UnityEngine.Events;

namespace RichPackage.YieldInstructions
{
    public class WaitUntilEvent : CustomYieldInstruction
    {
        private bool eventRaised = false;
        public override bool keepWaiting { get => !eventRaised; }

        #region Constructors

        public WaitUntilEvent(Action _event)
        {
            _event += Response;
            void Response()
            {
                eventRaised = true;
                _event -= Response;
            }
        }

        public WaitUntilEvent(GameEvent _event)
        {
            _event.AddListener(Response);
            void Response()
            {
                eventRaised = true;
                _event.RemoveListener(Response);
            }
        }

        public WaitUntilEvent(ASignal _event)
        {
            _event.AddListener(Response);
            void Response()
            {
                eventRaised = true;
                _event.RemoveListener(Response);
            }
        }

        public WaitUntilEvent(UnityEvent _event)
        {
            _event.AddListener(Response);
            void Response()
            {
                eventRaised = true;
                _event.RemoveListener(Response);
            }
        }


		#endregion Constructors
    }
}
