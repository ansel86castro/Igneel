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
                Tech("tech0").Pass<MeshVertex>("PreFilterVS", "PreFilterPS")               
            };

        }
        public override void OnRenderCreated(Render render)
        {
              render.BindWith(new CameraBinding())
                    .BindWith(new SceneNodeWorldBinding())
                    .BindWith(new EdgeShadowFilteringTechnique.ShadowMapBinding());
        }
    }
}
