using UnityEngine;
using Sirenix.OdinInspector;

namespace RichPackage.Pooling
{

    /// <summary>
    /// A basic Poolable MonoBehaviour. Serves as an example for how to implement an <see cref="IPoolable"/>
    /// whose lifetime is driven by a <see cref="PoolablePool"/>.
    /// </summary>
    public class Poolable : RichMonoBehaviour, IPoolable
    {
        [MinValue(0)]
        public float lifetime = 10f;

        [ShowInInspector, ReadOnly]
        public bool InUse { get; private set; }

        public PoolablePool PoolOwner { get; set; }
        private Coroutine lifetimeCoroutine;

        public virtual void OnCreate()
        {
            //nada
        }

        public virtual void OnDepool()
        {
            InUse = true;
            gameObject.SetActive(true);
            if (lifetime > 0)
            {
                lifetimeCoroutine = 
                    InvokeAfterDelay(ReturnToPool, lifetime);
            }
        }

        public virtual void OnEnpool()
        {
            InUse = false;
            if (lifetimeCoroutine != null)
            {
                StopCoroutine(lifetimeCoroutine);
            }
            gameObject.SetActive(false);
        }

        [Button, DisableInEditorMode]
        public void ReturnToPool() => PoolOwner.Enpool(this);
    }
}
