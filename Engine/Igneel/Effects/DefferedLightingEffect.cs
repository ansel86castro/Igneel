using Igneel.Graphics;
using Igneel.Rendering;
using Igneel.Rendering.Bindings;

namespace Igneel.Effects
{
    public class DefferedLightingEffect : Effect
    {
        public DefferedLightingEffect(GraphicDevice device)
            : base(device) { }

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
        public DefferedShadowRender(GraphicDevice device)
            : base(device) { }

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
