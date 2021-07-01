using System.Text;

/// <summary>
/// 
/// </summary>
public static class CommunityStringBuilder
{
    private const int STARTING_AMOUNT = 16; //salt to needs of project.
    private static readonly StringBuilder communityStringBuilder 
        = new StringBuilder(STARTING_AMOUNT);

    /// <summary>
    /// Community String Builder (so you don't have to 'new' one).
    /// Just don't bet it will hold its data. Always safe to use right away.
    /// </summary>
    public static StringBuilder Instance
    {
        get
        {
            communityStringBuilder.Clear(); // clear so it's safe to sue
            return communityStringBuilder;
        }
    }
}
