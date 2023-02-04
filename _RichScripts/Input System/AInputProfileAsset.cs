using Sirenix.OdinInspector;
using UnityEngine;

namespace RichPackage.InputSystem
{
    /// <summary>
    /// Base class for all input profiles for use with <see cref="PlayerInput"/>.
    /// </summary>
    /// <remarks>Inheritors should inherit from <see cref="AInputProfileAsset{TProfile}"/>.</remarks>
    public abstract class AInputProfileAsset : RichScriptableObject
    {
        public abstract class PlayerInputProfile : PlayerInput.InputProfile
        {
            // exists
        }

        public abstract PlayerInputProfile Profile { get; }
        
        public static implicit operator PlayerInputProfile(AInputProfileAsset asset)
        {
            return asset.Profile;
        }
    }

    /// <summary>
    /// Base generic class for all input profiles for use with <see cref="PlayerInput"/>.
    /// </summary>
    public abstract class AInputProfileAsset<TProfile> : AInputProfileAsset
        where TProfile : AInputProfileAsset.PlayerInputProfile
    {
        [SerializeField, InlineProperty, HideLabel, Title(nameof(Profile))]
        protected TProfile profile;

        public sealed override PlayerInputProfile Profile { get => profile; }
        
        /// <summary>
        /// Sets this profile as the active profile.
        /// </summary>
        public void SetActive()
        {
            profile.SetActive();
        }
    }
}
