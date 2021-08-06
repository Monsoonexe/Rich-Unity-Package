using UnityEngine;

/// <summary>
/// 
/// </summary>
public class UseDeckExample : RichMonoBehaviour
{
    [SerializeField]
    private ADeck deck;

    void DrawFromDeck()
    {
        //this sucks, but until Unity can serialize 'ADeck<ScriptableObject>',
        //it must be casted.
        var derp = (ADeck<ScriptableObject>)deck;
        derp.Draw();
    }
}
