using Igneel.Graphics;
using Igneel.Rendering.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Rendering.Effects
{
    public class ShadowEdgeEffect:Effect
    {
        protected override TechniqueDesc[] GetTechniques()
        {
            return new TechniqueDesc[]{
                Tech("tech0").Pass<MeshVertex>("PreFilterVS", "PreFilterPS"),
                Tech("tech1").Pass<VertexPTxH>("RenderQuadVS", "DownSampleEdgePS"),
            };

        }
        public override void OnRenderCreated(Render render)
        {
              render.BindWith(new CameraBinding())
                    .BindWith(new SceneNodeWorldBinding())
                    .BindWith(new EdgeShadowFilteringTechnique.ShadowMapBinding());
        }
    }

    public class RenderShadowEdge : BasicMeshEffect
    {
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
