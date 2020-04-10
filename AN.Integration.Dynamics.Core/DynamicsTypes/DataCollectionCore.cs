using System.Collections;
using System.Collections.Generic;

namespace AN.Integration.Dynamics.Core.DynamicsTypes
{
    public abstract class DataCollectionCore<TKey, TValue> : IEnumerable<KeyValuePair<TKey, TValue>>
    {
        private IDictionary<TKey, TValue> _innerDictionary = new Dictionary<TKey, TValue>();

        protected internal DataCollectionCore()
        {
        }

        public void Add(KeyValuePair<TKey, TValue> item) =>
            _innerDictionary.Add(item);

        public void AddRange(params KeyValuePair<TKey, TValue>[] items)
        {
            this.AddRange((IEnumerable<KeyValuePair<TKey, TValue>>) items);
        }

        public void AddRange(IEnumerable<KeyValuePair<TKey, TValue>> items)
        {
            if (items is null)
                return;
            var innerDictionary = (ICollection<KeyValuePair<TKey, TValue>>) _innerDictionary;
            foreach (var keyValuePair in items)
                innerDictionary.Add(keyValuePair);
        }

        public void Add(TKey key, TValue value) =>
            _innerDictionary.Add(key, value);

        public virtual TValue this[TKey key]
        {
            get => _innerDictionary[key];
            set => _innerDictionary[key] = value;
        }

        public void Clear() => _innerDictionary.Clear();

        public bool Contains(TKey key) => _innerDictionary.ContainsKey(key);

        public bool Contains(KeyValuePair<TKey, TValue> key) => _innerDictionary.Contains(key);

        public bool TryGetValue(TKey key, out TValue value) => _innerDictionary.TryGetValue(key, out value);

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex) =>
            _innerDictionary.CopyTo(array, arrayIndex);

        public bool ContainsKey(TKey key) => _innerDictionary.ContainsKey(key);

        public bool Remove(TKey key) => _innerDictionary.Remove(key);

        public bool Remove(KeyValuePair<TKey, TValue> item) => _innerDictionary.Remove(item);

        public int Count => _innerDictionary.Count;

        public ICollection<TKey> Keys => _innerDictionary.Keys;

        public ICollection<TValue> Values => _innerDictionary.Values;

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => _innerDictionary.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _innerDictionary.GetEnumerator();
    }
}