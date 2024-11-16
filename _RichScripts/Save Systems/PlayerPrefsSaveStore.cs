// TODO - obfuscate
// TODO - abstract the serializatation backend, effectively making this A savestore wrapper

using System;
using UnityEngine;

namespace RichPackage.SaveSystem
{
    /// <summary>
    /// A save store for UnityEngine.PlayerPrefs.
    /// </summary>
    /// <remarks>Currently only uses EasySave's serializer.</remarks>
    public class PlayerPrefsSaveStore : ISaveStore
    {
        /// <summary>
        /// A prefix to append to all keys, effectively scoping all of the entries.
        /// </summary>
        public string Scope = null;

        public void Clear() => throw new NotImplementedException("How to do this? should we track ALL of our keys? Or really delete the entire PlayerPrefs??");
        
        public void Delete(string key)
        {
            key = ApplyScope(key);
            PlayerPrefs.DeleteKey(key);
        }

        public bool KeyExists(string key)
        {
            key = ApplyScope(key);
            return PlayerPrefs.HasKey(key);
        }

        public T Load<T>(string key)
        {
            UnityEngine.Assertions.Assert.IsTrue(KeyExists(key), $"Key not found '{key}'");

            key = ApplyScope(key);
            if (typeof(T) == typeof(int))
            {
                return (T)(object)PlayerPrefs.GetInt(key);
            }

            if (typeof(T) == typeof(float))
            {
                return (T)(object)PlayerPrefs.GetFloat(key);
            }

            if (typeof(T) == typeof(bool))
            {
                return (T)(object)(PlayerPrefs.GetInt(key) != 0);
            }

            if (typeof(T) == typeof(string))
            {
                return (T)(object)PlayerPrefs.GetString(key);
            }

            string text = PlayerPrefs.GetString(key);
            byte[] bytes = Convert.FromBase64String(text);
            return ES3.Deserialize<T>(bytes);
        }

        public void LoadInto<T>(string key, T memento)
            where T : class
        {
            UnityEngine.Assertions.Assert.IsTrue(KeyExists(key), $"Key not found '{key}'");

            key = ApplyScope(key);
            string text = PlayerPrefs.GetString(key);
            byte[] bytes = Convert.FromBase64String(text);
            ES3.DeserializeInto(bytes, memento);
        }

        public void Save<T>(string key, T memento)
        {
            key = ApplyScope(key);

            if (typeof(T) == typeof(int))
            {
                PlayerPrefs.SetInt(key, memento.GetHashCode());
                return;
            }

            if (typeof(T) == typeof(float))
            {
                PlayerPrefs.SetFloat(key, (float)(object)memento);
                return;
            }

            if (typeof(T) == typeof(bool))
            {
                int value = memento.GetHashCode();
                PlayerPrefs.SetInt(key, value);
                return;
            }

            if (typeof(T) == typeof(string))
            {
                PlayerPrefs.SetString(key, (string)(object)memento);
                return;
            }

            byte[] bytes = ES3.Serialize(memento);
            string text = Convert.ToBase64String(bytes);
            PlayerPrefs.SetString(key, text);
        }

        private string ApplyScope(string key)
        {
            string scope = Scope;

            if (scope != null)
                key = scope + key;

            return key;
        }
    }
}
