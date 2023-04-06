namespace RichPackage.SaveSystem
{
    public interface ISaveSystem
    {
        /// <summary>
        /// Save <paramref name="memento"/> with <paramref name="key"/>.
        /// </summary>
        void Save<T>(string key, T memento);

        /// <summary>
        /// Load memento with <paramref name="key"/>.
        /// </summary>
        T Load<T>(string key);

        /// <summary>
        /// Load memento with <paramref name="key"/>.
        /// </summary>
        /// <param name="default">A default value in case <paramref name="key"/> isn't a key.</param>
        T Load<T>(string key, T @default);

        /// <summary>
        /// Returns tr
        /// </summary>
        /// <param name="key"></param>
        /// <returns><see langword="true"/> if there is data saved in <paramref name="key"/>.</returns>
        bool Contains(string key);

        /// <summary>
        /// Removes the memento with <paramref name="key"/>.
        /// </summary>
        void Delete(string key);

        /// <summary>
        /// Reset the entire contents of the save file.
        /// </summary>
        void Clear();
    }
}
