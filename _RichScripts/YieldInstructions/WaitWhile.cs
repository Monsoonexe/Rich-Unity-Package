//from Unity documentation: https://docs.unity3d.com/ScriptReference/CustomYieldInstruction.html

using UnityEngine;
using System;

namespace RichPackage.YieldInstructions
{
    /// <summary>
    /// Wait until the predicate returns false.
    /// yield return new WaitWhile(() => Princess.isInCastle);
    /// </summary>
    public class WaitWhile : CustomYieldInstruction
    {
        private Func<bool> predicate;

        public override bool keepWaiting { get => predicate(); }

        /// <summary>
        /// To keep coroutine suspended, return true. 
        /// To let coroutine proceed with execution, return false.
        /// </summary>
        public WaitWhile(Func<bool> predicate)
        {
            this.predicate = predicate;
        }
    }
}
