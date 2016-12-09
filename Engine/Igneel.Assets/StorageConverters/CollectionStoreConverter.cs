using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;

namespace Igneel.Assets.StorageConverters
{
    public class CollectionStoreConverter<T> : IStoreConverter
        where T:class ,IResource
    {
        public object GetStorage(object provider, object propValue, PropertyInfo pi, ResourceOperationContext context)
        {
            ICollection<T> collection = (ICollection<T>)propValue;
            ResourceReference[] references = new ResourceReference[collection.Count];
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
            ResourceReference[] references = (ResourceReference[])storeValue;
            ICollection<T> collection = (ICollection<T>)pi.GetValue(provider);

            var referenceManager = context.ReferenceManager;
            if (referenceManager == null)
                Debug.Assert(referenceManager != null);

            for (int index = 0; index < references.Length; index++)
            {
                var refe = references[index];
                if (refe != null)
                    collection.Add((T) referenceManager.GetResource(refe, context));
            }
        }
    }
}