namespace Igneel.Assets
{
    public interface IResourceActivator
    {
    
        /// <summary>
        ///  Must store the serializable objects in the IResourceActivator that are used
        /// to create the resource
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="context"></param>
        void OnAssetCreated(object provider, ResourceOperationContext context);

        /// <summary>
        /// Must create the resource form the saved state
        /// </summary>
        /// <returns></returns>
        object OnCreateResource(ResourceOperationContext context);
    }
}