using QFSW.QC;
using RichPackage.Administration;

namespace RichPackage.ConsoleCommands.Administration
{
    /// <summary>
    /// Holds Qc commands for admin.
    /// </summary>
    [AdminCommand, CommandPrefix("admin-")]
    public static class AdminConsoleCommands
    {
		[Command("is")]
        public static bool IsAdmin()=> Admin.IsAdmin;
    }
}
