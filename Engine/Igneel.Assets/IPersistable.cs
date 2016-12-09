using System;
namespace Igneel.Assets
{
    public interface IPersistable
    {
        Asset CreateAsset(ResourceOperationContext context);
    }

    public class Persistable : IPersistable
    {

        public Asset CreateAsset(ResourceOperationContext context)
        {
            return new GenericAsset(this, context);
        }
    }
}
