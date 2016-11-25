using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Igneel;
using Igneel.Effects;
using Igneel.Graphics;
using Igneel.Rendering;
using Igneel.Rendering.Bindings;

namespace ForgeEditor.Effects
{
    public class MaterialHitTestEffect:Igneel.Rendering.Effect        
    {
        public MaterialHitTestEffect(GraphicDevice device)
            : base(device) { }

        protected override TechniqueDesc[] GetTechniques()
        {
            return new TechniqueDesc[]{
                Tech("tech0")
                    .Pass<MeshVertex>("Mesh_IdRenderVS", "Mesh_IdRenderPS")
            };
        }

        public override void OnRenderCreated(Render render)
        {
            render.BindWith(new CameraBinding())
                  .BindWith(new SceneNodeBinding())
                  .BindWith(new MaterialVisualIdBinding());
        }
    }

     public class MaterialHitSkinneffect : Effect
    {
         public MaterialHitSkinneffect(GraphicDevice device)
            : base(device) { }


        protected override TechniqueDesc[] GetTechniques()
        {
            return new TechniqueDesc[]{
                Tech("tech0")
                    .Pass<SkinnedVertex>("Skin_IdRenderVS", "Mesh_IdRenderPS")
            };
        }
        public override void OnRenderCreated(Render render)
        {
            render.BindWith(new CameraBinding())
                .BindWith(new SkinWorldBiding())
                .BindWith(new MaterialVisualIdBinding());
        }
    }


    public class MaterialVisualIdBinding : RenderBinding<IVisualMaterial, IIdMap>
    {     
        protected override void OnEffectChanged(Effect effect)
        {
            base.OnEffectChanged(effect);            
        }

        public override void OnBind(IVisualMaterial value)
        {
            if (Mapping != null)
            {
                var color = Color4.FromRgba(value.VisualId);
                int v = color.ToRgba();
                Mapping.Id = color;
            }
        }

        public override void OnUnBind(IVisualMaterial value)
        {
            if (Mapping != null)
            {
                Mapping.Id = new Color4();
            }
        }
    }
}
