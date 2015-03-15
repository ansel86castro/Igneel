using Igneel.Graphics;
using Igneel.Rendering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Scenering.Effects
{
    public class RenderQuadEffect:Effect
    {
        protected override TechniqueDesc[] GetTechniques()
        {
            return new TechniqueDesc[]{
                 Tech("tech0")
                     .Pass<VertexPTxH>("RenderQuadVS", "RenderQuadPS")
            };
        }
    }
}
