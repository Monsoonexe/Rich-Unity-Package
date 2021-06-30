using UnityEngine;

namespace RichPackage.Managed
{
    /// <summary>
    /// Can be statically assigned to the Engine to get ManagedInitializer callbacks.
    /// </summary>
    /// <see cref="ManagedBehaviourEngine"/>
    /// <see cref="IManagedBehaviour"/>
    public abstract class AManagedBehaviour 
        : RichMonoBehaviour, IManagedBehaviour
         //IManagedUpdate, IManagedStart
    {
        /*  should include Initializers (e.g. Awake())
         *  if this is a dynamic object. Otherwise, can use
         *  ManagedInitializers (e.g. IManagedAwake()) to get callbacks
         * 
         *  Prefer to be specific: 
         *  ManagedBehaviourEngine.AddManagedListener((IManagedUpdate)this);
         *  Specific calls are faster.
         */

        protected virtual void OnEnable()
        {
            ManagedBehaviourEngine.AddManagedListener(this);
        }

        protected virtual void OnDisable()
        {
            ManagedBehaviourEngine.RemoveManagedListener(this);
        }

        //public void ManagedUpdate()
        //{
        //    //update
        //}

        //public void ManagedStart()
        //{
        //    //start
        //}
    }
}
