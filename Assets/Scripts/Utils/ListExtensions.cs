using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Razorhead.Core
{
    public static class ListExtensions
    {
        public static bool IsEmpty<T>(this List<T> list)
        {
            return list == null || list.Count == 0;
        }

        public static void Swap<T>(this List<T> a, List<T> b)
        {
            using var _ = ListPool<T>.Get(out var temp);
            temp.AddRange(a);

            a.Clear();
            a.AddRange(b);

            b.Clear();
            b.AddRange(temp);
        }

        public static void AddOnce<T>(this List<T> list, T item)
        {
            if (!list.Contains(item)) list.Add(item);
        }

        private static readonly System.Random shuffleRng = new();

        /// <summary>Based on https://en.wikipedia.org/wiki/Fisher%E2%80%93Yates_shuffle</summary>
        public static void Shuffle<T>(this IList<T> list)
        {
            int n = list.Count;

            while (n > 1)
            {
                var k = shuffleRng.Next(n--);
                (list[n], list[k]) = (list[k], list[n]);
            }
        }

        public static PooledObject<List<T>> GetTempCopy<T>(this List<T> original, out List<T> copy)
        {
            var handle = ListPool<T>.Get(out copy);

            for (int i = 0; i < original.Count; i++)
            {
                copy.Add(original[i]);
            }

            return handle;
        }

        public static RectInt GetBoundingRect(this IList<Vector2Int> list)
        {
            if (list == null || list.Count == 0) return default;

            var min = list[0];
            var max = list[0];

            for (int i = 1; i < list.Count; i++)
            {
                min = Vector2Int.Min(min, list[i]);
                max = Vector2Int.Max(max, list[i]);
            }

            return new RectInt(min, max - min);
        }
    }
}