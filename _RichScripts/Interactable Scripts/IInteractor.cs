
namespace RichPackage.Interaction
{
	/// <summary>
	/// 
	/// </summary>
	public partial interface IInteractor
	{
        /// <summary>
        /// This method should call `interactable.Activate(this)`.
        /// </summary>
        /// <param name="actor">Activate of the "visitor" pattern.</param>
        void Interact(IInteractable interactable);

		// generic, non-special interaction
		void InteractWith(IInteractable interactable);

		/* special interactions
		 * void InteractWith(Spinner spinner);
		 * void InteractWith(Playdough dough);
		 * ...
		 */
    }
}
