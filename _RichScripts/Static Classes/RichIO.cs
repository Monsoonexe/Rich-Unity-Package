using System.IO;

namespace RichPackage.IO
{
    /// <summary>
    /// Helper methods for working with IO.
    /// </summary>
    public static class RichIO
    {
        /// <summary>
        /// Deletes the file at <paramref name="path"/>, if it exists.
        /// </summary>
        /// <returns>True if the file was deleted (because it existed).</returns>
        public static bool DeleteFile(string path)
        {
            bool exists;
            if (exists = File.Exists(path))
            {
                File.Delete(path);
            }

            return exists;
        }
    }
}
