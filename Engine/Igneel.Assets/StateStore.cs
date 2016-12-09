using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Assets
{
    [Serializable]
    public class StateStore
    {
        Dictionary<string, object> _properties = new Dictionary<string,object>();      
        Dictionary<string, StateStore> _subStores = new Dictionary<string,StateStore>();

        public StateStore(object target)
        {
            Capture(target);
        }

        public StateStore()
        {

        }

        public StateStore(Type type)
        {
            Capture(null, type, new HashSet<object>());
        }

        public void Capture(Object target)
        {
            Capture(target, target.GetType(), new HashSet<object>());
        }

        private void Capture(Object target, Type type ,HashSet<object> instances)
        {
            _properties.Clear();
            _subStores.Clear();

            if(target!=null)
                instances.Add(target);

            foreach (var item in type.GetProperties())
            {
                if (item.GetCustomAttributes(typeof(NonSerializedPropAttribute), true).Length == 0)
                {
                    if (item.GetIndexParameters().Length > 0 || !item.CanRead)
                        continue;

                    var value = item.GetValue(target);

                    if (item.CanWrite && (value == null || value.GetType().IsSerializable))
                    {
                        _properties.Add(item.Name, value);
                    }

                    else if (item.PropertyType.IsClass && value != null && !instances.Contains(value))
                    {
                        StateStore state = new StateStore();
                        state.Capture(value,value.GetType() ,instances);
                        _subStores.Add(item.Name, state);
                    }
                }
            }
        }

        public void Restore(object target)
        {
            Restore(target, target.GetType());
        }

        public void Restore(Object target, Type type)
        {
            foreach (var item in type.GetProperties())
            {
                object value;
                StateStore store;
                if (_properties.TryGetValue(item.Name, out value))
                {
                    item.SetValue(target, value);
                }
                else if (_subStores.TryGetValue(item.Name, out store))
                {
                    store.Restore(item.GetValue(target));
                }
            }
        }
    }
}
