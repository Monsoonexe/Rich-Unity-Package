using QFSW.QC;
using System.Reflection;
using RichPackage.Administration;

namespace RichPackage.ConsoleCommands.Administration
{
    /// <summary>
    /// Scans for protected commands.
    /// </summary>
    /// <seealso cref="AdminConsoleCommands"/>
    /// <seealso cref="AdminCommandAttribute"/>
    /// <seealso cref="Admin"/>
    public class AdminScanRule : IQcScanRule
    {
        #region Constructors

        static AdminScanRule()
        {
            Admin.OnLogout += RegenerateCommandTable;
            Admin.OnLogin += RegenerateCommandTable;
        }

        #endregion Constructors

        private static void RegenerateCommandTable()
        {
            QuantumConsoleProcessor.GenerateCommandTable(
                        deployThread: true, forceReload: true);
        }

        public ScanRuleResult ShouldScan<T>(T entity) where T : ICustomAttributeProvider
        {
            // if admin, suggest to scan everything
            if (Admin.IsAdmin)
            {
                // everything
                return ScanRuleResult.Accept;
            }
            else
            {
                // filter
                return HasAdminAttribute(entity)
                    ? ScanRuleResult.Reject
                    : ScanRuleResult.Accept;
            }
        }

        private bool HasAdminAttribute(ICustomAttributeProvider provider)
        {
            // an admin command has the admin command attribute
            return provider.GetCustomAttributes(inherit: true)
                        .Any((att) => att is AdminCommandAttribute);
        }
    }
}
