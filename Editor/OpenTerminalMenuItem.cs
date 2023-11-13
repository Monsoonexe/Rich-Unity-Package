using System.Diagnostics;
using UnityEditor;

namespace RichPackage.Editor
{
	/// <summary>
	/// Holds menu items to start terminals like bash, cmd, or powershell.
	/// </summary>
	public static class OpenTerminalMenuItem
	{
		private const string TerminalMenu = RichEditorUtility.MenuName + "Terminals";

		[MenuItem(TerminalMenu + "/Powershell %`")] //%` means you can press ctrl+` to call this function
		public static void OpenPowershell()
		{
			var process = Process.Start("powershell");
			process.EnableRaisingEvents = true;
			process.Exited += (obj, ctx) => ((Process)obj).Dispose();
		}

		[MenuItem(TerminalMenu + "/Command Prompt")]
		public static void OpenCommandPrompt()
		{
			var process = Process.Start("cmd");
			process.EnableRaisingEvents = true;
			process.Exited += (obj, ctx) => ((Process)obj).Dispose();
		}

		[MenuItem(TerminalMenu + "/Bash")]
		public static void OpenBash()
		{
			var process = Process.Start(@"D:\3rdPartyUtility\Git\git-bash.exe");//, "--cd-to-home");
			process.EnableRaisingEvents = true;
			process.Exited += (obj, ctx) => ((Process)obj).Dispose();
		}
	}
}
