using System;
using UnityEngine;
using QFSW.QC;

namespace RichPackage.ConsoleCommands
{
	[CommandPrefix("gc.")]
	public static class GCCommands
	{
		[Command, CommandDescription("Invokes the C# garbage collector and prints runtime.")]
		public static void Collect()
		{
			Debug.Log("Invoking C# runtime GC...");
			var startTime = DateTime.Now;
			long heapSizeStart = GC.GetTotalMemory(forceFullCollection: false); 
			GC.Collect();
			var runtime = DateTime.Now - startTime;
			long heapSizeEnd = GC.GetTotalMemory(forceFullCollection: false);
			Debug.Log("Done!\r\n"+
				$"Time: {runtime.TotalMilliseconds}ms before: {heapSizeStart:n0}b after: {heapSizeEnd:n0}b diff: {heapSizeStart - heapSizeEnd:n0}b");
		}

		[Command, CommandDescription("Allocates memory")]
		public static void Allocate(long bytes)
		{
			GC.AddMemoryPressure(bytes);
		}

		[Command, Command(aliasOverride:"get-mem"), CommandDescription("Retrieves the number of bytes currently thought to be allocated.")]
		public static long GetTotalMemory(bool forceFullCollection = false )
		{
			return GC.GetTotalMemory(forceFullCollection);
		}

		public static void StartNoGCRegion()
		{
			//Debug.Log(GC.TryStartNoGCRegion())
		}

		public static void EndNoGCRegion()
		{

		}
	}
}
