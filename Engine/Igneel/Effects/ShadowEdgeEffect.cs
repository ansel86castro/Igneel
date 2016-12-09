using Igneel.Graphics;
using Igneel.Rendering;
using Igneel.Rendering.Bindings;
using Igneel.Techniques;

namespace Igneel.Effects
{
    public class ShadowEdgeEffect:Effect
    {
        public ShadowEdgeEffect(GraphicDevice device)
            : base(device) { }

        protected override TechniqueDesc[] GetTechniques()
        {
            return new TechniqueDesc[]{
                Tech().Pass<MeshVertex>("PreFilterVS", "RenderShadowPS"),
                Tech().Pass<VertexPTxH>("RenderQuadVS", "ShadowEdgeDetectPS"),
                Tech().Pass<VertexPTxH>("RenderQuadVS", "DownSampleEdgePS"),
            };

        }
        public override void OnRenderCreated(Render render)
        {
              render.BindWith(new CameraBinding())
                    .BindWith(new SceneNodeBinding())
                    .BindWith(new EdgeShadowFilteringTechnique.ShadowMapBinding());
        }
    }

    public class RenderShadowEdge : BasicMeshEffect
    {
        public RenderShadowEdge(GraphicDevice device)
            : base(device) { }

        protected override TechniqueDesc[] GetTechniques()
        {
            return new TechniqueDesc[]{
                Tech("tech0").Pass<MeshVertex>("Mesh_ShadowPhongVS","Mesh_ShadowEdge3KPS")
            };
        }
        public override void OnRenderCreated(Render render)
        {
            base.OnRenderCreated(render);
            render.BindWith(new ShadowMapBinding());
            render.BindWith(new EdgeShadowFilteringTechnique.Binding());
        }
    }
}
