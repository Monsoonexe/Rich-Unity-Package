
namespace RichPackage.Managed
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class AManagedEvent : AUnityLifetimeEvent
    {
        ////override this because specific is better than vague for this system.
        //protected virtual void OnEnable()
        //{
        //    ManagedBehaviourEngine.AddManagedListener(this);
        //}

        ////override this because specific is better than vague for this system.
        //protected virtual void OnDisable()
        //{
        //    ManagedBehaviourEngine.RemoveManagedListener(this);
        //}
    }
}
