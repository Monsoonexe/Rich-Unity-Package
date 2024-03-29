﻿using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace RichPackage.TagSystem
{
    /// <summary>
    /// Contains methods to help make querying <see cref="Tag"/>s easier.
    /// </summary>
    public static class TagUtilities
    {
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
            => string.Join(separator, tags.Select((t) => t.ToString()));

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

        public static IEnumerable<Tag> GetAllTags(this GameObject obj)
        {
            var components = UnityEngine.Rendering.ListPool<Component>.Get();
            obj.GetComponents(components);

            foreach (Component tagged in components)
            {
                if (tagged is ITagged t)
                {
                    foreach (Tag tag in t.GetTags())
                        yield return tag;
                }
            }
            UnityEngine.Rendering.ListPool<Component>.Release(components);
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
