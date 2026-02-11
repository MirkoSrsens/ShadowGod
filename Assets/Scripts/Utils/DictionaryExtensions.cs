using System;
using System.Collections.Generic;
using UnityEngine.Pool;

namespace Razorhead.Core
{
    public static class DictionaryExtensions
    {
        public static PooledObject<Dictionary<TKey, TValue>> ToStandardDictionary<TKey, TValue>(this SerializableDictionary<TKey, TValue> data, out Dictionary<TKey, TValue> outVal)
        {
            var res = DictionaryPool<TKey, TValue>.Get(out outVal);
            foreach (var item in data)
            {
                outVal.Add(item.Key, item.Value);
            }

            return res;
        }

        public static void Overwrite<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key, TValue value)
        {
            if(!dict.ContainsKey(key))
            {
                dict.Add(key, value);
            }
            else
            {
                dict[key] = value;
            }
        }

        public static bool IsEmpty<TKey, TValue>(this Dictionary<TKey, TValue> dict)
        {
            return dict == null || dict.Count == 0;
        }

        public static void RemoveWhere<TKey, TValue>(this Dictionary<TKey, TValue> dict, Func<KeyValuePair<TKey, TValue>, bool> predicate)
        {
            using (HashSetPool<TKey>.Get(out var toRemove))
            {
                foreach (var item in dict)
                {
                    if (predicate?.Invoke(item) ?? false) toRemove.Add(item.Key);
                }

                foreach (var item in toRemove)
                {
                    dict.Remove(item);
                }
            }
        }

        public static void RemoveWhere<TKey, TValue, TState>(this Dictionary<TKey, TValue> dict, TState state, Func<KeyValuePair<TKey, TValue>, TState, bool> predicate)
        {
            using (HashSetPool<TKey>.Get(out var toRemove))
            {
                foreach (var item in dict) if (predicate?.Invoke(item, state) ?? false) toRemove.Add(item.Key);
                foreach (var item in toRemove) dict.Remove(item);
            }
        }

        public static void RemoveWhere<T>(this IList<T> list, Func<T, bool> predicate)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (predicate?.Invoke(list[i]) ?? false) list.RemoveAt(i--);
            }
        }

        public static void RemoveWhere<T, TState>(this IList<T> list, TState state, Func<T, TState, bool> predicate)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (predicate?.Invoke(list[i], state) ?? false) list.RemoveAt(i--);
            }
        }
    }
}