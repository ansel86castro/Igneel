using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Components
{
    public interface IShadingInput
    {
        bool IsGPUSync { get; set; }
    }
}
