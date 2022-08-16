using UnityEngine;

namespace RichPackage
{
    /// <summary>
    /// Mark this <see cref="GameObject"/> as <see cref="Object.DontDestroyOnLoad(Object)"/>
    /// </summary>
    [AddComponentMenu(ConstStrings.RICH_PACKAGE + "/" + nameof(DontDestroyOnLoadComponent))]
    public sealed class DontDestroyOnLoadComponent : RichMonoBehaviour
    {
        protected override void Reset()
        {
            base.Reset();
            SetDevDescription("Marks this GameObject as DontDestroyOnLoad.");
        }

        protected override void Awake()
        {
            base.Awake();
            myTransform.SetParent(null, worldPositionStays: true);
            DontDestroyOnLoad(gameObject);
            Destroy(this);
        }
    }
}
