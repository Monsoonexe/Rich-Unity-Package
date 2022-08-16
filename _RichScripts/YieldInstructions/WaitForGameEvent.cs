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
	[System.Obsolete("Use " + nameof(WaitUntilEvent) + " instead.")]
    public class WaitForGameEvent : CustomYieldInstruction
    {
        public const int INFINITE_TIMEOUT = -1;

        /// <summary>
        /// Flag for when <see cref="gameEvent"/> has been raised.
        /// Held <see langword="false"/> until the event is fired, 
        /// then is set <see langword="true"/>.
        /// </summary>
        private bool eventRaised;

        /// <summary>
        /// Flag for when <see cref="gameEvent"/> has been raised.
        /// Held <see langword="false"/> until the event is fired, 
        /// then is set <see langword="true"/>.
        /// </summary>
        public bool EventRaised { get => eventRaised; }
        private float endTime;
        private readonly float timeout;
        private readonly GameEvent gameEvent;

        #region Constructors

        /// <summary>
        /// Wait until the event is fired or timeout.
        /// </summary>
        public override bool keepWaiting { get => !(eventRaised || endTime > Time.time); }

        public WaitForGameEvent(GameEvent gameEvent)
            :this (gameEvent, INFINITE_TIMEOUT)
        {
            //nada
        }

        /// <param name="timeout">value &lt; 0 implies infinite timeout.</param>
        public WaitForGameEvent(GameEvent gameEvent, float timeout)
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
            eventRaised = true;
            gameEvent.RemoveListener(OnEventRaised);
        }

        public new void Reset()
        {
            gameEvent.RemoveListener(OnEventRaised); //ensure no duplicate subscriptions
            gameEvent.AddListener(OnEventRaised);
            endTime = timeout < 0 ? INFINITE_TIMEOUT : Time.time + timeout;
            eventRaised = false;
        }

        /// <summary>
        /// Only need to call this if the Coroutine was interrupted to ensure that 
        /// the event is unsubscribed.
        /// </summary>
        public void Close()
        {
            gameEvent.RemoveListener(OnEventRaised);
        }
    }
}
