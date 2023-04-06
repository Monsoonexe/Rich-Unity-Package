#if ES3
using QFSW.QC;
using UnityEngine;

namespace RichPackage.SaveSystem
{
	/// <summary>
	/// Console commands to drive the <see cref="SaveSystem"/>.
	/// </summary>
	public static class SaveSystemConsoleCommands
	{
		#region Console Commands
		// TODO - move these to its own file

		[Command("save")]
		public static void Save_()
		{
			if (SaveSystem.Instance)
				SaveSystem.Instance.Save();
			else
				Debug.LogWarning("No SaveSystem in Scene.");
		}

		[Command("load")]
		public static void Load_()
		{
			if (SaveSystem.Instance)
				SaveSystem.Instance.Load();
			else
				Debug.LogWarning("No SaveSystem in Scene.");
		}

		[Command("loadSlot")]
		public static void Load_(int slot)
		{
			if (SaveSystem.Instance)
				SaveSystem.Instance.Load(slot);
			else
				Debug.LogWarning("No SaveSystem in Scene.");
		}

		[Command("deleteSave")]
		public static void DeleteSave_()
		{
			if (SaveSystem.Instance)
				SaveSystem.Instance.DeleteSave();
			else
				Debug.LogWarning("No SaveSystem in Scene.");
		}

		[Command("deleteSaveSlot")]
		public static void DeleteSave_(int slot)
		{
			if (SaveSystem.Instance)
				SaveSystem.Instance.DeleteSave(slot);
			else
				Debug.LogWarning("No SaveSystem in Scene.");
		}

		#endregion
	}
}
#endif // ES3