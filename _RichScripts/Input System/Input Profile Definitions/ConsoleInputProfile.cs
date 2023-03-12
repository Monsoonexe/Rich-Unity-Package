using QFSW.QC;
using System;
using UnityEngine;

namespace RichPackage.InputSystem
{
    public class ConsoleInputProfile
        : AInputProfileAsset<ConsoleInputProfile.InputProfile>
    {
        [Serializable]
        public sealed class InputProfile : PlayerInputProfile
        {
            public override void OnLogic()
            {
                // submit input
                if (Input.GetKeyDown(KeyCode.KeypadEnter))
                    QuantumConsole.Instance.InvokeCommand();

                // close console
                if (Input.GetKeyDown(KeyCode.Escape))
                    QuantumConsole.Instance.Deactivate();
            }
        }
    }
}
