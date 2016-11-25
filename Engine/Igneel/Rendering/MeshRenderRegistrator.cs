using Igneel.Effects;
using Igneel.SceneComponents;
using Igneel.Techniques;

namespace Igneel.Rendering
{   
    [Registrator]
    public class MeshInstanceRenderRegistrator : Registrator<FrameMesh, MeshInstanceRenderRegistrator>
    {       
        public override void RegisterRenders()
        {                 
            //default Render Technique
            Register<DefaultTechnique, FrameMeshRender<BasicMeshEffect>>();
           
            //render node id to RenderTarget
            Register<HitTestTechnique, FrameMeshRender<RenderMeshIdEffect>>();

            //render mesh to a depth Map
            Register<DepthSceneRender, FrameMeshRender<RenderMeshDepthEffect>>();
         
            //render a mesh with shadow map technique
            Register<SceneShadowMapping, FrameMeshRender<MeshShadowMapEffect>>();

            Register<EdgeShadowFilteringTechnique.SillueteRender, FrameMeshRender<ShadowEdgeEffect>>();

            Register<EdgeShadowFilteringTechnique, FrameMeshRender<RenderShadowEdge>>();

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
            Register<DefferedLigthing<DefferedLightingEffect>, FrameMeshRender<DefferedRenderEffect>>();

            //Deferred render with shadow mapping
            Register<DefferedLigthing<DefferedShadowRender>, FrameMeshRender<DefferedRenderEffect>>();
        }
    }

    [Registrator]
    public class SkinInstanceRenderRegistrator : Registrator<FrameSkin, SkinInstanceRenderRegistrator>
    {        
        public override void RegisterRenders()
        {
            Register<DefaultTechnique, FrameSkinRender<BasicSkinnedEffect>>();

            //render mesh to a depth Map
            Register<DepthSceneRender, FrameSkinRender<RenderSkinnedDepthEffect>>();

            //render a mesh with shadow map technique
            Register<SceneShadowMapping, FrameSkinRender<SkinnedShadowMapEffect>>();

            //render node id to RenderTarget
            Register<HitTestTechnique, FrameSkinRender<RenderSkinnedIdEffect>>();
        }
    }
}
