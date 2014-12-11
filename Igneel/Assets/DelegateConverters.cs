using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Assets
{
    [Serializable]
    struct _Store
    {
        public MethodInfo Method;
        public AssetReference TargetRef;
        public object Target;
    }

    public class ActionConverter<T> : IStoreConverter
    {
        public object GetStorage(IAssetProvider provider, object propValue, System.Reflection.PropertyInfo pi)
        {
            Action<T> del = (Action<T>)propValue;
            _Store st = new _Store();
            st.Method = del.Method;
            if (del.Target is IAssetProvider)
            {
                st.TargetRef = AssetManager.Instance.GetAssetReference((IAssetProvider)del.Target);
            }
            else
                st.Target = del.Target;

            return st;
        }

        public void SetStorage(IAssetProvider provider, object storeValue, System.Reflection.PropertyInfo pi)
        {
            _Store st = (_Store)storeValue;
            object target = st.TargetRef != null ? AssetManager.Instance.GetAssetProvider(st.TargetRef) : null;
            if (st.TargetRef != null)
            {
                var action = Delegate.CreateDelegate(typeof(Action<T>), target, st.Method);
                pi.SetValue(provider, action);
            }
            else if (st.Target != null)
            {
                var action = Delegate.CreateDelegate(typeof(Action<T>), st.Target, st.Method);
                pi.SetValue(provider, action);
            }
        }
    }

    public class ActionConverter<T1, T2> : IStoreConverter
    {       
        public object GetStorage(IAssetProvider provider, object propValue, System.Reflection.PropertyInfo pi)
        {
            Action<T1, T2> del = (Action<T1, T2>)propValue;
            _Store st = new _Store();
            st.Method = del.Method;
            if (del.Target is IAssetProvider)
            {
                st.TargetRef = AssetManager.Instance.GetAssetReference((IAssetProvider)del.Target);
            }
            else
            {
                st.Target = del.Target;
            }
            return st;
        }

        public void SetStorage(IAssetProvider provider, object storeValue, System.Reflection.PropertyInfo pi)
        {
            _Store st = (_Store)storeValue;
            object target = st.TargetRef != null ? AssetManager.Instance.GetAssetProvider(st.TargetRef) : null;
            if (st.TargetRef != null)
            {
                var action = Delegate.CreateDelegate(typeof(Action<T1, T2>), target, st.Method);
                pi.SetValue(provider, action);
            }
            else if (st.Target != null)
            {
                var action = Delegate.CreateDelegate(typeof(Action<T1, T2>), st.Target, st.Method);
                pi.SetValue(provider, action);
            }
        }
    }

    public class DelegateConverter : IStoreConverter
    {
        [Serializable]
        struct _Store
        {
            public Type DelType;
            public MethodInfo Method;
            public AssetReference TargetRef;
            public object Target;
            public Tuple<MethodInfo, AssetReference>[] InvocationList;
        }

        public object GetStorage(IAssetProvider provider, object propValue, System.Reflection.PropertyInfo pi)
        {
            Delegate del = (Delegate)propValue;
            _Store st = new _Store();
            st.Method = del.Method;
            st.DelType = del.GetType();

            if (del.Target != null)
            {
                if (del.Target is IAssetProvider)
                {
                    st.TargetRef = AssetManager.Instance.GetAssetReference((IAssetProvider)del.Target);
                }
                else if (del.Target.GetType().IsSerializable)
                {
                    st.Target = del.Target;
                }
            }

            var invoList = del.GetInvocationList();
            if (invoList.Length > 1)
            {
                st.InvocationList = new Tuple<MethodInfo, AssetReference>[invoList.Length];
                for (int i = 0; i < invoList.Length; i++)
                {
                    Tuple<MethodInfo, AssetReference> t = new Tuple<MethodInfo, AssetReference>(
                        invoList[i].Method, invoList[i].Target is IAssetProvider ? AssetManager.Instance.GetAssetReference((IAssetProvider)invoList[i].Target) : null);
                    st.InvocationList[i] = t;
                }
            }
            return st;
        }

        public void SetStorage(IAssetProvider provider, object storeValue, System.Reflection.PropertyInfo pi)
        {
            _Store st = (_Store)storeValue;
            object target = st.TargetRef != null ? AssetManager.Instance.GetAssetProvider(st.TargetRef) :
                            st.Target != null ? st.Target : null;

            Delegate del = null;
            del = Delegate.CreateDelegate(st.DelType, target, st.Method);

            if (st.InvocationList != null && st.InvocationList.Length > 2)
            {
                del = st.InvocationList.Select(x => Delegate.CreateDelegate(st.DelType, x.Item2 != null ? AssetManager.Instance.GetAssetProvider(x.Item2) : null, x.Item1))
                                 .Aggregate((d1, d2) => Delegate.Combine(d1, d2));
            }

            pi.SetValue(provider, del);
        }
    }
}
