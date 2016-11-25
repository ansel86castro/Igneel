using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Igneel.Graphics;
using Igneel.Rendering;
using Igneel.Rendering.Bindings;
using Igneel.SceneComponents;

using float4x4 = Igneel.Matrix;

namespace ForgeEditor.Effects
{
    public class DecalGlypEffect:Igneel.Rendering.Effect
    {
        public DecalGlypEffect(GraphicDevice device)
            : base(device) { }

        protected override TechniqueDesc[] GetTechniques()
        {
            return new TechniqueDesc[]{
                Tech("tech0")
                    .Pass<VertexPositionColor>("TranslationGlypVS", "Mesh_ColoredPS")
            };
        }
        public override void OnRenderCreated(Render render)
        {
            render.BindWith(new CameraBinding())              
                  .BindWith(new SceneNodeBinding());
        }

        public interface ICamaraMap
        {
            float4x4 View { get; set; }
            float4x4 Proj { get; set; }            
        }

        public sealed class CameraBinding : RenderBinding<Camera>
        {
            ICamaraMap _map;

            protected override void OnEffectChanged(Effect effect)
            {
                base.OnEffectChanged(effect);

                _map = effect.Map<ICamaraMap>();
            }

            public sealed override void OnBind(Camera obj)
            {
                if (_map != null)
                {
                    _map.View = obj.View;
                    _map.Proj = obj.Projection;
                }
            }

            public override void OnUnBind(Camera value)
            {

            }
        }
    }
}
