using System;
using Igneel.Assets.StorageConverters;

namespace Igneel.Assets
{
    public class AssetMemberAttribute : Attribute
    {
        private IStoreConverter _storage;

        public Type ConverterType { get; set; }

        public StoreType StoreAs { get; set; }

        public AssetMemberAttribute()
        {
            StoreAs = StoreType.None;
        }

        public AssetMemberAttribute(StoreType storeAs)
        {
            StoreAs = storeAs;
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

    //public interface IStoreConverter<T> : IStoreConverter
    //{
                
    //}

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
}
