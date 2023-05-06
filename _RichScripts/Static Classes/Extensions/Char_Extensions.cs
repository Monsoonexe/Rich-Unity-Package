namespace RichPackage
{
    public static class Char_Extensions
    {
        /// <summary>
        /// Returns true if <paramref name="query"/> is a vowel.
        /// </summary>
        public static bool IsVowel(this char query)
        {
            switch (query)
            {
                case 'a':
                case 'A':
                case 'e':
                case 'E':
                case 'i':
                case 'I':
                case 'o':
                case 'O':
                case 'u':
                case 'U':
                    return true;
                default:
                    return false;
            }
        }
    }
}
