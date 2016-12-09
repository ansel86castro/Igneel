using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Igneel.Assets
{
    [Serializable]
    public abstract class Asset:SerializableBase
    {      
      
        private Dictionary<string, object> _members;                                           

        public abstract object CreateResource(ResourceOperationContext context);              

        public void GetProperties(Dictionary<string, object> properties, object value)
        {
            Type type = value.GetType();
            var props = type.GetProperties();
            foreach (var p in props)
            {
                var attr = p.GetCustomAttributes(typeof(NonSerializedPropAttribute), false);
                if (attr.Length == 0 && p.CanRead && p.CanWrite && p.GetIndexParameters().Length == 0)
                {
                    properties.Add(p.Name,p.GetValue(value));   
                }
            }
        }

        public void SetProperties(Dictionary<string, object> properties, object value)
        {
            var props = value.GetType().GetProperties().ToDictionary(p => p.Name);
            foreach (var item in properties)
            {
                props[item.Key].SetValue(value, item.Value);
            }
        }

        public void GetMembers(object provider, Type type, ResourceOperationContext context)
        {
            _members = new Dictionary<string,object>();
            IReferenceManager referenceManager = context.ReferenceManager;
            if (referenceManager == null)
            {
                Debug.Assert(referenceManager != null, "Reference Manager in the context must not be null");                
            }

            foreach (var prop in type.GetProperties())
            {
                if (!prop.CanRead) continue;

                var attr = prop.GetCustomAttribute<AssetMemberAttribute>(true);
                if (attr == null) continue;

                var converter = attr.Converter;
                if (converter != null)
                {
                    var value = prop.GetValue(provider);
                    if (value != null)
                        _members.Add(prop.Name, converter.GetStorage(provider, prop.GetValue(provider), prop, context));
                }
                else if (prop.CanWrite)
                {
                    var value = prop.GetValue(provider);                    
                    switch (attr.StoreAs)
                    {
                        case StoreType.None:
                            {
                                if (prop.PropertyType.IsSerializable)
                                    _members.Add(prop.Name, value);
                                else
                                {
                                    var resource = value as IResource;
                                    if (resource != null)
                                    {
                                        var refe = referenceManager.GetReference(resource, context);
                                        _members.Add(prop.Name, refe);
                                    }
                                }
                                break;
                            }
                        case StoreType.Asset:
                            {
                                var persistable = value as IPersistable;
                                if (persistable != null)
                                {
                                    var asset = persistable.CreateAsset(context);
                                    _members.Add(prop.Name, asset);
                                }
                                break;
                            }
                        case StoreType.Reference:
                            {
                                var resource = value as IResource;
                                if (resource != null)
                                {
                                    var refe = referenceManager.GetReference(resource, context);
                                    _members.Add(prop.Name, refe);
                                }
                                break;
                            }
                    }                            
                }
            }

            if (_members.Count == 0)
                _members = null;
        }

        public void SetMembers(object provider, Type type, ResourceOperationContext context)
        {
            if (_members == null)
                return;

            IReferenceManager referenceManager = context.ReferenceManager;
            if (referenceManager == null)
            {
                Debug.Assert(referenceManager != null, "Reference Manager in the context must not be null");
            }

            foreach (var item in _members)
            {
                var prop = type.GetProperty(item.Key);                
                 var attr = prop.GetCustomAttribute<AssetMemberAttribute>(true);
                if (attr == null) continue;

                var converter = attr.Converter;
                if (converter != null)                      
                    converter.SetStorage(provider, item.Value, prop, context);
                else
                {
                    var asset = item.Value as Asset;
                    if (asset != null)
                    {                              
                        prop.SetValue(provider, asset.CreateResource(context));
                    }
                    else
                    {
                        ResourceReference reference = item.Value as ResourceReference;
                        if (reference != null)
                        {
                            var value = referenceManager.GetResource(reference, context);
                            prop.SetValue(provider, value);
                        }
                        else
                            prop.SetValue(provider, item.Value);
                    }
                }
            }

        }
        
        public static Asset Create(object provider, ResourceOperationContext context)
        {
            return new GenericAsset(provider, context);
        }
    }


    //[Serializable]
    //public sealed class AutoAssetStore : AssetStorage
    //{
    //     IResourceActivator activator;
    //    Type providerType;

    //    public AutoAssetStore(IAssetProvider provider, string name = null)
    //        : base(provider, name)
    //    {
    //        if (provider is IAssetProviderNotificator)
    //            ((IAssetProviderNotificator)provider).OnSavingBegin();

    //        var attr = (ProviderActivatorAttribute[])provider.GetType().GetCustomAttributes(typeof(ProviderActivatorAttribute), true);
    //        if (attr.Length > 0)
    //        {
    //            activator = attr[0].ActivatorInst;
    //            activator.Initialize(provider);
    //        }
    //        else
    //        {
    //            providerType = provider.GetType();
    //        }
    //        GetMembers(provider, provider.GetType());

    //        if (provider is IAssetProviderNotificator)
    //            ((IAssetProviderNotificator)provider).OnSavingEnd();
    //    }

    //    public override IAssetProvider CreateResource()
    //    {
    //        IAssetProvider provider = activator != null ? activator.CreateInstance() : (IAssetProvider)Activator.CreateInstance(providerType);
    //        if (provider == null)
    //            return null;

    //        Manager.RegisterProvider(provider, Id);

    //        SetMembers(provider, provider.GetType());

    //        var attr = (OnCompleteAttribute[])provider.GetType().GetCustomAttributes(typeof(OnCompleteAttribute), true);
    //        if (attr.Length > 0)
    //        {
    //            foreach (var item in attr)
    //            {
    //                item.InvokeComplete(provider);
    //            }
    //        }
    //        return provider;
    //    }

    //    protected override void RegisterAsset(IAssetProvider provider)
    //    {
    //        var attr = (AssetAttribute[])provider.GetType().GetCustomAttributes(typeof(AssetAttribute), true);
    //        if (attr.Length > 0)
    //        {
    //            Extension = attr[0].Extension;
    //            AssetType = attr[0].AssetType;
    //        }
    //        base.RegisterAsset(provider);
    //    }
    //}


    //[Serializable]
    //public sealed class StaticAsset : Asset
    //{
    //    Type type;
    //    public StaticAsset(Type type)
    //        : base(null)
    //    {
    //        GetMembers(null, type);
    //        this.type = type;
    //    }

    //    public override IAssetProvider CreateProviderInstance()
    //    {
    //        SetMembers(null, type);
    //        return null;
    //    }
    //}

    //[Serializable]
    //public sealed class StaticStoreAsset : AssetStorage
    //{
    //    Type type;
    //    public StaticStoreAsset(Type type)
    //        : base(null)
    //    {
    //        GetMembers(null, type);
    //        this.type = type;
    //    }

    //    public override IAssetProvider CreateProviderInstance()
    //    {
    //        SetMembers(null, type);
    //        return null;
    //    }
    //}

    //public abstract class ExtendedAsset : Asset
    //{
    //    Dictionary<string, object> properties;

    //     public ExtendedAsset(IAssetProvider provider):base(provider)
    //     {
    //         properties = new Dictionary<string, object>();
    //         GetProperties(properties, provider);
    //     }

    //     public ExtendedAsset(IAssetProvider provider, string name) : base(provider, name) 
    //     {
    //         properties = new Dictionary<string, object>();
    //         GetProperties(properties, provider);
    //     }

    //     public override IAssetProvider CreateProviderInstance()
    //     {
    //         var inst = _CreateInstance();
    //         InitializeProvider(inst);
    //         return inst;

    //     }

    //     public virtual void InitializeProvider(IAssetProvider provider)
    //     {
    //         var props = provider.GetType().GetProperties().ToDictionary(p => p.Name);
    //         foreach (var item in properties)
    //         {
    //             props[item.Key].SetValue(provider, item.Value);
    //         }
    //     }

    //     protected abstract IAssetProvider _CreateInstance();
    //}
  
}
