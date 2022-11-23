using System.Collections.Generic;

namespace RichPackage.TagSystem
{
    public interface ITagged
    {
        IList<Tag> GetTags();
    }
}
