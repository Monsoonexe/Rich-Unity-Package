using System.Diagnostics;
using UnityEditor;

namespace RichPackage.Editor
{
	/// <summary>
	/// Holds menu items to start terminals like bash, cmd, or powershell.
	/// </summary>
	public static class OpenTerminalMenuItem
	{
		private const string TerminalMenu = "RichUtilities/Terminals";

		[MenuItem(TerminalMenu + "/Powershell %`")] //%` means you can press ctrl+` to call this function
		public static void OpenPowershell()
		{
			Process.Start("powershell");
		}

		[MenuItem(TerminalMenu + "/Command Prompt")]
		public static void OpenCommandPrompt()
		{
			Process.Start("cmd");
		}

		[MenuItem(TerminalMenu + "/Bash")]
		public static void OpenBash()
		{
			Process.Start(@"D:\3rdPartyUtility\Git\git-bash.exe");//, "--cd-to-home");
		}
	}
}
