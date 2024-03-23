using RichPackage.Events;
using RichPackage.Events.Signals;
using System;

namespace RichPackage.SaveSystem.Signals
{
	/// <summary>
	/// Raise a request to have the entire game save its current state.
	/// </summary>
	/// <seealso cref="SaveStateToFileSignal"/>
	public class SaveGameSignal : ASignal
	{
		// exists
	}

	/// <summary>
	/// The game state needs to be saved to this file.
	/// </summary>
	public class SaveStateToFileSignal : ASignal<ISaveStore>
    {
        // exists
    }

	/// <summary>
	/// The game state needs to be loaded from this file.
	/// </summary>
	public class LoadStateFromFileSignal : ASignal<ISaveStore>
	{
        private readonly EventHandlerList<ISaveStore> lateListeners = new();

        public void AddLateListener(Action<ISaveStore> listener) => lateListeners.Add(listener);
        public void RemoveLateListener(Action<ISaveStore> listener) => lateListeners.Remove(listener);
    
        public new void Dispatch(ISaveStore store)
        {
            base.Dispatch(store);
            lateListeners.Invoke(store);
        }
    }

    /// <summary>
    /// Alert a listener that this object would like its state saved.
    /// </summary>
    public class SaveObjectStateSignal : ASignal<ISaveable>
	{
		// exists
	}
}
