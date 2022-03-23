using UnityEngine;

namespace RichPackage
{
    /// <summary>
    /// 
    /// </summary>
    public static class LayerMask_Extensions
    {
        public static bool ContainsLayer(this LayerMask layerMask, int layer)
            => layerMask == (layerMask | (1 << layer));
    }
}
