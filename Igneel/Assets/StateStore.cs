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
        Dictionary<string, object> properties = new Dictionary<string,object>();      
        Dictionary<string, StateStore> subStores = new Dictionary<string,StateStore>();

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
            properties.Clear();
            subStores.Clear();

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
                        properties.Add(item.Name, value);
                    }

                    else if (item.PropertyType.IsClass && value != null && !instances.Contains(value))
                    {
                        StateStore state = new StateStore();
                        state.Capture(value,value.GetType() ,instances);
                        subStores.Add(item.Name, state);
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
                if (properties.TryGetValue(item.Name, out value))
                {
                    item.SetValue(target, value);
                }
                else if (subStores.TryGetValue(item.Name, out store))
                {
                    store.Restore(item.GetValue(target));
                }
            }
        }
    }


    public class StateStoreConverter : IStoreConverter
    {
        public object GetStorage(IAssetProvider provider, object propValue, System.Reflection.PropertyInfo pi)
        {            
            StateStore state = new StateStore(propValue);
            return state;
        }

        public void SetStorage(IAssetProvider provider, object storeValue, System.Reflection.PropertyInfo pi)
        {
            StateStore state = (StateStore)storeValue;
            var obj = pi.GetValue(provider);
            state.Restore(obj);
        }
    }
}
