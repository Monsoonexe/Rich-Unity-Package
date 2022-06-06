using UnityEngine;
using ScriptableObjectArchitecture;
using RichPackage.GuardClauses;

namespace RichPackage.YieldInstructions
{
    /// <summary>
    /// Waits until the GameEvent has been raised or an optional timeout.
    /// </summary>
    /// <remarks>Note: Instances should be <see cref="Reset"/> if reusing
    /// and cannot be used synchronously like 
    /// <see cref="CommonYieldInstructions"/>.</remarks>
    /// <seealso cref="WaitWhile"/>
    /// <seealso cref="WaitUntil"/>
    public class WaitUntilGameEvent : CustomYieldInstruction
    {
        public const int INFINITE_TIMEOUT = -1;
        private GameEvent gameEvent;

        /// <summary>
        /// Flag for when <see cref="gameEvent"/> has been raised.
        /// Held <see langword="true"/> until the event is fired, 
        /// then is set <see langword="false"/>.
        /// </summary>
        public bool Flag { get; private set; }
        private float endTime;
        private float timeout;

        public override bool keepWaiting { get => Flag  &&  endTime > Time.time; }

        #region Constructors

        public WaitUntilGameEvent(GameEvent gameEvent)
            : this (gameEvent, INFINITE_TIMEOUT)
        {
            //nada
        }

        /// <param name="timeout">value &lt; 0 implies infinite timeout.</param>
        public WaitUntilGameEvent(GameEvent gameEvent, float timeout)
        {
            //validate
            GuardAgainst.ArgumentIsNull(gameEvent, nameof(gameEvent));

            //assign
            this.gameEvent = gameEvent;
            this.timeout = timeout;

            //configure
            Reset();
        }

        #endregion Constructors

        private void OnEventRaised()
        {
            Flag = false;
            gameEvent.RemoveListener(OnEventRaised);
        }

        public new void Reset()
        {
            gameEvent.RemoveListener(OnEventRaised); //ensure no duplicate subscriptions
            gameEvent.AddListener(OnEventRaised);
            endTime = timeout < 0 ? INFINITE_TIMEOUT : Time.time + timeout;
            Flag = true;
        }

        /// <summary>
        /// Only need to call this if the Coroutine was interrupted to ensure that 
        /// the events is unsubscribed.
        /// </summary>
        public void Close()
        {
            gameEvent.RemoveListener(OnEventRaised);
        }
    }
}
