using UnityEngine;

namespace RichPackage
{
    /// <summary>
    /// Utility methods for working with <see cref="UniqueID"/>.
    /// </summary>
    public static class UniqueIdUtilities
    {
        public static UniqueID CreateFullScopeIdFrom(MonoBehaviour mono, bool includeType = true)
            => CreateIdFrom(mono, true, true, true, includeType);

        /// <param name="includeScene">If <see langword="true"/>, the id will include the name of the scene.</param>
        /// <param name="includePath">If <see langword="true"/>, the id will be formatted as a path, taking the other flags into account.</param>
        /// <param name="includeName">If <see langword="true"/>, the id will include <paramref name="mono"/>'s name.</param>
        /// <param name="includeType">If <see langword="true"/>, the id will include <paramref name="mono"/>'s type, taking other flags into account.</param>
        public static UniqueID CreateIdFrom(MonoBehaviour mono,
            bool includeScene = false, 
            bool includePath = false,
            bool includeName = false,
            bool includeType = false)
        {
            // handle scene
            string scene = string.Empty;
            if (includeScene)
            {
                scene = mono.gameObject.scene.name;

                // add separator if doing a full path
                if (includePath)
                    scene += "/";
            }

            // handle path
            string path = string.Empty;
            if (includePath)
            {
                path = mono.gameObject.GetFullPath();
            }

            // handle name
            string name = string.Empty;
            if (includeName)
            {
                string n = mono.name;
                // prevent dupes
                if (path != n)
                    name = n;
            }
            else if (includePath)
            {
                // don't dupe the name
                path = path.Remove(mono.name);
            }

            // handle type switch
            string type = string.Empty;
            if (includeType)
            {
                type = mono.GetType().Name;
                
                // if type is not the only switch,
                // then put it in parens
                if (includeName)
                {
                    type = $" ({type})";
                }
                else  if (includePath)
                {
                    path += "/";
                }
            }

            // FIXME: smarter separators
            string final = $"{scene}{path}{name}{type}";

            // TODO - assert path with no name or type

            Debug.Assert(includeName || includeType, 
                $"The id '{final}' doesn't include a name or type, so it might be ambiguous.");
            Debug.Assert(!string.IsNullOrEmpty(final),
                "The id should include at least one thing.");

            return new UniqueID(final);
        }
    }
}
