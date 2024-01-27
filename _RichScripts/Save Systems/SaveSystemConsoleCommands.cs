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

		[Command("save")]
		public static void Save_()
		{
			if (SaveSystem.Instance)
				SaveSystem.Instance.SaveGame();
			else
				Debug.LogWarning("No SaveSystem in Scene.");
		}

		[Command("load")]
		public static void Load_()
		{
			if (SaveSystem.Instance)
				SaveSystem.Instance.LoadGame();
			else
				Debug.LogWarning("No SaveSystem in Scene.");
		}

		[Command("loadSlot")]
		public static void Load_(string name) // TODO - suggest save file names
		{
			if (SaveSystem.Instance)
				SaveSystem.Instance.LoadFile(name);
			else
				Debug.LogWarning("No SaveSystem in Scene.");
		}

		[Command("deleteSave")]
		public static void DeleteSave_()
		{
			if (SaveSystem.Instance)
				SaveSystem.Instance.DeleteFile();
			else
				Debug.LogWarning("No SaveSystem in Scene.");
		}

		[Command("deleteSaveSlot")]
		public static void DeleteSave_(string name)
		{
			if (SaveSystem.Instance)
				SaveSystem.Instance.DeleteFile(name); // TODO - suggest save file names
            else
				Debug.LogWarning("No SaveSystem in Scene.");
		}

		#endregion Console Commands
	}
}
