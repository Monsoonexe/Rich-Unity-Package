//TODO - maybe implement the Rent-Return pattern

using System.Text;

namespace RichPackage.Pooling
{
    /// <summary>
    /// A <see cref="StringBuilder"/> that is re-used 
    /// </summary>
    public static class CommunityStringBuilder
    {
        private const int STARTING_AMOUNT = 128; //salt to needs of project.

        private static readonly StringBuilder communityStringBuilder 
            = new StringBuilder(STARTING_AMOUNT);

        /// <summary>
        /// Lazily trims the string builder to the correct size on fetch.
        /// </summary>
        public static int MaxCapacity = 1024;

        /// <summary>
        /// Community String Builder (so you don't have to 'new' one).
        /// Just don't bet it will hold its data. Always safe to use right away.
        /// </summary>
        public static StringBuilder Instance
        {
            get
            {
                communityStringBuilder.Clear(); // clear so it's safe to sue

                //trim if getting too big
                if (communityStringBuilder.Capacity > MaxCapacity)
                    communityStringBuilder.Capacity = MaxCapacity;
                    
                return communityStringBuilder;
            }
        }
    }
}
