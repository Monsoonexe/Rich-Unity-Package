using UnityEngine;

namespace RichPackage
{
	/// <summary>
	/// Contains helper methods for implementing the Singleton Pattern.
	/// </summary>
	public static class Singleton
    {
        /// <summary>
        /// Set a reference to a singleton to the given instance if valid.
        /// </summary>
        /// <param name="singleton">Reference to the Singleton object, typically a static class variable.</param>
        /// <returns>False if a SingletonError occured.</returns>
        public static bool Take<T>(T instance, ref T singleton)
            where T : class
        {
            var valid = true; //return value
            if (singleton == null)
            {   //we are the singleton
                singleton = instance;
            }
            else if (!ReferenceEquals(instance, singleton))
            {   //there are two Singletons
                //throw new SingletonException(string.Format("[SingletonError] Two instances of a singleton exist: {0} and {1}.",
                //instance.ToString(), singletonRef.ToString()));
                valid = false;
            }
            return valid;
        }

        /// <summary>
        /// Set a reference to a singleton to the given instance if valid.
        /// </summary>
        /// <param name="singleton">Reference to the Singleton object, typically a static class variable.</param>
        /// <returns>False if a SingletonError occured.</returns>
        public static bool Take<T>(T instance, ref T singleton,
            bool dontDestroyOnLoad = true)
            where T : MonoBehaviour
        {
            var valid = true; //return value
            if (singleton == null)
            {   //we are the singleton
                singleton = instance;
                if (dontDestroyOnLoad)
                    instance.DontDestroyOnLoad();
            }
            else if (instance.GetInstanceID() != singleton.GetInstanceID())
            {   //there are two Singletons
                //throw new SingletonException(string.Format("[SingletonError] Two instances of a singleton exist: {0} and {1}.",
                //instance.ToString(), singletonRef.ToString()));
                valid = false;
            }
            return valid;
        }

        public static bool TakeOrDestroy<T>(T instance, ref T singleton,
            bool dontDestroyOnLoad = true)
            where T : MonoBehaviour
        {
            bool valid;
            if (!(valid = Take(instance, ref singleton, dontDestroyOnLoad)))
                UnityEngine.Object.Destroy(instance);
            return valid;
        }

        public static bool TakeOrDestroyGameObject<T>(T instance, ref T singleton,
            bool dontDestroyOnLoad = true)
            where T : MonoBehaviour
        {
            bool valid;
            if (!(valid = Take(instance, ref singleton, dontDestroyOnLoad)))
                UnityEngine.Object.Destroy(instance.gameObject);

            return valid;
        }

        /// <exception cref="SingletonException"></exception>
        public static void TakeOrThrow<T>(T instance, ref T singleton)
            where T : class
        {
            if (!Take(instance, ref singleton))
                throw new SingletonException(typeof(T).Name);
        }

        /// <exception cref="SingletonException"></exception>
        public static void TakeOrThrowObject<T>(T instance, ref T singleton)
            where T : Object
        {
            if (!Take(instance, ref singleton))
            {
                Debug.LogError($"{instance.name} cannot take the singleton...", instance);
                Debug.Log($"{singleton.name} already holds it.", singleton);
                throw new SingletonException(typeof(T).Name);
            }
        }

        /// <returns>true if the <paramref name="instance"/> is the singleton 
        /// referred to by <paramref name="singleton"/>.</returns>
        public static bool Release<T>(T instance, ref T singleton)
            where T : class
        {
            bool isSingleton = ReferenceEquals(instance, singleton);
            if (isSingleton)
                singleton = null;
            return isSingleton; // was released
        }

    }
}
