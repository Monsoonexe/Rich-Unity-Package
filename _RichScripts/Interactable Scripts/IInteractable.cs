
/// <summary>
/// Common interface
/// </summary>
public interface IInteractable
{
    //empty on purpose -- common interface.
}

/// <summary>
/// The Player can interact with Interactables and pass some information.
/// </summary>
/// <typeparam name="T"></typeparam>
public interface IInteractable<T> : IInteractable
{
    void OnEnterRange(T actor);
    void Interact(T actor);
    void OnExitRange(T actor);
}

/// <summary>
/// The Player can interact with Interactables, pass some information, and get something back.
/// </summary>
/// <typeparam name="Ureturn"></typeparam>
/// <typeparam name="Targ"></typeparam>
public interface IInteractable<Ureturn, Targ> : IInteractable
{
    Ureturn OnEnterRange(Targ actor);
    Ureturn Interact(Targ actor);
    Ureturn OnExitRange(Targ actor);
}
