
namespace RichPackage.Interaction
{
    public interface IInteractable
    {
        void OnEnterHover();

        void OnExitHover();

        void OnEnterRange();

        void OnExitRange();

        void Interact();

        void EndInteraction();
    }
}
