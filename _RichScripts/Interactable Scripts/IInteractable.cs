
namespace RichPackage.Interaction
{
    public partial interface IInteractable
    {
        /// <summary>
        /// Gets or sets whether this can be interacted with. It 
        /// can still be seen and will receive hover events, but it will not
        /// be Activatable if false.
        /// </summary>
        bool IsAvailable { get; set; }

        /// <summary>
        /// Gets or sets whether this is seen at all by the interaction system.
        /// </summary>
        bool IsEnabled { get; set; }

        /// <summary>
        /// Called when the interactor stops hovering over the interactable.
        /// </summary>
        void OnEnterHover();

        /// <summary>
        /// Called when the interactor begins hovering over the interactable.
        /// </summary>
        void OnExitHover();

        /// <summary>
        /// Called when the <paramref name="actor"/> enters the range.
        /// </summary>
        void OnEnterRange(IInteractor actor);

        /// <summary>
        /// Called when the <paramref name="actor"/> exits the range.
        /// </summary>
        void OnExitRange(IInteractor actor);

        /// <summary>
        /// The target of this call should call the <paramref name="actor"/>'s specific InteractWith method.
        /// </summary>
        /// <param name="actor"></param>
        void Activate(IInteractor actor);

        /// <summary>
        /// Called when the <paramref name="actor"/> is finished operating this.
        /// </summary>
        void Release(IInteractor actor);
    }
}
