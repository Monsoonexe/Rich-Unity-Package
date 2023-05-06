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

        /// <summary>
        /// Quicker than <see cref="char.ToLower(char)"/> when using ASCII.
        /// </summary>
        public static char QuickToLower(this char c)
        {
            if (c >= 'A' && c <= 'Z')
            {
                return (char)(c + 32);
            }
            else
            {
                return c;
            }
        }

        /// <summary>
        /// Quicker than <see cref="char.ToUpper(char)"/> when using ASCII.
        /// </summary>
        public static char QuickToUpper(this char c)
        {
            if (c >= 'a' && c <= 'c')
            {
                return (char)(c - 32);
            }
            else
            {
                return c;
            }
        }
    }
}
