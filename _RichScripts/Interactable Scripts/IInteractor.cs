
namespace RichPackage.Interaction
{
    /// <summary>
    /// Something that can interact with an IInteractable.
    /// </summary>
    public partial interface IInteractor
	{
        UnityEngine.Transform Transform { get; }

        void OnLoseFocus(IInteractable interactable);
        void OnTakeFocus(IInteractable interactable);

        /// <summary>
        /// This method should call `interactable.Activate(this)`.
        /// </summary>
        /// <param name="interactable">Activate of the "visitor" pattern.</param>
        void InteractWith(IInteractable interactable);

		/* special interactions
		 * void InteractWith(Spinner spinner);
		 * void InteractWith(Playdough dough);
		 * ...
		 */
    }
}
