using System;
using UnityEngine;
using QFSW.QC;

namespace RichPackage.ConsoleCommands
{
	/// <summary>
	/// 
	/// </summary>
	[CommandPrefix("GC.")]
	public static class GCCommands
	{
		[Command, CommandDescription("Invokes the C# garbage collector and prints runtime.")]
		public static void Collect()
		{
			Debug.Log("Invoking C# runtime GC...");
			var startTime = DateTime.Now;
			GC.Collect();
			var runtime = DateTime.Now - startTime;
			Debug.Log($"Done! ({runtime.TotalMilliseconds}ms)");
		}

		[Command, CommandDescription("Allocates a `new byte[size] * count`.")]
		public static void Allocate(int size, int count = 1)
		{
			for(int i = 0; i < count; i++)
			{
				var bytes = new byte[size];
			}
		}
	}
}
