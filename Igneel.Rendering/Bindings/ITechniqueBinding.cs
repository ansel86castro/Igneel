using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Rendering.Bindings
{
    public interface ITechniqueBinding<T> :IRenderBinding<T>    
    {
        T LastBindedTechnique { get; }
    }

 

}
