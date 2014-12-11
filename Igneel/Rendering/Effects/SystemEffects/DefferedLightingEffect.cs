using Igneel.Graphics;
using Igneel.Rendering.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Rendering.Effects
{
    public class DefferedLightingEffect : Effect
    {
        protected override TechniqueDesc[] GetTechniques()
        {
            return new TechniqueDesc[]
            {
                Tech("tech0").Pass<VertexPTxH>("RenderQuadVS", "DefferedLightingPS")
            };
        }

        public override void OnRenderCreated(Render render)
        {
            render.BindWith(new CameraBinding())
                  .BindWith(new LightBinding())
                  .BindWith(new AmbientLightBinding())
                  .BindWith(new PlaneReflectionBinding());
        }
    }

    public class DefferedShadowRender : Effect
    {
        protected override TechniqueDesc[] GetTechniques()
        {
            return new TechniqueDesc[]
            {
                Tech("tech0").Pass<VertexPTxH>("RenderQuadVS", "DefferedLightingShadowedPS")
            };
        }

        public override void OnRenderCreated(Render render)
        {
            render.BindWith(new CameraBinding())
                  .BindWith(new LightBinding())
                  .BindWith(new ShadowMapBinding())
                  .BindWith(new AmbientLightBinding())
                  .BindWith(new PlaneReflectionBinding());
        }
    }

    
}
