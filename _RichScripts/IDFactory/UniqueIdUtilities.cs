using UnityEngine;

namespace RichPackage
{
    /// <summary>
    /// Utility methods for working with <see cref="UniqueID"/>.
    /// </summary>
    public static class UniqueIdUtilities
    {
        public static UniqueID CreateIdFrom(MonoBehaviour mono, bool includeScene = false, bool includePath = false, bool includeType = false)
        {
            string name = mono.name;

            string scene = includeScene
                ? mono.gameObject.scene.name + "/"
                : string.Empty;

            string path = includePath
                ? mono.gameObject.GetFullPath()
                : name;

            string type = includeType
                ? $" ({mono.GetType().Name})"
                : string.Empty;

            string final = $"{scene}{path}{type}";
            return new UniqueID(final);
        }
    }
}
