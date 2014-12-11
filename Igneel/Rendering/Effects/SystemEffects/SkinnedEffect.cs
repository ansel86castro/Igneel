using Igneel.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Igneel.Rendering.Bindings;
using Igneel.Components;

namespace Igneel.Rendering.Effects
{
    public class SkinWorldBiding : RenderBinding<SceneNode>
    {
        SceneNodeWorldBinding.IWorldMapping mapping;

        protected override void OnEffectChanged(Effect effect)
        {
            base.OnEffectChanged(effect);
            mapping = effect.Map<SceneNodeWorldBinding.IWorldMapping>(true);
        }
        public override void OnBind(SceneNode value)
        {
            mapping.World = Matrix.Identity;
        }

        public override void OnUnBind(SceneNode value)
        {

        }
    }

    public class BasicSkinnedEffect : BasicMeshEffect
    {
        protected override TechniqueDesc[] GetTechniques()
        {
            return Descriptions(
                Tech("tech0")
                    .Pass<SkinnedVertex>("Skin_PhongVS", "Mesh_PhongPS"),
                Tech("tech1")
                    .Pass<SkinnedVertex>("Skin_BumpVS", "Mesh_BumpPS")
            );
        }        


        public override void OnRenderCreated(Render render)
        {
            base.OnRenderCreated(render);
            var skinBinding = new SkinBinding();

            render.BindWith<SkinInstance>(skinBinding)
                  .BindWith<MeshPart>(skinBinding)
                  .BindWith(new SkinWorldBiding()); 
        }
    }

    public class RenderSkinnedIdEffect : Effect
    {
        protected override TechniqueDesc[] GetTechniques()
        {
            return Descriptions(
                Tech("tech0")
                    .Pass<SkinnedVertex>("Skin_IdRenderVS", "Mesh_IdRenderPS")
            );
        }
        public override void OnRenderCreated(Render render)
        {
            var skinBinding = new SkinBinding();
            render.BindWith(new CameraBinding())
                .BindWith<SkinInstance>(skinBinding)
                .BindWith<MeshPart>(skinBinding)
                .BindWith(new SkinWorldBiding())
                .BindWith(new IdBinding());
        }
    }

    public class RenderSkinnedDepthEffect : Effect
    {
        protected override TechniqueDesc[] GetTechniques()
        {
            return Descriptions(
                Tech("tech0")
                    .Pass<SkinnedVertex>("Skin_RenderDepthVS", "Mesh_RenderDepthPS")
            );
        }
        public override void OnRenderCreated(Render render)
        {
            var skinBinding = new SkinBinding();
            render.BindWith(new CameraBinding())
                  .BindWith(new BuildSMapMatBinding())                  
                  .BindWith<SkinInstance>(skinBinding)
                  .BindWith<MeshPart>(skinBinding)
                  .BindWith(new SkinWorldBiding());

        }
    }

    public class SkinnedShadowMapEffect : BasicSkinnedEffect
    {
        protected override TechniqueDesc[] GetTechniques()
        {
            return Descriptions(
                Tech().Pass<SkinnedVertex>("Skin_ShadowPhongVS", "Mesh_ShadowPhong3KPS"),
                Tech().Pass<SkinnedVertex>("Skin_ShadowPhongVS", "Mesh_ShadowPhong5KPS"),
                Tech().Pass<SkinnedVertex>("Skin_ShadowPhongVS", "Mesh_ShadowPhong7KPS"),

                Tech().Pass<SkinnedVertex>("Skin_ShadowBumpVS", "Mesh_ShadowBump3KPS"),
                Tech().Pass<SkinnedVertex>("Skin_ShadowBumpVS", "Mesh_ShadowBump5KPS"),
                Tech().Pass<SkinnedVertex>("Skin_ShadowBumpVS", "Mesh_ShadowBump7KPS")
            );
        }

        public override void OnRender(Render render)
        {
            var sm = render.GetBinding<ShadowMapTechnique>();
            Technique = 0;
            ShadowMapTechnique tech;
            if ((tech = sm.BindedValue) != null)
            {
                switch (tech.KernelSize)
                {
                    case 3:
                        Technique = 0;
                        break;
                    case 5:
                        Technique = 1;
                        break;
                    case 7:
                        Technique = 2;
                        break;
                    default:
                        Technique = 0;
                        break;
                }
            }
            var binding = render.GetBinding<MeshMaterial>();
            if (binding.BindedValue != null && binding.BindedValue.NormalMap != null)
            {
                Technique = +3;
            }
        }

        public override void OnRenderCreated(Render render)
        {
            base.OnRenderCreated(render);
            render.BindWith(new ShadowMapBinding());
        }
    }
}
