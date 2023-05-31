using UnityEngine;

namespace RichPackage
{
    /// <summary>
    /// Utility methods for working with <see cref="UniqueID"/>.
    /// </summary>
    public static class UniqueIdUtilities
    {
        public static UniqueID CreateIdFrom(MonoBehaviour mono, bool includeScene)
        {
            string scene = includeScene
                ? mono.gameObject.scene.name + "/" // separator
                : string.Empty;
            string name = mono.name;
            string type = mono.GetType().Name;

            return new UniqueID($"{scene}{name}-{type}");
        }
    }
}
