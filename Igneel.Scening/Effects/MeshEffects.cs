
using Igneel.Graphics;
using Igneel.Rendering;
using Igneel.Rendering.Bindings;
using Igneel.Scenering.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Scenering.Effects
{
    public class BasicMeshEffect : Effect
    {            
        protected override TechniqueDesc[] GetTechniques()
        {
            return new TechniqueDesc[]
            {
                Tech("tech0")
                    .Pass<MeshVertex>("Mesh_PhongVS", "Mesh_PhongPS"),
                Tech("tech1")
                    .Pass<MeshVertex>("Mesh_BumpVS", "Mesh_BumpPS")
            };
        }

        public override void OnRender(Render render)
        {
            var binding = render.GetBinding<MeshMaterial>();
            if (binding.BindedValue.NormalMap != null)
            {
                Technique = 1;
            }
            else
                Technique = 0;            
        }
     
        public override void OnRenderCreated(Render render)
        {
            render.BindWith(new CameraBinding())
                  .BindWith(new LightBinding())
                  .BindWith(new AmbientLightBinding())
                  .BindWith(new SceneNodeBinding())
                  .BindWith(new MeshMaterialBinding())
                  .BindWith(new EnvironmentMapBinding())
                  .BindWith(new PlaneReflectionBinding())
                  .BindWith(new ClipSceneTechniqueBinding())
                  .BindWith(new PixelClippingBinding());
        }
    }

    public class RenderMeshDepthEffect : Effect
    {
        protected override TechniqueDesc[] GetTechniques()
        {
            return new TechniqueDesc[]{
                Tech("tech0")
                    .Pass<MeshVertex>("Mesh_RenderDepthVS", "Mesh_RenderDepthPS")
            };
        }
       
        public override void OnRenderCreated(Render render)
        {
            render.BindWith(new CameraBinding())
                  .BindWith(new BuildSMapMatBinding())
                  .BindWith(new SceneNodeBinding());

        }
    }

    public class RenderMeshIdEffect<T> : Effect
       where T : struct
    {
        protected override TechniqueDesc[] GetTechniques()
        {
            return new TechniqueDesc[]{
                Tech("tech0")
                    .Pass<T>("Mesh_IdRenderVS", "Mesh_IdRenderPS")
            };
        }

        public override void OnRenderCreated(Render render)
        {
            render.BindWith(new CameraBinding())
                  .BindWith(new SceneNodeBinding());               
        }
    }

    public class RenderMeshIdEffect : RenderMeshIdEffect<MeshVertex> { }

    public class MeshShadowMapEffect : BasicMeshEffect
    {
        protected override TechniqueDesc[] GetTechniques()
        {
            return new TechniqueDesc[]{
                Tech().Pass<MeshVertex>("Mesh_ShadowPhongVS", "Mesh_ShadowPhong3KPS"),
                Tech().Pass<MeshVertex>("Mesh_ShadowPhongVS", "Mesh_ShadowPhong5KPS"),
                Tech().Pass<MeshVertex>("Mesh_ShadowPhongVS", "Mesh_ShadowPhong7KPS"),
                Tech().Pass<MeshVertex>("Mesh_ShadowPhongVS", "Mesh_ShadowPhong1KPS"),


                Tech().Pass<MeshVertex>("Mesh_ShadowBumpVS", "Mesh_ShadowBump3KPS"),
                Tech().Pass<MeshVertex>("Mesh_ShadowBumpVS", "Mesh_ShadowBump5KPS"),
                Tech().Pass<MeshVertex>("Mesh_ShadowBumpVS", "Mesh_ShadowBump7KPS"),
                Tech().Pass<MeshVertex>("Mesh_ShadowBumpVS", "Mesh_ShadowBump1KPS")
           };  
        }

        public override void OnRender(Render render)
        {            
            var sm = render.GetBinding<ShadowMapTechnique>();
            Technique = 0;
            ShadowMapTechnique tech;
            if ((tech = sm.BindedValue)!=null)
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
                    case 1:
                        Technique = 3;
                        break;
                    default:
                        Technique = 0;
                        break;
                }
            }
            var binding = render.GetBinding<MeshMaterial>();
            if (binding.BindedValue!=null && binding.BindedValue.NormalMap != null)
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

    public class RenderMeshColorEffect : BasicMeshEffect
    {
        protected override TechniqueDesc[] GetTechniques()
        {
            return new TechniqueDesc[]{
                Tech("tech0")
                    .Pass<VertexPositionColor>("Mesh_ColoredVS", "Mesh_ColoredPS")
            };
        }
        public override void OnRender(Render render)
        {
            base.OnRender(render);
        }
       
    }

    public class DefferedRenderEffect : Effect
    {
        protected override TechniqueDesc[] GetTechniques()
        {
            return new TechniqueDesc[]{
                Tech("tech0").Pass<MeshVertex>("Mesh_DefferedVS", "Mesh_DefferedPS")               
            };
        }
      
        public override void OnRenderCreated(Render render)
        {
            render.BindWith(new CameraBinding())          
                .BindWith(new SceneNodeBinding())
                .BindWith(new MeshMaterialBinding())
                .BindWith(new EnvironmentMapBinding())
                .BindWith(new PlaneReflectionBinding())
                .BindWith(new ClipSceneTechniqueBinding())
                .BindWith(new PixelClippingBinding());
        }
    }   

}
