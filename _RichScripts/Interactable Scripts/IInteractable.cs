
namespace RichPackage.Interaction
{
    public partial interface IInteractable
    {
        /// <summary>
        /// Gets or sets whether this interactable can be interacted with. It 
        /// can still be seen and will receive hover events, but it will not
        /// be Activatable.
        /// </summary>
        bool IsAvailable { get; set; }

        /// <summary>
        /// Gets or sets whether this interactable is seen at all by the
        /// interaction system.
        /// </summary>
        bool IsEnabled { get; set; }
        
        void OnEnterHover();

        void OnExitHover();

        void OnEnterRange();

        void OnExitRange();

        void Activate(IInteractor actor);

        void Release(IInteractor actor);
    }
}
