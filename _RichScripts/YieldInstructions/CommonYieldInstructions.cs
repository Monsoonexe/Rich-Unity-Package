using UnityEngine;

namespace RichPackage.YieldInstructions
{
	/// <summary>
	/// Holds cached values for often-used yield instructions.
	/// </summary>
	public static class CommonYieldInstructions
	{
        // frame timing
        public static readonly YieldInstruction WaitForUpdate = null; // lol
		public static readonly YieldInstruction WaitForEndOfFrame = new WaitForEndOfFrame();
		public static readonly YieldInstruction WaitForFixedUpdate = new WaitForFixedUpdate();

        // fractions of seconds
		public static readonly YieldInstruction WaitForTenthSecond = new WaitForSeconds(0.1f);
        public static readonly YieldInstruction WaitForEighthSecond = new WaitForSeconds(1f / 8f);
		public static readonly YieldInstruction WaitForQuarterSecond = new WaitForSeconds(0.25f);
        public static readonly YieldInstruction WaitForThirdSecond = new WaitForSeconds(1f / 3f);
		public static readonly YieldInstruction WaitForHalfSecond = new WaitForSeconds(0.5f);

        // seconds
		public static readonly YieldInstruction WaitForOneSecond = new WaitForSeconds(1);
		public static readonly YieldInstruction WaitForTwoSeconds = new WaitForSeconds(2);
		public static readonly YieldInstruction WaitForThreeSeconds = new WaitForSeconds(3);
        public static readonly YieldInstruction WaitForFourSeconds = new WaitForSeconds(4);
		public static readonly YieldInstruction WaitForFiveSeconds = new WaitForSeconds(5);
		public static readonly YieldInstruction WaitForTenSeconds = new WaitForSeconds(10);

        // minutes
        public static readonly YieldInstruction WaitForOneMinute = new WaitForSeconds(60);
        public static readonly YieldInstruction WaitForTwoMinutes = new WaitForSeconds(120);
    }
}
