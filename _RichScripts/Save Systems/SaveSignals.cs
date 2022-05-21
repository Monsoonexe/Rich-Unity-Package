using RichPackage.Events.Signals;

namespace RichPackage.SaveSystem.Signals
{
	/// <summary>
	/// Raise a request to have the game save its current state.
	/// </summary>
	/// <seealso cref="SaveStateToFile"/>
	public class SaveGame : ASignal
	{
		//exists
	}

	/// <summary>
	/// The game state needs to be saved to this file.
	/// </summary>
	public class SaveStateToFile : ASignal<ES3File>
    {
        //exists
    }

	/// <summary>
	/// The game's state needs to be loaded from this file.
	/// </summary>
	public class LoadStateFromFile : ASignal<ES3File>
	{
		//exists
	}
}
