using System;

namespace RichPackage.InputSystem
{
    /// <summary>
    /// Standard input.
    /// </summary>
    public sealed class StandardAppInputProfile
        : AInputProfileAsset<StandardAppInputProfile.InputProfile>
    {
        [Serializable]
        public sealed class InputProfile : PlayerInputProfile
        {
            // exists
        }
    }
}
