using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Graphics
{
    public abstract partial class GraphicDevice
    {
        RasterizerStateStack rasStack;
        BlendStateStack blendStack;
        DepthStencilStateStack depthStack;

        public RasterizerStateStack RasterizerStack { get { return rasStack; } }

        public BlendStateStack BlendStack { get { return blendStack; } }

        public DepthStencilStateStack DepthStencilStack { get { return depthStack; } }
    }
}
