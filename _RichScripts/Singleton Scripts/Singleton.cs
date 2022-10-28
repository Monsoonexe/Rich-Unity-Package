using UnityEngine;

namespace RichPackage
{
	/// <summary>
	/// 
	/// </summary>
	public static class Singleton
    {
        /// <summary>
        /// Set a reference to a singleton to the given instance if valid.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="singletonRef">Reference to the Singleton object, typically a static class variable.</param>
        /// <returns>False if a SingletonError occured.</returns>
        public static bool InitSingleton<T>(T instance, ref T singletonRef)
        {
            var valid = true; //return value
            if (singletonRef == null)
            {   //we are the singleton
                singletonRef = instance;
            }
            else if (!ReferenceEquals(instance, singletonRef))
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
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="singletonRef">Reference to the Singleton object, typically a static class variable.</param>
        /// <returns>False if a SingletonError occured.</returns>
        public static bool InitSingleton<T>(T instance, ref T singletonRef,
            bool dontDestroyOnLoad = true)
            where T : RichMonoBehaviour
        {
            var valid = true; //return value
            if (singletonRef == null)
            {   //we are the singleton
                singletonRef = instance;
                if (dontDestroyOnLoad)
                    instance.DontDestroyOnLoad();
            }
            else if (!instance.Equals(singletonRef))
            {   //there are two Singletons
                //throw new SingletonException(string.Format("[SingletonError] Two instances of a singleton exist: {0} and {1}.",
                //instance.ToString(), singletonRef.ToString()));
                valid = false;
            }
            return valid;
        }

        public static bool InitSingletonOrDestroyGameObject<T>(T instance, ref T singletonRef,
            bool dontDestroyOnLoad = true)
            where T : RichMonoBehaviour
        {
            bool valid;
            if (!(valid = InitSingleton(instance, ref singletonRef, dontDestroyOnLoad)))
                UnityEngine.Object.Destroy(instance.gameObject);

            return valid;
        }

        /// <exception cref="SingletonException"></exception>
        public static void InitSingletonOrThrow<T>(T instance, ref T singletonRef,
            bool dontDestroyOnLoad = true)
            where T : class
        {
            if (!InitSingleton(instance, ref singletonRef))
                throw new SingletonException(typeof(T).Name);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="singletonRef"></param>
        /// <returns>true if the <paramref name="instance"/> is the singleton 
        /// referred to by <paramref name="singletonRef"/>.</returns>
        public static bool ReleaseSingleton<T>(T instance, ref T singletonRef)
            where T : class
        {
            bool isSingleton = ReferenceEquals(instance, singletonRef); // reference equals
            if (isSingleton)
                singletonRef = null;
            return isSingleton; // was released
        }

    }
}
