using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Assets
{
    public interface IReferenceManager
    {
        ResourceReference GetReference(IResource resource, ResourceOperationContext context);

        IResource GetResource(ResourceReference reference, ResourceOperationContext context);
    }
}
