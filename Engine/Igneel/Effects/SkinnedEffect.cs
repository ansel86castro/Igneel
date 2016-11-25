using Igneel.Graphics;
using Igneel.Rendering;
using Igneel.Components;
using Igneel.Rendering.Bindings;
using Igneel.SceneManagement;
using Igneel.Techniques;


namespace Igneel.Effects
{
    public class SkinWorldBiding : RenderBinding<Frame>
    {
        IWorldMap _mapping;

        protected override void OnEffectChanged(Effect effect)
        {
            base.OnEffectChanged(effect);
            _mapping = effect.Map<IWorldMap>();
        }
        public override void OnBind(Frame value)
        {
            _mapping.World = Matrix.Identity;
        }

        public override void OnUnBind(Frame value)
        {

        }
    }

    public class BasicSkinnedEffect : BasicMeshEffect
    {
        public BasicSkinnedEffect(GraphicDevice device)
            : base(device) { }

        protected override TechniqueDesc[] GetTechniques()
        {
            return new TechniqueDesc[]{
                Tech("tech0")
                    .Pass<SkinnedVertex>("Skin_PhongVS", "Mesh_PhongPS"),
                Tech("tech1")
                    .Pass<SkinnedVertex>("Skin_BumpVS", "Mesh_BumpPS")
            };
        }


        public override void OnRenderCreated(Render render)
        {
            base.OnRenderCreated(render);            
            render.BindWith(new SkinWorldBiding()); 
        }
    }

    public class RenderSkinnedIdEffect : Effect
    {
        public RenderSkinnedIdEffect(GraphicDevice device)
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
                .BindWith(new HitTestIdBinding());
        }
    }

    public class RenderSkinnedDepthEffect : Effect
    {
        public RenderSkinnedDepthEffect(GraphicDevice device)
            : base(device) { }

        protected override TechniqueDesc[] GetTechniques()
        {
            return new TechniqueDesc[]{
                Tech("tech0")
                    .Pass<SkinnedVertex>("Skin_RenderDepthVS", "Mesh_RenderDepthPS")
            };
        }
        public override void OnRenderCreated(Render render)
        {       
            render.BindWith(new CameraBinding())
                  .BindWith(new BuildSMapMatBinding())                           
                  .BindWith(new SkinWorldBiding());

        }
    }

    public class SkinnedShadowMapEffect : BasicSkinnedEffect
    {
        public SkinnedShadowMapEffect(GraphicDevice device)
            : base(device) { }

        protected override TechniqueDesc[] GetTechniques()
        {
            return new TechniqueDesc[]{
                Tech().Pass<SkinnedVertex>("Skin_ShadowPhongVS", "Mesh_ShadowPhong3KPS"),
                Tech().Pass<SkinnedVertex>("Skin_ShadowPhongVS", "Mesh_ShadowPhong5KPS"),
                Tech().Pass<SkinnedVertex>("Skin_ShadowPhongVS", "Mesh_ShadowPhong7KPS"),

                Tech().Pass<SkinnedVertex>("Skin_ShadowBumpVS", "Mesh_ShadowBump3KPS"),
                Tech().Pass<SkinnedVertex>("Skin_ShadowBumpVS", "Mesh_ShadowBump5KPS"),
                Tech().Pass<SkinnedVertex>("Skin_ShadowBumpVS", "Mesh_ShadowBump7KPS")
            };
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
            var binding = render.GetBinding<BasicMaterial>();
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
