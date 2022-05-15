//from Unity documentation: https://docs.unity3d.com/ScriptReference/CustomYieldInstruction.html

using UnityEngine;
using System;

namespace RichPackage.YieldInstructions
{
    /// <summary>
    /// Wait until the predicate returns true.
    /// yield return new WaitUntil(() => Princess.isInCastle);
    /// </summary>
    public class WaitUntil : CustomYieldInstruction
    {
        private Func<bool> predicate;

        public override bool keepWaiting { get => !predicate(); }

        /// <summary>
        /// To keep coroutine suspended, return true. 
        /// To let coroutine proceed with execution, return false.
        /// </summary>
        public WaitUntil(Func<bool> predicate)
        {
            this.predicate = predicate;
        }
    }
}
