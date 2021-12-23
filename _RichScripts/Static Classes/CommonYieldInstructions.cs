using UnityEngine;

namespace RichPackage
{
	public static class CommonYieldInstructions
	{
		public static readonly YieldInstruction WaitForEndOfFrame = new WaitForEndOfFrame();
		public static readonly YieldInstruction WaitForFixedUpdate = new WaitForFixedUpdate();

		public static readonly YieldInstruction WaitForTenthSecond = new WaitForSeconds(0.1f);
		public static readonly YieldInstruction WaitForHalfSecond = new WaitForSeconds(0.5f);

		public static readonly YieldInstruction WaitForOneSecond = new WaitForSeconds(1);
		public static readonly YieldInstruction WaitForTwoSeconds = new WaitForSeconds(2);
		public static readonly YieldInstruction WaitForThreeSecond = new WaitForSeconds(3);
	}
}
