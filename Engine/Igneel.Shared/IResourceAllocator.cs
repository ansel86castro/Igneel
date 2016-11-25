using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace Igneel
{
    
    public interface IResourceAllocator : IDisposable
    {        
        [NonSerializedPropAttribute]
        bool Disposed { get; }
    }
}
