using ScriptableObjectArchitecture;

/// <summary>
/// Class that shows data based on a BaseVariable.
/// </summary>
/// <typeparam name="T"></typeparam>
[SelectionBase]
public abstract class VariableUIElement<T> : RichUIElement<T>
    where T : BaseVariable
{
    protected override void SubscribeToEvents()
        => targetData.AddListener(UpdateUI);

    protected override void UnsubscribeFromEvents()
        => targetData.RemoveListener(UpdateUI);
}
