namespace RichPackage.Events.Signals
{
    /// <summary>
    /// The scene is about to be unloaded. Last call before your possible demise! <br/>
    /// NOTE: If multiple scenes exist, it may not be 'your' scene that is being unloaded. <br/>
    /// See also: <seealso cref="UnityEngine.SceneManagement.SceneManager.sceneUnloaded"/>
    /// </summary>
    public class ScenePreUnloadSignal : ASignal
    {
        //exists
    }
}
