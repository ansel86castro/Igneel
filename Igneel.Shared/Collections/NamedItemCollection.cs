using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.ObjectModel;

namespace Igneel.Collections
{
    public class NamedItemCollection<T> : IList<T>
        where T:INameable
    {
        List<T> items = new List<T>();
        Dictionary<string, int> lookup = new Dictionary<string,int>();

        public int IndexOf(T item)
        {
            return lookup[item.Name];
        }

        public void Insert(int index, T item)
        {            
            lookup[item.Name] = index;
            items.Insert(index, item);
            HookNameChanged(item);
        }

        public void RemoveAt(int index)
        {
            T obj = items[index];
            lookup.Remove(obj.Name);
            items.RemoveAt(index);
            UnHookNameChanged(obj);
        }

        public T this[int index]
        {
            get
            {
                return items[index];
            }
            set
            {
                var item = items[index];
                lookup.Remove(item.Name);
                UnHookNameChanged(item);

                items[index] = value;
                lookup[item.Name] = index;
                HookNameChanged(value);
            }
        }

        public T this[string name]
        {
            get
            {
                return items[lookup[name]];
            }
        }

        public void Add(T item)
        {
            lookup[item.Name] = items.Count;
            items.Add(item);
            HookNameChanged(item);
        }

        public void Clear()
        {
            foreach (var item in items)            
                UnHookNameChanged(item);
            
            items.Clear();
            lookup.Clear();
        }

        public bool Contains(T item)
        {
            return lookup.ContainsKey(item.Name);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            items.CopyTo(array, arrayIndex);
        }

        public int Count
        {
            get { return items.Count; }
        }

        bool ICollection<T>.IsReadOnly
        {
            get { return false; }
        }

        public bool Remove(T item)
        {            
            int index;
            if (lookup.TryGetValue(item.Name, out index))
            {
                UnHookNameChanged(item);
                lookup.Remove(item.Name);
                items.RemoveAt(index);
                return true;
            }
            return false;
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

        private void HookNameChanged(T item)
        {
            if (item is INameChangingNotificator)
            {
                ((INameChangingNotificator)item).NameChanged += KeyObjectCollection_NameChanged;
            }
        }

        private void UnHookNameChanged(T item)
        {
            if (item is INameChangingNotificator)
            {
                ((INameChangingNotificator)item).NameChanged -= KeyObjectCollection_NameChanged;
            }
        }

        void KeyObjectCollection_NameChanged(object obj, string newName)
        {
            T item = (T)obj;
            int index = lookup[item.Name];
            lookup.Remove(item.Name);
            lookup.Add(newName, index);
        }
      
    }

    public class NamedCollection<T> : KeyedCollection<string, T>
        where T:INameable
    {
        
        protected override string GetKeyForItem(T item)
        {
            return item.Name;
        }
    }
}
