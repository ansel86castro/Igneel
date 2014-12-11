using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Igneel.Assets
{
    public class AssetMemberAttribute : Attribute
    {
        private IStoreConverter _storage;

        public Type ConverterType { get; set; }

        public StoreType StoreAs { get; set; }

        public AssetMemberAttribute()
        {
            this.StoreAs = StoreType.None;
        }

        public AssetMemberAttribute(StoreType storeAs)
        {
            this.StoreAs = storeAs;
        }

        public AssetMemberAttribute(Type converter)
        {
            this.ConverterType = converter;
        }
       

        public IStoreConverter Converter
        {
            get
            {
                if (_storage != null)
                    return _storage;                
                else if (ConverterType != null)
                {
                   return _storage = (IStoreConverter)Activator.CreateInstance(ConverterType);
                }
                else
                    return null;
            }
        }        

    }

    public class ProviderActivatorAttribute : Attribute
    {
        private IProviderActivator _activator;        

        public ProviderActivatorAttribute(Type type)
        {
            ActivatorType = type;
        }

        public Type ActivatorType { get; set; }
        

        public IProviderActivator ActivatorInst
        {
            get
            {
                return _activator ?? (_activator = (IProviderActivator)Activator.CreateInstance(ActivatorType));
            }
        }
       
    }

    public class OnCompleteAttribute : Attribute
    {
        public string OnComplete { get; set; }        

        public OnCompleteAttribute(string methodName)
        {
            OnComplete = methodName;
        }

        public void InvokeComplete(object target)
        {
            if (OnComplete == null) return;

            var method = target.GetType().GetMethod(OnComplete);

            method.Invoke(target, null);
        }

    }

    public interface IProviderActivator
    {
        void Initialize(IAssetProvider provider);

        IAssetProvider CreateInstance();
    }

    public interface IStoreConverter
    {
        object GetStorage(IAssetProvider provider, object propValue, System.Reflection.PropertyInfo pi);

        void SetStorage(IAssetProvider provider, object storeValue, System.Reflection.PropertyInfo pi); 
    }

    public interface IStoreConverter<T> : IStoreConverter
    {
                
    }

    //public class CollectionConverter : IStoreConverter
    //{
    //    public object GetStorage(IAssetProvider provider, object value, System.Reflection.PropertyInfo pi)
    //    {
    //        ICollection collection = (ICollection)value;
    //        AssetReference[] references = new AssetReference[collection.Count];
    //        int i = 0;
    //        var am = AssetManager.Instance;
    //        foreach (var item in collection)
    //        {
    //            if(item!=null && item is IAssetProvider)
    //                references[i++] = am.GetAssetReference((IAssetProvider)item);
    //        }

    //        return references;
    //    }

    //    public void SetStorage(IAssetProvider provider, object storeValue, System.Reflection.PropertyInfo pi)
    //    {
    //        AssetReference[] references = (AssetReference[])storeValue;
    //        dynamic collection = pi.GetValue(provider);
    //        var am = AssetManager.Instance;
    //        foreach (var refe in references)
    //        {
    //            if (refe != null)
    //                collection.Add(am.GetAssetProvider(refe));
    //        }
    //    }
    //}

    public class CollectionStoreConverter<T> : IStoreConverter<T>       
    {
        public object GetStorage(IAssetProvider provider, object propValue, System.Reflection.PropertyInfo pi)
        {
            ICollection<T> collection = (ICollection<T>)propValue;
            AssetReference[] references = new AssetReference[collection.Count];
            int i = 0;
            var am = AssetManager.Instance;
            foreach (var item in collection)
            {
                if (item != null && item is IAssetProvider)
                    references[i++] = am.GetAssetReference((IAssetProvider)item);
            }

            return references;
        }

        public void SetStorage(IAssetProvider provider, object storeValue, System.Reflection.PropertyInfo pi)
        {
            AssetReference[] references = (AssetReference[])storeValue;
            ICollection<T> collection = (ICollection<T>)pi.GetValue(provider);
            var am = AssetManager.Instance;
            foreach (var refe in references)
            {
                if (refe != null)
                    collection.Add((T)am.GetAssetProvider(refe));
            }
        }
    }

    public class ArrayStoreConverter<T> : IStoreConverter<T>     
        where T:class
    {
        public object GetStorage(IAssetProvider provider, object value, System.Reflection.PropertyInfo pi)
        {
            T[] collection = (T[])value;            
            var references = new AssetReference[collection.Length];            
            int i = 0;
            var am = AssetManager.Instance;
            foreach (var item in collection)
            {
                if (item != null && item is IAssetProvider)
                    references[i++] = am.GetAssetReference((IAssetProvider)item);
            }

            return references;
        }

        public void SetStorage(IAssetProvider provider, object storeValue, System.Reflection.PropertyInfo pi)
        {
            AssetReference[] referenes = (AssetReference[])storeValue;
            T[] array = new T[referenes.Length];
            var am = AssetManager.Instance;

            for (int i = 0; i < referenes.Length; i++)
            {
                if (referenes[i] != null)
                    array[i] = (T)am.GetAssetProvider(referenes[i]);
            }

            array = array.Where(x=> x != null).ToArray();
            pi.SetValue(provider, array);
        }
    }
    
}
