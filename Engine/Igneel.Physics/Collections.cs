using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Physics
{
    public class ReadOnlyDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        internal List<TValue> items = new List<TValue>();
        internal Dictionary<TKey, TValue> itemLookup = new Dictionary<TKey, TValue>();

        public bool ContainsKey(TKey key)
        {
            return itemLookup.ContainsKey(key);
        }

        public IEnumerable<TKey> Keys
        {
            get { return itemLookup.Keys; }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return itemLookup.TryGetValue(key, out value);
        }

        public IEnumerable<TValue> Values
        {
            get { return itemLookup.Values; }
        }

        public TValue this[TKey key]
        {
            get { return itemLookup[key]; }
        }

        public TValue this[int index]
        {
            get { return items[index]; }
        }

        public int Count
        {
            get { return items.Count; }
        }

        public List<TValue>.Enumerator GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator<KeyValuePair<TKey, TValue>> IEnumerable<KeyValuePair<TKey, TValue>>.GetEnumerator()
        {
            return itemLookup.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return itemLookup.GetEnumerator();
        }

        internal void Add(TKey key, TValue item)
        {
            itemLookup.Add(key, item);
            items.Add(item);
        }

        internal bool Remove(TKey key)
        {
            if (itemLookup.ContainsKey(key))
            {
                TValue v = itemLookup[key];
                itemLookup.Remove(key);
                items.Remove(v);
                return true;
            }
            return false;
        }
    }

    public class ReadOnlyList<T> : IReadOnlyList<T>
    {
        internal List<T> items = new List<T>();

        public T this[int index]
        {
            get { return items[index]; }
        }

        public int Count
        {
            get { return items.Count; }
        }

        public List<T>.Enumerator GetEnumerator()
        {
            return items.GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }
    }
}
