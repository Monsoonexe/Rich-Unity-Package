using UnityEngine;

namespace RichPackage
{
    /// <summary>
    /// Utility methods for working with <see cref="UniqueID"/>.
    /// </summary>
    public static class UniqueIdUtilities
    {
        public static UniqueID CreateIdFrom(MonoBehaviour mono, 
            bool includeScene = false, 
            bool includePath = false,
            bool includeName = false,
            bool includeType = false)
        {
            string scene = includeScene
                ? mono.gameObject.scene.name + "/"
                : string.Empty;

            string path = includePath
                ? mono.gameObject.GetFullPath()
                : string.Empty;

            string name = includeName
                ? mono.name
                : string.Empty;

            string type = includeType
                ? $" ({mono.GetType().Name})"
                : string.Empty;

            string final = $"{scene}{path}{name}{type}";
            UnityEngine.Assertions.Assert.IsFalse(string.IsNullOrEmpty(final), "Should include at least one thing.");
            return new UniqueID(final);
        }
    }
}
