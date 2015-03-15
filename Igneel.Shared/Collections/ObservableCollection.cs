using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Collections
{
    public class ObservedDictionary<TKey, TValue> : ICollection<TValue>
    {
        List<TValue> items;
        Dictionary<TKey, TValue> itemLookup;

        Action<TValue> itemAdded;
        Action<TValue> itemRemoved;
        Func<TValue, TKey> keySelector;

        public ObservedDictionary(Action<TValue> itemAdded, Action<TValue> itemRemoved, Func<TValue, TKey> keySelector)
        {
            this.itemAdded = itemAdded;
            this.itemRemoved = itemRemoved;
            this.keySelector = keySelector;

            items = new List<TValue>();
            itemLookup = new Dictionary<TKey, TValue>();
        }

        public ObservedDictionary(int capacity, Action<TValue> itemAdded, Action<TValue> itemRemoved, Func<TValue, TKey> keySelector)
        {
            this.itemAdded = itemAdded;
            this.itemRemoved = itemRemoved;
            this.keySelector = keySelector;

            items = new List<TValue>(capacity);
            itemLookup = new Dictionary<TKey, TValue>(capacity);
        }

        public TValue this[int index]
        {
            get { return items[index]; }
        }

        public TValue this[TKey key]
        {
            get { return itemLookup[key]; }
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return itemLookup.TryGetValue(key, out value);
        }

        public void Add(TValue item)
        { 
            var key = keySelector(item);
            if (itemLookup.ContainsKey(key))
                return;

            items.Add(item);            
            itemLookup.Add(key, item);
            if (itemAdded != null)
                itemAdded(item);
        }

        public void Clear()
        {
            items.Clear();
            itemLookup.Clear();
        }

        public bool Contains(TValue item)
        {
            return itemLookup.ContainsKey(keySelector(item));
        }

        public bool ContainsKey(TKey key)
        {
            return itemLookup.ContainsKey(key);
        }

        public void CopyTo(TValue[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return items.Count; }
        }

        public int IndexOf(TValue item)
        {
            return items.IndexOf(item);
        }

        public void RemoveAt(int index)
        {
            var item = items[index];
            items.RemoveAt(index);
            var key = keySelector(item);
            itemLookup.Remove(key);

            if (itemRemoved != null)
                itemRemoved(item);
        }

        public void AddRange(IEnumerable<TValue> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public void RemoveRange(IEnumerable<TValue> items)
        {
            foreach (var item in items)
            {
                Remove(item);
            }
        }

        bool ICollection<TValue>.IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(TValue item)
        {
            var key = keySelector(item);
            if (itemLookup.Remove(key))
            {
                items.Remove(item);
                if (itemRemoved != null)
                    itemRemoved(item);
                return true;
            }
            return false;
        }

        public void ChangeKey(TKey oldKey, TKey newKey)
        {
            var item = itemLookup[oldKey];
            itemLookup.Remove(oldKey);
            itemLookup.Add(newKey, item);
        }

        IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
        {
           return items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }

        public List<TValue>.Enumerator GetEnumerator()
        {
            return items.GetEnumerator();
        }
    }

    public class ObservedCollection<TValue> : ICollection<TValue>
    {
        List<TValue> items;     
        Action<TValue> itemAdded;
        Action<TValue ,int> itemRemoved;

        public ObservedCollection(Action<TValue> itemAdded, Action<TValue, int> itemRemoved)
        {
            this.itemAdded = itemAdded;
            this.itemRemoved = itemRemoved;            

            items = new List<TValue>();            
        }

        public ObservedCollection(int capacity, Action<TValue> itemAdded, Action<TValue, int> itemRemoved)
        {
            this.itemAdded = itemAdded;
            this.itemRemoved = itemRemoved;          

            items = new List<TValue>(capacity);            
        }

        public TValue this[int index]
        {
            get { return items[index]; }
        }     

        public void Add(TValue item)
        {
            items.Add(item);           
            if (itemAdded != null)
                itemAdded(item);
        }

        public void Clear()
        {
            if (itemRemoved != null)
            {
                for (int i = 0; i < items.Count; i++)
                {
                    itemRemoved(items[i], i);
                }
            }
            items.Clear();           
        }

        public bool Contains(TValue item)
        {
            return items.Contains(item);
        }       

        public void CopyTo(TValue[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return items.Count; }
        }

        public int IndexOf(TValue item)
        {
            return items.IndexOf(item);
        }

        public void RemoveAt(int index)
        {
            var item = items[index];
            items.RemoveAt(index);          
            if (itemRemoved != null)
                itemRemoved(item, index);
        }

        public void AddRange(IEnumerable<TValue> items)
        {
            foreach (var item in items)
            {
                Add(item);
            }
        }

        public void RemoveRange(IEnumerable<TValue> items)
        {
            foreach (var item in items)
            {
                Remove(item);
            }
        }

        bool ICollection<TValue>.IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(TValue item)
        {
            int index = items.IndexOf(item);
            if (index>=0)
            {             
                items.RemoveAt(index);
                if (itemRemoved != null)
                    itemRemoved(item, index);
                return true;
            }
            return false;
        }       

        IEnumerator<TValue> IEnumerable<TValue>.GetEnumerator()
        {
            return items.GetEnumerator();
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return items.GetEnumerator();
        }

        public List<TValue>.Enumerator GetEnumerator()
        {
            return items.GetEnumerator();
        }
    }

    public class ReadOnlyDictionary<TKey, TValue> : IReadOnlyDictionary<TKey, TValue>
    {
        internal List<TValue> items = new List<TValue>();
        internal Dictionary<TKey, TValue> itemLookup = new Dictionary<TKey,TValue>();

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
        internal List<T> items;

        public ReadOnlyList()
        {
            items = new List<T>();
        }

        public ReadOnlyList(List<T> items)
        {
            this.items = items;
        }

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
