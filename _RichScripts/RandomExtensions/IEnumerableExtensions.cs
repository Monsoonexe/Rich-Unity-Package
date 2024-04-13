using System.Collections.Generic;

namespace RichPackage.RandomExtensions
{
    public static class IEnumerableExtensions
    {
        public static T GetRandomElement<T>(this IEnumerable<T> items)
        {
            using (UnityEngine.Rendering.ListPool<T>.Get(out List<T> list))
            {
                list.AddRange(items);
                return list.GetRandomElement();
            }
        }

        public static IEnumerable<T> YieldRandom<T>(this IEnumerable<T> src, int count)
        {
            if (count <= 0)
                yield break;

            using (UnityEngine.Rendering.ListPool<T>.Get(out var values))
            {
                values.AddRange(src);
                for (int i = 0; i < count; ++i)
                    yield return values.TakeRandomElement();
            }
        }
    }
}
