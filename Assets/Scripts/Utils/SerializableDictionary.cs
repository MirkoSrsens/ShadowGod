using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Runtime.CompilerServices;
using UnityEngine;

namespace Razorhead.Core
{
    /// <summary>
    /// Serializable generic Dictionary
    /// </summary>
    /// <typeparam name="K"></typeparam>
    /// <typeparam name="V"></typeparam>
    [Serializable]
    public class SerializableDictionary<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>, ISerializationCallbackReceiver
    {
        [Serializable]
        public class KeyValuePair
        {
            public TKey key;
            public TValue value;
    
            public KeyValuePair()
            {
            }
    
            public KeyValuePair(TKey key, TValue value)
            {
                this.key = key;
                this.value = value;
            }
    
            public bool Equals(KeyValuePair pair) => EqualityComparer<TKey>.Default.Equals(key, pair.key) && EqualityComparer<TValue>.Default.Equals(value, pair.value);
    
            public override bool Equals(object obj) => obj is KeyValuePair pair && Equals(pair);
    
            public override int GetHashCode() => HashCode.Combine(key, value);
        }
    
        [SerializeField]
        private List<KeyValuePair> data = new();
    
        [JsonIgnore]
        private Dictionary<TKey, int> _keyIndexTable;
    
        private Dictionary<TKey, int> keyIndexTable
        {
            get
            {
                if (_keyIndexTable == null)
                {
                    _keyIndexTable = new(EqualityComparer<TKey>.Default);
    
                    for (int i = 0; i < data.Count; i++)
                    {
                        var key = data[i].key;
                        _keyIndexTable.Add(key, i);
                    }
                }
    
                return _keyIndexTable;
            }
        }
    
        private int GetKeyIndex(TKey key)
        {
            return keyIndexTable.TryGetValue(key, out var index) ? index : -1;
        }
    
        private TValue GetValueAtIndex(int index)
        {
            return data[index].value;
        }
    
        private void SetValueAtIndex(int index, TValue value)
        {
            data[index].value = value;
        }
    
        private void AddNewPair(TKey key, TValue value)
        {
            keyIndexTable.Add(key, data.Count);
    
            data.Add(new KeyValuePair(key, value));
        }
    
        // Indexer to access values by key
        public TValue this[TKey key]
        {
            get
            {
                return key != null && keyIndexTable.TryGetValue(key, out var index) ? data[index].value : default;
            }
            set
            {
                int index = GetKeyIndex(key);
    
                if (index != -1)
                {
                    SetValueAtIndex(index, value);
                }
                else
                {
                    AddNewPair(key, value);
                }
            }
        }
    
        public int Count => data.Count;
    
        // Get all keys in the dictionary
        public KeysEnumerator Keys => new(data);
    
        // Get all values in the dictionary
        public ValuesEnumerator Values => new(data);
    
        public SerializableDictionary(Dictionary<TKey, TValue> other)
        {
            foreach (var item in other)
            {
                AddNewPair(item.Key, item.Value);
            }
        }
    
        public SerializableDictionary()
        {
        }
    
        public SerializableDictionary(SerializableDictionary<TKey, TValue> other)
        {
            foreach (var item in other)
            {
                AddNewPair(item.Key, item.Value);
            }
        }
    
        public TValue GetValue(TKey key)
        {
            return key != null && keyIndexTable.TryGetValue(key, out var index) ? data[index].value : default;
        }
    
        public bool GetValue(TKey key, out TValue value)
        {
            if (key != null && keyIndexTable.TryGetValue(key, out var index) && index != -1)
            {
                value = data[index].value;
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }
    
        public void Overwrite(TKey key, TValue value)
        {
            var index = GetKeyIndex(key);
    
            if (index != -1)
            {
                SetValueAtIndex(index, value);
            }
            else
            {
                AddNewPair(key, value);
            }
        }
    
        public bool ContainsKey(TKey key)
        {
            return key != null && keyIndexTable.ContainsKey(key);
        }
    
        public TValue GetValueOrDefault(TKey key)
        {
            return key != null && keyIndexTable.TryGetValue(key, out var index) ? data[index].value : default;
        }
    
        public bool TryAdd(TKey key, TValue value)
        {
            var index = GetKeyIndex(key);
    
            if (index == -1)
            {
                AddNewPair(key, value);
                return true;
            }
    
            return false;
        }
    
        public void Add(TKey key, TValue value)
        {
            var index = GetKeyIndex(key);
    
            if (index != -1)
            {
                throw new DuplicateNameException(string.Format("Duplicate key detected {0}", key));
            }
    
            AddNewPair(key, value);
        }
    
        public void AddRange(IEnumerable<KeyValuePair<TKey, TValue>> source)
        {
            foreach (var (key, value) in source)
            {
                Add(key, value);
            }
        }
    
        public TValue Find(TKey key)
        {
            return key != null && keyIndexTable.TryGetValue(key, out var index) ? data[index].value : default;
        }
    
        public bool TryGetValue(TKey key, out TValue value)
        {
            if (key != null && keyIndexTable.TryGetValue(key, out var index))
            {
                value = data[index].value;
                return true;
            }
            else
            {
                value = default;
                return false;
            }
        }
    
        public void Clear()
        {
            _keyIndexTable?.Clear();
            data.Clear();
        }
    
        public bool IsEmpty()
        {
            return data == null || data.IsEmpty();
        }
    
        public bool Remove(TKey key)
        {
            if (key != null && keyIndexTable.TryGetValue(key, out var index))
            {
                var lastIndex = data.Count - 1;
    
                // Last item in the list can always be safely removed
                if (index == lastIndex)
                {
                    data.RemoveAt(index);
                    keyIndexTable.Remove(key);
                }
                else // We remove item at index and swap back last element in its place, for more info google `RemoveAtSwapBack` 
                {
                    var lastElement = data[lastIndex];
                    data.RemoveAt(lastIndex);
                    data[index] = lastElement;
                    keyIndexTable[lastElement.key] = index;
                    keyIndexTable.Remove(key);
                }
    
                return true;
            }
    
            return false;
        }
    
        public Enumerator GetEnumerator()
        {
            return new Enumerator(data);
        }
    
        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return new Enumerator(data);
        }
    
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    
        internal void AddRange(SerializableDictionary<TKey, TValue> toAdd)
        {
            foreach (var item in toAdd)
            {
                AddNewPair(item.Key, item.Value);
            }
        }
    
        public bool Insert(TKey key, TValue value, bool overwrite = true)
        {
            var index = GetKeyIndex(key);
    
            if (index == -1)
            {
                AddNewPair(key, value);
                return true;
            }
            else if (overwrite)
            {
                data[index].value = value;
            }
    
            return false;
        }
    
        public void OnBeforeSerialize()
        {
        }
    
        public void OnAfterDeserialize()
        {
            _keyIndexTable = null;
        }
    
        internal void Add(SerializableDictionary<TKey, TValue> data)
        {
            foreach (var item in data)
            {
                Add(item.Key, item.Value);
            }
        }
    
        public struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>, IEnumerator, IDisposable
        {
            readonly List<KeyValuePair> list;
            int index;
    
            public Enumerator(List<KeyValuePair> list)
            {
                this.list = list;
                index = -1;
            }
    
            public readonly KeyValuePair<TKey, TValue> Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
                    var item = list[index];
                    return new KeyValuePair<TKey, TValue>(item.key, item.value);
                }
            }
    
            readonly object IEnumerator.Current => Current;
    
            public readonly void Dispose() { }
    
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext() => ++index < list.Count;
    
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Reset() => index = -1;
        }
    
        public struct KeysEnumerator : IEnumerator<TKey>, IEnumerator, IDisposable, IEnumerable<TKey>
        {
            readonly List<KeyValuePair> list;
            int index;
    
            public KeysEnumerator(List<KeyValuePair> list)
            {
                this.list = list;
                index = -1;
            }
    
            public readonly TKey Current => list[index].key;
            readonly object IEnumerator.Current => Current;
    
            public readonly void Dispose() { }
    
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext() => ++index < list.Count;
    
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Reset() => index = -1;
    
            public readonly KeysEnumerator GetEnumerator() => this;
            readonly IEnumerator IEnumerable.GetEnumerator() => this;
            readonly IEnumerator<TKey> IEnumerable<TKey>.GetEnumerator() => this;
        }
    
        public struct ValuesEnumerator : IEnumerator<TValue>, IEnumerator, IDisposable, IEnumerable<TValue>
        {
            readonly List<KeyValuePair> list;
            int index;
    
            public ValuesEnumerator(List<KeyValuePair> list)
            {
                this.list = list;
                index = -1;
            }
    
            public readonly TValue Current => list[index].value;
            readonly object IEnumerator.Current => Current;
    
            public readonly void Dispose() { }
    
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext() => ++index < list.Count;
    
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public void Reset() => index = -1;
    
            public readonly ValuesEnumerator GetEnumerator() => this;
            readonly IEnumerator IEnumerable.GetEnumerator() => this;
            readonly IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator() => this;
        }
    }
}