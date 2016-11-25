using System;
using System.Reflection;

namespace Igneel.Assets
{
    [Serializable]
    public sealed class GenericAsset : Asset
    {
        readonly IResourceActivator _activator;
        readonly Type _providerType;

        public GenericAsset(object provider, ResourceOperationContext context)            
        {         
            ResourceActivatorAttribute attr = provider.GetType().GetCustomAttribute<ResourceActivatorAttribute>(true);
            if (attr != null)
            {
                _activator = (IResourceActivator) Activator.CreateInstance(attr.ActivatorType);
                _activator.OnAssetCreated(provider, context);
            }
            else
            {
                _providerType = provider.GetType();
            }            
            
            GetMembers(provider, provider.GetType(), context);
        }

        public override object CreateResource(ResourceOperationContext context)
        {
            var provider = _activator != null
                ? _activator.OnCreateResource(context)
                : Activator.CreateInstance(_providerType);

            if (provider == null)
                return null;

            SetMembers(provider, provider.GetType(), context);

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
    }
}