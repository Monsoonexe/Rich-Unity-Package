using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace RichPackage.TagSystem
{
    /// <summary>
    /// 
    /// </summary>
    public static class TagUtilities
    {
        // TODO - support multiple tagged components
        // TODO - HasAnyXTags

        public static IEnumerable<string> GetValues(this IEnumerable<Tag> tags)
        {
            foreach (Tag tag in tags)
                yield return tag.Value;
        }

        /// <summary>
        /// Get values from <paramref name="tags"/> that match 
        /// <paramref name="propertyQuery"/>.
        /// </summary>
        public static IEnumerable<string> GetValues(this IEnumerable<Tag> tags,
            string propertyQuery)
        {
            foreach (Tag t in tags)
                if (t.Property.Equals(propertyQuery)) // TODO - quick string compare
                    yield return t.Value;
        }
        
        public static IEnumerable<string> GetProperties(this IEnumerable<Tag> tags)
        {
            foreach (Tag t in tags)
                yield return t.Property;
        }

        /// <summary>
        /// Comma-separated list.
        /// </summary>
        public static string ToString(this IEnumerable<Tag> tags)
            => ToString(tags, ", ");

        public static string ToString(this IEnumerable<Tag> tags, string separator)
        {
            var sb = StringBuilderCache.Rent();
            foreach (Tag t in tags)
            {
                sb.Append(t.ToString());
                sb.Append(separator);
            }

            // remove last separator
            sb.Remove(sb.Length - separator.Length, separator.Length);
            return sb.ToStringAndReturn();
        }

        public static bool Contains(this IEnumerable<Tag> tags, Tag tag)
        {
            foreach (Tag t in tags)
                if (t.Equals(tag))
                    return true;
            return false;
        }

        public static IEnumerable<Tag> GetTags(this GameObject obj)
        {
            if (obj.TryGetComponent(out ITagged tagged))
                return tagged.GetTags();
            return Tag.None;
        }

        public static bool HasTag(this GameObject obj, Tag tag)
        {
            if (obj.TryGetComponent(out ITagged tagged))
                if (Contains(tagged.GetTags(), tag))
                    return true;
            return false;
        }

        public static bool HasAnyTag(this GameObject obj, IEnumerable<Tag> tags)
        {
            if (obj.TryGetComponent(out ITagged tagged))
            {
                IEnumerable<Tag> otherTags = tagged.GetTags();
                foreach (Tag t in tags) // TODO - fetch comp once
                    if (otherTags.Contains(t))
                        return true;
            }
            return false;
        }

        public static bool HasAllTags(this GameObject obj, IEnumerable<Tag> tags)
        {
            if (obj.TryGetComponent(out ITagged tagged))
            {
                IEnumerable<Tag> otherTags = tagged.GetTags();
                foreach (Tag t in tags) // TODO - fetch comp once
                    if (!otherTags.Contains(t))
                        return false;
                return true;
            }
            return false;
        }
    }
}
