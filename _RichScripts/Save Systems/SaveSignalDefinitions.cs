using RichPackage.Events.Signals;

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
	public class SaveStateToFileSignal : ASignal<ES3File>
    {
        // exists
    }

	/// <summary>
	/// The game state needs to be loaded from this file.
	/// </summary>
	public class LoadStateFromFileSignal : ASignal<ES3File>
	{
		// exists
	}

	/// <summary>
	/// Alert a listener that this object would like its state saved.
	/// </summary>
	public class SaveObjectStateSignal : ASignal<ISaveable>
	{
		// exists
	}
}
