using Igneel.Rendering;
using Igneel.Scenering;
using Igneel.Scenering.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace Igneel.Rendering
{   
    [Registrator]
    public class MeshInstanceRenderRegistrator : Registrator<MeshInstance, MeshInstanceRenderRegistrator>
    {       
        public override void RegisterRenders()
        {                 
            //default Render Technique
            Register<SceneTechnique, MeshRender<BasicMeshEffect, MeshInstance>>();
           
            //render node id to RenderTarget
            Register<HitTestTechnique, MeshRender<RenderMeshIdEffect, MeshInstance>>();

            //render mesh to a depth Map
            Register<DepthSceneRender, MeshRender<RenderMeshDepthEffect, MeshInstance>>();
         
            //render a mesh with shadow map technique
            Register<SceneShadowMapping, MeshRender<MeshShadowMapEffect, MeshInstance>>();

            Register<EdgeShadowFilteringTechnique.SillueteRender, MeshRender<ShadowEdgeEffect, MeshInstance>>();

            Register<EdgeShadowFilteringTechnique, MeshRender<RenderShadowEdge, MeshInstance>>();

            //render mesh shadow factor technique
            //Register<RenderShadowFactorTechnique, MeshRender<RenderShadowFactorEffect, MeshInstance>>(() =>                
            //        new MeshRender<RenderShadowFactorEffect, MeshInstance>()
            //        .SetBinding<ShadowMapBinding<ShadowMapTechnique>, ShadowMapTechnique>()
            //        .SetBinding<StaticShadowMapBinding, StaticShadowMapTechnique>()
            //        .EndBindings()                    
            //    );

            //render a mesh with soft shadow map technique
            //Register<SoftShadowMappingTechnique, MeshRender<SoftModelShadowMap, MeshInstance>>(() =>                
            //        new MeshRender<SoftModelShadowMap, MeshInstance>()
            //        .SetBinding(new MeshMaterialBinding())
            //        .SetBinding(new EnvironmentMapBinding())
            //        .SetBinding(new ShaderSelector<SoftModelShadowMap>())
            //        .SetBinding<SoftShadowMappingTechnique>(
            //                x => GraphicDeviceFactory.Device.PixelShaderStage.SetResource(7, x.BlurShadowFactorMap),
            //                x => GraphicDeviceFactory.Device.PixelShaderStage.SetResource(7, null))
            //        .SetBinding(new PlaneReflectionBinding())
            //        .SetBinding(new ClipSceneTechniqueBinding())
            //        .SetBinding(new PixelClippingBinding())
            //        .EndBindings()                    
            //    );

           //Deferred render
            Register<DefferedLigthing<DefferedLightingEffect>, MeshRender<DefferedRenderEffect, MeshInstance>>();

            //Deferred render with shadow mapping
            Register<DefferedLigthing<DefferedShadowRender>, MeshRender<DefferedRenderEffect, MeshInstance>>();
        }
    }

     [Registrator]
    public class SkinInstanceRenderRegistrator : Registrator<SkinInstance, SkinInstanceRenderRegistrator>
    {        
        public override void RegisterRenders()
        {
            Register<SceneTechnique, MeshRender<BasicSkinnedEffect, SkinInstance>>( );

            //render mesh to a depth Map
            Register<DepthSceneRender, MeshRender<RenderSkinnedDepthEffect, SkinInstance>>();

            //render a mesh with shadow map technique
            Register<SceneShadowMapping, MeshRender<SkinnedShadowMapEffect, SkinInstance>>();
        }
    }
}
