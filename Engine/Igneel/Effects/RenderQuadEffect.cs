using Igneel.Graphics;
using Igneel.Rendering;

namespace Igneel.Effects
{
    public class RenderQuadEffect:Effect
    {
        public RenderQuadEffect(GraphicDevice device)
            : base(device) { }

        protected override TechniqueDesc[] GetTechniques()
        {
            return new TechniqueDesc[]{
                 Tech("tech0")
                     .Pass<VertexPTxH>("RenderQuadVS", "RenderQuadPS")
            };
        }
    }
}
