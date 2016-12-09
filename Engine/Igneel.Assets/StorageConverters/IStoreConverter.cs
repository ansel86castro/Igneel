using System.Reflection;

namespace Igneel.Assets.StorageConverters
{
    public interface IStoreConverter
    {
        object GetStorage(object resource, object propValue, PropertyInfo pi, ResourceOperationContext context);

        void SetStorage(object resource, object storeValue, PropertyInfo pi, ResourceOperationContext context); 
    }
}