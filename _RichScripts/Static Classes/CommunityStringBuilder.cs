using System.Text;

/// <summary>
/// 
/// </summary>
public static class CommunityStringBuilder
{
    private static readonly StringBuilder communityStringBuilder = new StringBuilder(10);

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

    #region Constructors



    #endregion
}
