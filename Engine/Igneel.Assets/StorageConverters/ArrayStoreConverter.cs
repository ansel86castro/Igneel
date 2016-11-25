using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace Igneel.Assets.StorageConverters
{
    public class ArrayStoreConverter<T> : IStoreConverter
        where T : class, IResource
    {
        public object GetStorage(object provider, object value, PropertyInfo pi, ResourceOperationContext context)
        {
            T[] collection = (T[]) value;
            var references = new ResourceReference[collection.Length];
            int i = 0;
            var referenceManager = context.ReferenceManager;
            if (referenceManager == null)
                Debug.Assert(referenceManager != null);

            foreach (var item in collection)
            {
                if (item != null)
                    references[i++] = referenceManager.GetReference(item, context);
            }

            return references;
        }

        public void SetStorage(object provider, object storeValue, PropertyInfo pi, ResourceOperationContext context)
        {
            ResourceReference[] referenes = (ResourceReference[]) storeValue;
            T[] array = new T[referenes.Length];

            var referenceManager = context.ReferenceManager;
            if (referenceManager == null)
                Debug.Assert(referenceManager != null);

            for (int i = 0; i < referenes.Length; i++)
            {
                if (referenes[i] != null)
                    array[i] = (T) referenceManager.GetResource(referenes[i], context);
            }

            array = array.Where(x => x != null).ToArray();
            pi.SetValue(provider, array);
        }
    }
}