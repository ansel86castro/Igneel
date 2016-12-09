using System;
using System.Diagnostics;
using System.IO.IsolatedStorage;
using System.Linq;
using System.Reflection;

namespace Igneel.Assets.StorageConverters
{
    //[Serializable]
    //struct Store
    //{
    //    public MethodInfo Method;
    //    public ResourceReference TargetRef;
    //    public object Target;
    //}

    //public class ActionConverter<T> : IStoreConverter
    //{
    //    public object GetStorage(IResource provider, object propValue, System.Reflection.PropertyInfo pi, ResourceOperationContext context)
    //    {
    //        Action<T> del = (Action<T>)propValue;
    //        Store st = new Store();
    //        st.Method = del.Method;
    //        if (del.Target is IResource)
    //        {
    //            var referenceManager = context.ReferenceManager;
    //            if (referenceManager == null)
    //                Debug.Assert(referenceManager != null);

    //            st.TargetRef = referenceManager.GetReference((IResource)del.Target, context);
    //        }
    //        else
    //            st.Target = del.Target;

    //        if (st.Target != null)
    //        {
    //            Debug.Assert(st.Target.GetType().IsSerializable, "Target value not serializable");
    //        }
    //        return st;
    //    }

    //    public void SetStorage(IResource provider, object storeValue, System.Reflection.PropertyInfo pi, ResourceOperationContext context)
    //    {
    //        Store st = (Store)storeValue;           
    //        object target = st.Target;
    //        if (st.TargetRef != null)
    //        {
    //            var referenceManager = context.ReferenceManager;
    //            if (referenceManager == null)
    //                Debug.Assert(referenceManager != null);
    //            target = referenceManager.GetResource(st.TargetRef, context);
    //        }

    //        if(target!=null)
    //        {
    //            var action = Delegate.CreateDelegate(typeof(Action<T>), target, st.Method);
    //            pi.SetValue(provider, action);
    //        }            
    //    }
    //}

    //public class ActionConverter<T1, T2> : IStoreConverter
    //{
    //    public object GetStorage(IResource provider, object propValue, System.Reflection.PropertyInfo pi, ResourceOperationContext context)
    //    {
    //        Action<T1, T2> del = (Action<T1, T2>)propValue;
    //        Store st = new Store();
    //        st.Method = del.Method;
    //        if (del.Target is IResource)
    //        {
    //            var referenceManager = context.ReferenceManager;
    //            if (referenceManager == null)
    //                Debug.Assert(referenceManager != null);

    //            st.TargetRef = referenceManager.GetReference((IResource)del.Target, context);
    //        }
    //        else
    //        {
    //            st.Target = del.Target;
    //        }
    //        return st;
    //    }

    //    public void SetStorage(IResource provider, object storeValue, System.Reflection.PropertyInfo pi, ResourceOperationContext context)
    //    {
    //        Store st = (Store)storeValue;
    //        var referenceManager = context.ReferenceManager;
    //        if (referenceManager == null)
    //            Debug.Assert(referenceManager != null);

    //        object target = st.TargetRef != null ? referenceManager.GetResource(st.TargetRef, context) : null;
    //        if (st.TargetRef != null)
    //        {
    //            var action = Delegate.CreateDelegate(typeof(Action<T1, T2>), target, st.Method);
    //            pi.SetValue(provider, action);
    //        }
    //        else if (st.Target != null)
    //        {
    //            var action = Delegate.CreateDelegate(typeof(Action<T1, T2>), st.Target, st.Method);
    //            pi.SetValue(provider, action);
    //        }
    //    }
    //}

    //public class DelegateConverter : IStoreConverter
    //{
    //    [Serializable]
    //    struct Store
    //    {
    //        public Type DelType;
    //        public MethodInfo Method;
    //        public ResourceReference TargetRef;
    //        public object Target;
    //        public Tuple<MethodInfo, ResourceReference>[] InvocationList;
    //    }

    //    public object GetStorage(IResource provider, object propValue, System.Reflection.PropertyInfo pi, ResourceOperationContext context)
    //    {
    //        Delegate del = (Delegate)propValue;
    //        Store st = new Store();
    //        st.Method = del.Method;
    //        st.DelType = del.GetType();

    //        var referenceManager = context.ReferenceManager;
    //        if (referenceManager == null)
    //            Debug.Assert(referenceManager != null);

    //        if (del.Target != null)
    //        {
    //            if (del.Target is IResource)
    //            {
    //                st.TargetRef = referenceManager.GetReference((IResource)del.Target, context);
    //            }
    //            else if (del.Target.GetType().IsSerializable)
    //            {
    //                st.Target = del.Target;
    //            }
    //        }

    //        var invoList = del.GetInvocationList();
    //        if (invoList.Length > 1)
    //        {
    //            st.InvocationList = new Tuple<MethodInfo, ResourceReference>[invoList.Length];
    //            for (int i = 0; i < invoList.Length; i++)
    //            {
    //                Tuple<MethodInfo, ResourceReference> t = new Tuple<MethodInfo, ResourceReference>(
    //                    invoList[i].Method, invoList[i].Target is IResource ?  
    //                    referenceManager.GetReference((IResource)invoList[i].Target, context) : null);
    //                st.InvocationList[i] = t;
    //            }
    //        }
    //        return st;
    //    }

    //    public void SetStorage(IResource provider, object storeValue, System.Reflection.PropertyInfo pi, ResourceOperationContext context)
    //    {
    //        var referenceManager = context.ReferenceManager;
    //        if (referenceManager == null)
    //            Debug.Assert(referenceManager != null);

    //        Store st = (Store)storeValue;
    //        object target = st.TargetRef != null ? referenceManager.GetResource(st.TargetRef, context) :
    //                        st.Target ?? null;

    //        Delegate del = null;
    //        del = Delegate.CreateDelegate(st.DelType, target, st.Method);

    //        if (st.InvocationList != null && st.InvocationList.Length > 2)
    //        {
    //            del = st.InvocationList.Select(x => Delegate.CreateDelegate(st.DelType, 
    //                (object) (x.Item2 != null ? referenceManager.GetResource(x.Item2, context) : null), (MethodInfo) x.Item1))
    //                             .Aggregate((d1, d2) => Delegate.Combine(d1, d2));
    //        }

    //        pi.SetValue(provider, del);
    //    }
    //}
}
