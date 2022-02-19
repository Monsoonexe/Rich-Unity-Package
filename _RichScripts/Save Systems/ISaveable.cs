
namespace RichPackage.SaveSystem
{
	/// <summary>
	/// Allows for saving/loading from an <see cref="ES3File"/>.
	/// </summary>
	/// <seealso cref="ASaveableMonoBehaviour"/>
	public interface ISaveable
	{
		string SaveID { get; }

		void SaveState(ES3File saveFile);

		void LoadState(ES3File saveFile);

		void DeleteState(ES3File saveFile);
	}
}
