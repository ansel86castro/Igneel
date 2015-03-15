using Igneel.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Scenering.Bindings
{
    public interface ISyncronizableBinding<T> : IRenderBinding<T>
        where T : IShadingInput
    {

    }

}
