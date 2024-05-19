namespace RichPackage.SaveSystem
{
	/// <summary>
	/// Allows for saving/loading from an <see cref="ES3File"/>.
	/// </summary>
	/// <seealso cref="ASaveableMonoBehaviour"/>
	public interface ISaveable
	{
		void SaveState(ISaveStore saveFile);

		void LoadState(ISaveStore saveFile);

		void DeleteState(ISaveStore saveFile);
	}
}
