using System;

namespace Igneel.Assets
{
    public interface IResource : IResourceAllocator, 
        INameable, IEquatable<IResource>, IComparable<IResource>, IPersistable
    {               
        bool IsDesignOnly { get; set; }

        int Id { get; set; }
      
    }
}
