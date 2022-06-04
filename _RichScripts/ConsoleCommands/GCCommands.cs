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
			long heapSizeStart = GC.GetTotalMemory(forceFullCollection: false); 
			GC.Collect();
			var runtime = DateTime.Now - startTime;
			long heapSizeEnd = GC.GetTotalMemory(forceFullCollection: false);
			Debug.Log("Done!\r\n"+
				$"Time: {runtime}ms before: {heapSizeStart:n0}b after: {heapSizeEnd:n0}b diff: {heapSizeStart - heapSizeEnd:n0}b");
		}

		[Command, CommandDescription("Allocates a `new byte[size] * count`.")]
		public static void Allocate(int size, int count = 1)
		{
			for(int i = 0; i < count; i++)
			{
				var bytes = new byte[size];
			}
		}

		[CommandDescription("Retrieves the number of bytes currently thought to be allocated.")]
		public static void GetTotalMemory(bool forceFullCollection = false )
		{
			Debug.Log(GC.GetTotalMemory(forceFullCollection));
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
