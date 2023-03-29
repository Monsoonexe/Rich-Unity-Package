namespace RichPackage.Databases
{
    /// <summary>
    /// Interface for items that are kept within an <see cref="ADatabaseAsset{TData}"/>.
    /// </summary>
    public interface IDatabaseMember
    {
        int Key { get; set; }
    }
}
