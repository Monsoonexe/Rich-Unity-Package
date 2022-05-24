using UnityEngine;
using QFSW.QC;
using RichPackage.Events.Signals;

[CommandPrefix("signals.")]
public static class SignalsCommands
{
	[Command(aliasOverride:"dispatch"),
		CommandDescription("Invokes the given signal registered with GlobalSignals.")]
	public static void Dispatch(string signalHash)
	{
		if (GlobalSignals.Get(signalHash) is ASignal sig)
		{
			Debug.Log("Dispatching " + signalHash);
			sig.Dispatch();
		}
		else
		{
			string fullyQualifiedName = nameof(RichPackage) + "." + signalHash;

			if (GlobalSignals.Get(fullyQualifiedName) is ASignal sign)
			{
				Debug.Log("Dispatching " + fullyQualifiedName);
				sign.Dispatch();
			}
			else
			{
				Debug.Log(signalHash + " is not supported or found.");
			}
		}
	}
}
