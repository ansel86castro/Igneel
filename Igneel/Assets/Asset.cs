using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Igneel.Assets
{

    public enum StoreType { None, Reference, Asset };   

    public class AssetAttribute : Attribute
    {
        public AssetAttribute(AssetType type = AssetType.None, string extension = null)
        {
            Extension = extension;
            AssetType = type;
        }

        public string Extension { get; set; }
        public AssetType AssetType { get; set; }
    }
   
    [Serializable]
    public abstract class Asset:SerializableBase
    {      
        private string extension;
        Dictionary<string, object> members;

        public Asset(IAssetProvider provider):this(provider, null)
        {        

        }

        public Asset(IAssetProvider provider, string name)
        {
            Name = name;
            extension = null;
            AssetType = Assets.AssetType.None;

            var attr = (AssetAttribute[])GetType().GetCustomAttributes(typeof(AssetAttribute), true);
            if (attr.Length > 0)
            {
                extension = attr[0].Extension;
                AssetType = attr[0].AssetType;
            }

            RegisterAsset(provider);
        }

        protected virtual void RegisterAsset(IAssetProvider provider)
        {           
            if (provider != null)
                AssetManager.Instance._CreateAssetReference(this, provider);
        }

        public AssetManager Manager
        {
            get { return AssetManager.Instance; }
        }

        public int Id { get; internal set; }

        public string Name { get; set; }      

        public abstract IAssetProvider CreateProviderInstance();

        protected override void OnDeserialized(System.Runtime.Serialization.StreamingContext context)
        {
            Manager.LoadContext.RegisterAsset(this);
            base.OnDeserialized(context);
        }

        public string Extension
        {
            get
            {
                return extension;
            }
            protected set { extension = value; }
        }

        public AssetType AssetType { get; protected set; }

        public override string ToString()
        {
            if(Name!=null)
                return Name;
            return base.ToString();
        }

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

        public void GetMembers(IAssetProvider provider , Type type)
        {
            members = new Dictionary<string,object>();

            foreach (var prop in type.GetProperties())
            {
                if (prop.CanRead)
                {
                    var attr = prop.GetCustomAttribute<AssetMemberAttribute>(true);
                    if (attr != null)
                    {
                        var converter = attr.Converter;
                        if (converter != null)
                        {
                            var value = prop.GetValue(provider);
                            if (value != null)
                                members.Add(prop.Name, converter.GetStorage(provider, prop.GetValue(provider), prop));
                        }
                        else if (prop.CanWrite)
                        {
                            var value = prop.GetValue(provider);
                            switch (attr.StoreAs)
                            {
                                case StoreType.None:
                                    if (value is IAssetProvider)
                                    {
                                        var refe = Manager.GetAssetReference((IAssetProvider)value);
                                        members.Add(prop.Name, refe);
                                    }
                                    else if(prop.PropertyType.IsSerializable)
                                        members.Add(prop.Name, value);
                                    break;
                                case StoreType.Asset:
                                    if (value is IAssetProvider)
                                    {
                                        var asset = ((IAssetProvider)value).CreateAsset();
                                        members.Add(prop.Name, asset);
                                    }
                                    break;
                                case StoreType.Reference:
                                    if (value is IAssetProvider)
                                    {
                                        var refe = Manager.GetAssetReference((IAssetProvider)value);
                                        members.Add(prop.Name, refe);
                                    }
                                    break;
                            }                            
                        }
                    }
                }
            }

            if (members.Count == 0)
                members = null;
        }

        public void SetMembers(IAssetProvider provider, Type type)
        {
            if (members == null)
                return;
         
            foreach (var item in members)
            {
                var prop = type.GetProperty(item.Key);                
                 var attr = prop.GetCustomAttribute<AssetMemberAttribute>(true);
                 if (attr!=null)
                 {
                      var converter = attr.Converter;
                      if (converter != null)                      
                          converter.SetStorage(provider, item.Value, prop);                      
                      else
                      {
                          if (item.Value is Asset)
                          {
                              var value = Manager.GetAssetProvider((Asset)item.Value);
                              prop.SetValue(provider, value);
                          }
                          else if (item.Value is AssetReference)
                          {
                              var value = Manager.GetAssetProvider((AssetReference)item.Value);
                              prop.SetValue(provider, value);
                          }
                          else
                              prop.SetValue(provider, item.Value);
                      }
                 }

            }

        }

        public virtual void Delete() 
        {
            if (members == null || members.Count == 0) return;

            foreach (var item in members)
            {
                if (item.Value is Asset)
                {
                    ((Asset)item.Value).Delete();
                }
                else if (item.Value is AssetReference)
                {
                    Manager.DeleteFromContent((AssetReference)item.Value);
                }
            }
        }

        public static Asset Create(IAssetProvider provider, string name = null)
        {
            return new AutoAsset(provider, name);
        }
    }


    [Serializable]
    public sealed class AutoAsset : Asset
    {
        IProviderActivator activator;
        Type providerType;
        
        public AutoAsset(IAssetProvider provider, string name = null)
            : base(provider, name)
        {
            if (provider is IAssetProviderNotificator)
                ((IAssetProviderNotificator)provider).OnSavingBegin();

            var attr = (ProviderActivatorAttribute[])provider.GetType().GetCustomAttributes(typeof(ProviderActivatorAttribute), true);
            if (attr.Length > 0)
            {
                activator = attr[0].ActivatorInst;
                activator.Initialize(provider);
            }
            else
            {
                providerType = provider.GetType();
            }            
            
            GetMembers(provider, provider.GetType());

            if (provider is IAssetProviderNotificator)
                ((IAssetProviderNotificator)provider).OnSavingEnd();
        }

        public override IAssetProvider CreateProviderInstance()
        {
            IAssetProvider provider = activator != null ? activator.CreateInstance() : (IAssetProvider)Activator.CreateInstance(providerType);
            if (provider == null)
                return null;

            Manager.RegisterProvider(provider, Id);

            SetMembers(provider, provider.GetType());

            var attr = provider.GetType().GetCustomAttributes<OnCompleteAttribute>(true);
            if (attr != null)
            {
                foreach (var item in attr)
                {
                    item.InvokeComplete(provider);
                }
            }
            return provider;
        }

        protected override void RegisterAsset(IAssetProvider provider)
        {
            var attr = (AssetAttribute[])provider.GetType().GetCustomAttributes(typeof(AssetAttribute), true);
            if (attr.Length > 0)
            {
                Extension = attr[0].Extension;
                AssetType = attr[0].AssetType;
            }
            base.RegisterAsset(provider);
        }
    }

    [Serializable]
    public sealed class AutoAssetStore : AssetStorage
    {
         IProviderActivator activator;
        Type providerType;

        public AutoAssetStore(IAssetProvider provider, string name = null)
            : base(provider, name)
        {
            if (provider is IAssetProviderNotificator)
                ((IAssetProviderNotificator)provider).OnSavingBegin();

            var attr = (ProviderActivatorAttribute[])provider.GetType().GetCustomAttributes(typeof(ProviderActivatorAttribute), true);
            if (attr.Length > 0)
            {
                activator = attr[0].ActivatorInst;
                activator.Initialize(provider);
            }
            else
            {
                providerType = provider.GetType();
            }
            GetMembers(provider, provider.GetType());

            if (provider is IAssetProviderNotificator)
                ((IAssetProviderNotificator)provider).OnSavingEnd();
        }

        public override IAssetProvider CreateProviderInstance()
        {
            IAssetProvider provider = activator != null ? activator.CreateInstance() : (IAssetProvider)Activator.CreateInstance(providerType);
            if (provider == null)
                return null;

            Manager.RegisterProvider(provider, Id);

            SetMembers(provider, provider.GetType());

            var attr = (OnCompleteAttribute[])provider.GetType().GetCustomAttributes(typeof(OnCompleteAttribute), true);
            if (attr.Length > 0)
            {
                foreach (var item in attr)
                {
                    item.InvokeComplete(provider);
                }
            }
            return provider;
        }

        protected override void RegisterAsset(IAssetProvider provider)
        {
            var attr = (AssetAttribute[])provider.GetType().GetCustomAttributes(typeof(AssetAttribute), true);
            if (attr.Length > 0)
            {
                Extension = attr[0].Extension;
                AssetType = attr[0].AssetType;
            }
            base.RegisterAsset(provider);
        }
    }


    [Serializable]
    public sealed class StaticAsset : Asset
    {
        Type type;
        public StaticAsset(Type type)
            : base(null)
        {
            GetMembers(null, type);
            this.type = type;
        }

        public override IAssetProvider CreateProviderInstance()
        {
            SetMembers(null, type);
            return null;
        }
    }

    [Serializable]
    public sealed class StaticStoreAsset : AssetStorage
    {
        Type type;
        public StaticStoreAsset(Type type)
            : base(null)
        {
            GetMembers(null, type);
            this.type = type;
        }

        public override IAssetProvider CreateProviderInstance()
        {
            SetMembers(null, type);
            return null;
        }
    }

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
