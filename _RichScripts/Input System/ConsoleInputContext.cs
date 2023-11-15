using QFSW.QC;
using UnityEngine;

namespace RichPackage.InputSystem
{
    public sealed class ConsoleInputContext : BasicInputContext
    {
        public override void Execute()
        {
            QuantumConsole qc = QuantumConsole.Instance;
            Debug.Assert(qc.IsActive, "I expected the console to be open while in the Console Input Context.");

            // submit input
            if (Input.GetKeyDown(KeyCode.KeypadEnter))
                qc.InvokeCommand();

            // close console
            if (Input.GetKeyDown(KeyCode.Escape))
                qc.Deactivate();
        }
    }
}
