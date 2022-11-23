using System.Collections.Generic;
using UnityEngine;

namespace RichPackage.TagSystem
{
    /// <summary>
    /// Anything can be tagged, but this is a quick-and-dirty tagged behavior.
    /// </summary>
    public class TaggedMonoBehaviour : MonoBehaviour, ITagged
    {
        [SerializeField]
        private Tag[] tags = Tag.None;

        public IList<Tag> Tags { get => GetTags(); }

        public IList<Tag> GetTags() => tags ?? Tag.None; // never return null
    }
}