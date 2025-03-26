namespace RichPackage.SaveSystem
{
    /// <summary>
    /// Stores save data.
    /// </summary>
    public interface ISaveStore
    {
        /// <summary>
        /// Save <paramref name="memento"/> with <paramref name="key"/>.
        /// </summary>
        void Save<T>(string key, T memento);

        /// <summary>
        /// Load memento with <paramref name="key"/>.
        /// </summary>
        T Load<T>(string key);

#if UNITY_2020_OR_NEWER

        /// <summary>
        /// Load memento with <paramref name="key"/>.
        /// </summary>
        /// <param name="default">A default value to be used when <paramref name="key"/> key doesn't exist.</param>
        T Load<T>(string key, T @default) => KeyExists(key) ? Load<T>(key) : @default;

#endif

        /// <summary>
        /// Load memento with <paramref name="key"/>.
        /// </summary>
        void LoadInto<T>(string key, T memento) where T : class;

        /// <param name="key"></param>
        /// <returns><see langword="true"/> if there is data saved in <paramref name="key"/>.</returns>
        bool KeyExists(string key);

        /// <summary>
        /// Removes the memento with <paramref name="key"/>.
        /// </summary>
        void Delete(string key);

        /// <summary>
        /// Removes all save data.
        /// </summary>
        void Clear();
    }

#if !UNITY_2020_OR_NEWER

    public static class ISaveStoreExtensions
    {
        public static T Load<T>(this ISaveStore self, string key, T @default) => self.KeyExists(key) ? self.Load<T>(key) : @default;
    }

#endif
}
