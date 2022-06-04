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
        static AdminScanRule()
		{
            Admin.OnLogout += RegenerateCommandTable;
            Admin.OnLogin += RegenerateCommandTable;
		}

        private static void RegenerateCommandTable()
            => QuantumConsoleProcessor.GenerateCommandTable(
                deployThread: true, forceReload: true);

        public ScanRuleResult ShouldScan<T>(T entity) where T : ICustomAttributeProvider
        {
            //admin is always enabled in editor
            //if admin, suggest to scan everything
            if (!Admin.IsAdmin)
			{
                //filter
                return HasAdminAttribute(entity)
                    ? ScanRuleResult.Reject
                    : ScanRuleResult.Accept;
            }
            
            return ScanRuleResult.Accept;
        }

        private bool HasAdminAttribute(ICustomAttributeProvider provider)
            => provider.GetCustomAttributes(inherit: true)
                .Any((att) => att is AdminCommandAttribute);
    }
}
