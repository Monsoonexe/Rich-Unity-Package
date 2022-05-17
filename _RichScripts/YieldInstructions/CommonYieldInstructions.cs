using UnityEngine;

namespace RichPackage.YieldInstructions
{
	/// <summary>
	/// Holds cached values for often-used yield instructions.
	/// </summary>
	public static class CommonYieldInstructions
	{
		public static readonly YieldInstruction WaitForEndOfFrame = new WaitForEndOfFrame();
		public static readonly YieldInstruction WaitForFixedUpdate = new WaitForFixedUpdate();

		public static readonly YieldInstruction WaitForTenthSecond = new WaitForSeconds(0.1f);
		public static readonly YieldInstruction WaitForQuarterSecond = new WaitForSeconds(0.25f);
		public static readonly YieldInstruction WaitForHalfSecond = new WaitForSeconds(0.5f);

		public static readonly YieldInstruction WaitForOneSecond = new WaitForSeconds(1);
		public static readonly YieldInstruction WaitForTwoSeconds = new WaitForSeconds(2);
		public static readonly YieldInstruction WaitForThreeSeconds = new WaitForSeconds(3);
		public static readonly YieldInstruction WaitForFiveSeconds = new WaitForSeconds(5);
		public static readonly YieldInstruction WaitForTenSeconds = new WaitForSeconds(10);
	}
}
