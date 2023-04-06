
namespace RichPackage.SaveSystem
{
	/// <summary>
	/// Allows for saving/loading from an <see cref="ES3File"/>.
	/// </summary>
	/// <seealso cref="ASaveableMonoBehaviour"/>
	public interface ISaveable
	{
		UniqueID SaveID { get; }

		void SaveState(ISaveSystem saveFile);

		void LoadState(ISaveSystem saveFile);
	}
}
