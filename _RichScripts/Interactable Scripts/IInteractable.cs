
public interface IInteractable
{
    void OnEnterHover(PlayerScript player);

    void OnExitHover(PlayerScript player);

    void OnEnterRange(PlayerScript player);

    void OnExitRange(PlayerScript player);

    void Interact(PlayerScript player);

    void EndInteraction();
}