using Igneel.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Rendering.Effects
{
    public class CheckTransparenceEffect:Effect
    {
        protected override TechniqueDesc[] GetTechniques()
        {
            return new TechniqueDesc[]{
                Tech("tech0")
                    .Pass<VertexPTxH>("RenderQuadVS", "CheckTransparencePS")
            };
        }
    }
}
