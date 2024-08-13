// https://docs.unity3d.com/Manual/LogFiles.html

using QFSW.QC;
using System.Diagnostics;

namespace ScaryRobot.LostInSpace.Console
{
    [CommandPrefix("Log.")]
    public static class LogConsoleCommands
    {
        [Command,
            CommandPlatform(Platform.WindowsPlayer | Platform.WindowsEditor)]
        private static void OpenEditorLog() => Process.Start("%LOCALAPPDATA%\\Unity\\Editor\\Editor.log");

        [Command,
            CommandPlatform(Platform.WindowsPlayer | Platform.WindowsEditor)]
        private static void OpenPlayerLog() => Process.Start("%USERPROFILE%\\AppData\\LocalLow\\CompanyName\\ProductName\\Player.log");

        [Command,
            CommandPlatform(Platform.WindowsEditor)]
        private static void OpenHubLog() => Process.Start("%USERPROFILE%\\AppData\\Roaming\\UnityHub\\logs\\info-log.json");
    }
}
