namespace Igneel.Assets.StorageConverters
{
    public class StateStoreConverter : IStoreConverter
    {
        public object GetStorage(object provider, object propValue, System.Reflection.PropertyInfo pi, ResourceOperationContext context)
        {            
            StateStore state = new StateStore(propValue);
            return state;
        }

        public void SetStorage(object provider, object storeValue, System.Reflection.PropertyInfo pi, ResourceOperationContext context)
        {
            StateStore state = (StateStore)storeValue;
            var obj = pi.GetValue(provider);
            state.Restore(obj);
        }
    }
}