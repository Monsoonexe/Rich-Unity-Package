using System;

namespace RichPackage.ConsoleCommands.Administration
{
	/// <summary>
	/// Entities marked with this will be registered with
	/// <see cref="QFSW.QC.QuantumConsole"/>
	/// only if has admin privileges (<see cref="RichPackage.Administration.Admin.IsAdmin"/>).
	/// </summary>
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Struct | AttributeTargets.Method,
		AllowMultiple = false)]
	public sealed class AdminCommandAttribute : Attribute
	{
		// exists
	}
}
