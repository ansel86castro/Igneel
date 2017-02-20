using System.Collections.Generic;
using Igneel.Components;
using Igneel.Graphics;
using Igneel.Rendering;
using Igneel.SceneManagement;
using Igneel.States;

namespace Igneel.Techniques
{
    /// <summary>
    /// This is the default Render of a scene, it use the culling implementation of the scene and call the renders of the
    /// scenes`s objects
    /// </summary>
    public class DefaultTechnique : Technique
    {       
        static BlendState _transparent;
        static BlendState _noTransparent;
        static DepthStencilState _depthStenciState;
        static BlendState _additive;
        static RasterizerState _defaultCullingRState;
        static RasterizerState _noCullingRState;

        public static DepthStencilState DephtState { get { return _depthStenciState; } }
        public static BlendState NoBlend { get { return _noTransparent; } }
        public static BlendState Transparent { get { return _transparent; } }
        public static BlendState Additive { get { return _additive; } }
        public static RasterizerState NoCulling { get { return _noCullingRState; } }
        public static RasterizerState BackFaceCulling { get { return _defaultCullingRState; } }

        public DefaultTechnique()
        {            
            if (_transparent == null)
            {               
                _noTransparent = GraphicDeviceFactory.Device.CreateBlendState(new BlendDesc(blendEnable:false));

                _transparent = GraphicDeviceFactory.Device.CreateBlendState(new BlendDesc(blendEnable:true, 
                    srcBlend:Blend.SourceAlpha, 
                    destBlend:Blend.InverseSourceAlpha));

                _additive =  GraphicDeviceFactory.Device.CreateBlendState(new BlendDesc(true)
                {
                    BlendEnable = true,
                    SrcBlend = Blend.SourceAlpha,
                    DestBlend = Blend.One
                });

                _depthStenciState = GraphicDeviceFactory.Device.CreateDepthStencilState(new DepthStencilStateDesc(true) 
                {
                    DepthFunc = Comparison.Less
                });


                _noCullingRState = GraphicDeviceFactory.Device.CreateRasterizerState(new RasterizerDesc(true)
                {
                    AntialiasedLineEnable = GraphicDeviceFactory.Device.Info.MSAA.Count > 1,
                    MultisampleEnable = GraphicDeviceFactory.Device.Info.MSAA.Count > 1,
                    Cull = CullMode.None,
                    Fill = ShadingState.Shading.FillMode
                });
                _defaultCullingRState = GraphicDeviceFactory.Device.CreateRasterizerState(new RasterizerDesc(true)
                {
                    AntialiasedLineEnable = GraphicDeviceFactory.Device.Info.MSAA.Count > 1,
                    MultisampleEnable = GraphicDeviceFactory.Device.Info.MSAA.Count > 1,
                    Cull = CullMode.Back,
                    Fill = ShadingState.Shading.FillMode
                });

            }           
            
        }

        public override void Apply()
        {
            var scene = Engine.Scene;
            scene.UpdateVisibleComponents();

            GraphicDeviceFactory.Device.DepthTest = _depthStenciState;

            RenderScene();
        }      

        public static void RenderScene(bool useLights = true)
        {
            var scene = Engine.Scene;
            if (scene.ActiveCamera == null)
                return;                   

            /***********************************************************
             *      MULTY PASS RENDERING ALGORITHM
             *      
                *  1-Setup Alpha Blending
                *          SRCBLEND = SRCALPHA
                *          DESTBLEND = INVSRCALPHA
                *  4 IF activeLights.Length > 0
                *  2-Setup activeLights[0]
                *      Render Object
                *  3-Setup Blending 
                *          SRCBLEND = SRCALPHA
                *          DESTBLEND = ONE                                                 
                *  4-for i = 1 to activeLight.Lenght - 1 :
                *      Setup activeLight[i]
                *      Render Object
                *               
            *************************************************************/


            var activeLights = scene.ActiveLights;
            int nbLight = activeLights.Count;

            if (nbLight > 0 && useLights)
                Light.Current = activeLights[0];

            _RenderObjects(scene);
   
            if (useLights && nbLight > 1)
            {
                var enableAmbient = EngineState.Lighting.EnableAmbient;
                EngineState.Lighting.EnableAmbient = false;
                GraphicDeviceFactory.Device.Blend = _additive;

                for (int k = 1; k < nbLight; k++)
                {                    
                    Light.Current = activeLights[k];

                    GraphicDeviceFactory.Device.Rasterizer = _defaultCullingRState;
                    RenderEntries(scene.VisibleComponents, PixelClipping.Transparent);

                    GraphicDeviceFactory.Device.Rasterizer = _noCullingRState;
                    RenderEntries(scene.TransparentVisibleComponents, PixelClipping.Opaque);
                }

                EngineState.Lighting.EnableAmbient = enableAmbient;
            }
        }

        private static void _RenderObjects(Scene scene)
        {
            var visibleComponents = scene.VisibleComponents;
            var transparentComponents = scene.TransparentVisibleComponents;
            if (EngineState.Lighting.TransparencyEnable)
            {
                if (visibleComponents.Count > 0)
                {
                    //render opaque objects and clip transparent objects
                    GraphicDeviceFactory.Device.Blend = _noTransparent;
                    GraphicDeviceFactory.Device.Rasterizer = _defaultCullingRState;
                    RenderEntries(visibleComponents, PixelClipping.Transparent);
                }

                if (transparentComponents.Count > 0)
                {
                    ////render transparent objects and clip opaque objects           
                    GraphicDeviceFactory.Device.Blend = _transparent;
                    GraphicDeviceFactory.Device.Rasterizer = _noCullingRState;
                    RenderEntries(transparentComponents, PixelClipping.Opaque);
                }
            }
            else if (visibleComponents.Count > 0)
            {
                GraphicDeviceFactory.Device.Blend = _noTransparent;
                GraphicDeviceFactory.Device.Rasterizer = _defaultCullingRState;
                RenderEntries(visibleComponents, PixelClipping.None);
            }
        }

        public static void RenderEntries(List<GraphicSubmit> renderList, PixelClipping clipping)
        {            
            for (int i = 0, len = renderList.Count; i < len; i++)
            {                              
                renderList[i].Draw(clipping);               
            }           
        }

        public static void RenderSceneRenderList(bool useLights = true)
        {          
            var scene = Engine.Scene;
            var activeLights = scene.ActiveLights;
            int nbLight = useLights ? activeLights.Count : 1;

            if (nbLight > 0 && useLights)
                Light.Current  = activeLights[0];

            RenderEntries(scene.VisibleComponents, PixelClipping.Transparent);
            RenderEntries(scene.TransparentVisibleComponents, PixelClipping.Opaque);            
                   
            if (useLights)
            {
                for (int i = 1; i < nbLight; i++)
                {
                    Light.Current = activeLights[i];                 
                    RenderEntries(scene.VisibleComponents, PixelClipping.Transparent);
                    RenderEntries(scene.TransparentVisibleComponents, PixelClipping.Opaque);                          
                                   
                }
            }
        }

        public static void RenderSceneRenderList(List<GraphicSubmit> entries, bool useLights = true , PixelClipping clipping = PixelClipping.None)
        {
            var scene = Engine.Scene;
            var activeLights = scene.ActiveLights;
            int nbLight = useLights ? activeLights.Count : 1;
           
            if (nbLight > 0 && useLights)
                Light.Current  = activeLights[0];

            RenderEntries(entries, clipping);                     
            
            if (useLights)
            {
                for (int i = 1; i < nbLight; i++)
                {                  
                    Light.Current = activeLights[i];
                    RenderEntries(entries, clipping);
                             
                }
            }
        }       
      
        public static void SetupTranspacencyBlending()
        {
            if (EngineState.Lighting.TransparencyEnable)
            {
                GraphicDeviceFactory.Device.Blend = _transparent;
            }
            else
                GraphicDeviceFactory.Device.Blend = _noTransparent;
        }
    
        public static void SetupAdditiveBlending()
        {
            GraphicDeviceFactory.Device.Blend = _additive;
        }
       
    }

    public class BindableSceneTechnique<T> : DefaultTechnique
    {
        T _bindValue;
       
        public BindableSceneTechnique() : base() 
        {
            _bindValue = (T)(object)this;
        }              

        public override void Bind(Render render)
        {
            render.Bind(_bindValue);
        }

        public override void UnBind(Render render)
        {
            render.UnBind(_bindValue);
        }
    }   

    /// <summary>
    /// A render that support by shadow volumes and perform soft edge shadows
    /// </summary>
    //public class SceneShadowVolumeRender : SceneRenderTechnique
    //{
    //    EffectHandle renderShadowVolume;
    //    EffectHandle renderShadowVolume2Side;
    //    EffectHandle renderSceneAmbient;
    //    EffectHandle renderShadowVolume2;
    //    EffectHandle renderShadowVolume2Side2;
    //    EffectHandle showShadowVolume2;
    //    EffectHandle bumpSceneAmbient;

    //    ModelAmbientRender modelAmbientRender;
    //    AmbientTerrainRender terrainAmbientRender;
    //    //ModelShadowVolumeRender shadowModelRender;
    //    Vector4 shadowColor;
    //    public SceneShadowVolumeRender()
    //    {
    //        effect = EffectManager.ShadowVolume;
    //        shadowColor = new Vector4(0, 0, 0, 0.5f);

    //        renderShadowVolume = effect.GetTechique("RenderShadowVolume");
    //        renderShadowVolume2 = effect.GetTechique("RenderShadowVolume2");
    //        renderShadowVolume2Side = effect.GetTechique("RenderShadowVolume2Sided");
    //        renderShadowVolume2Side2 = effect.GetTechique("RenderShadowVolume2Sided2");
    //        showShadowVolume2 = effect.GetTechique("ShowShadowVolume2");
    //        renderSceneAmbient = effect.GetTechique("RenderSceneAmbient");
    //        bumpSceneAmbient = effect.GetTechique("BumpRenderSceneAmbient");

    //        modelAmbientRender = new ModelAmbientRender(effect, renderSceneAmbient, bumpSceneAmbient);
    //        terrainAmbientRender = new AmbientTerrainRender(effect, renderSceneAmbient);
    //        shadowModelRender = new ModelShadowVolumeRender(effect, renderShadowVolume2, renderShadowVolume2Side2) { ExtrudeBias = 100000, ShadowColor = new Vector4(0, 0, 0, 1) };
    //        //shadowModelRender = new ShadowModelRender(effect, showShadowVolume2, showShadowVolume2) { ExtrudeBias = 100000, ShadowColor = new Vector4(0, 0, 0, 1f) ,CullMode=Cull.CounterClockwise};
    //    }

    //    public override void Apply()
    //    {
    //        RenderManager rm = GEngine.RenderManager;

    //        GGraphicDeviceFactory.Device.Clear(ClearFlags.Target | ClearFlags.ZBuffer | ClearFlags.Stencil, 0, 1, 0);

    //        Mode1();

    //    }
    //    private void Mode1()
    //    {
    //        RenderManager rm = GEngine.RenderManager;
    //        //[1] - first render the scene with ambient only           
    //        rm.PushModelRender(modelAmbientRender);
    //        rm.PushTerrainRender(terrainAmbientRender);
    //        base.Apply();
    //        rm.RestoreModelRender();
    //        rm.RestoreTerrainRender();

    //        //[2] second setup the stencil-buffer and render the shadow volumenes of the scene
    //        rm.PushModelRender(shadowModelRender);
    //        rm.TerrainRender.Enable = false; // disable terrain render
    //        rm.SkyDomeRender.Enable = false; // disable sky render;

    //        base.Apply();

    //        rm.RestoreModelRender();        //restore to the default render
    //        rm.TerrainRender.Enable = true; // enable terrain render
    //        rm.SkyDomeRender.Enable = true; // enable sky render;

    //        //[3] setup the stencil-buffer and render the scene and process only the pixels where the stencil has cero value 
    //        //because the pixels that has non-cero value are in shadow
    //        RenderShadowedScene();
    //    }
    //    private void Mode2()
    //    {
    //        //RenderManager rm = GEngine.RenderManager;
    //        //var device = GGraphicDeviceFactory.Device;

    //        ////[1] - first render the scene with ambient only
    //        //var stencilEnable = device.RenderState.StencilEnable;
    //        //var zbufferFunc = device.RenderState.ZBufferFunction;
    //        //device.RenderState.StencilEnable = false;
    //        //device.RenderState.ZBufferFunction = Compare.LessEqual;
    //        //rm.PushModelRender(modelAmbientRender);
    //        //terrainAmbientRender.Enable = false;
    //        //rm.PushTerrainRender(terrainAmbientRender);
    //        //base.Apply();
    //        //rm.RestoreModelRender();
    //        //rm.RestoreTerrainRender();
    //        //device.RenderState.StencilEnable = stencilEnable;
    //        //device.RenderState.ZBufferFunction = zbufferFunc;

    //        ////[1] Render Scene normaly
    //        ////var stencilEnable = device.RenderState.StencilEnable;
    //        ////var zbufferFunc = device.RenderState.ZBufferFunction;
    //        ////device.RenderState.StencilEnable = false;
    //        ////device.RenderState.ZBufferFunction = Compare.LessEqual;            
    //        ////base.Apply();            
    //        ////device.RenderState.StencilEnable = stencilEnable;
    //        ////device.RenderState.ZBufferFunction = zbufferFunc;

    //        ////[2] Render Shadow Volumes into the stencil-buffer
    //        //rm.PushModelRender(shadowModelRender);
    //        //rm.TerrainRender.Enable = false; // disable terrain render
    //        //rm.SkyDomeRender.Enable = false; // disable sky render;
    //        //base.Apply();
    //        //rm.RestoreModelRender();        //restore to the default render
    //        //rm.TerrainRender.Enable = true; // enable terrain render
    //        //rm.SkyDomeRender.Enable = true; // enable sky render;

    //        ////[3]Draw a gray poligon in the shadowed area
    //        //DrawFullScreenQuad();

    //    }

    //    void RenderShadowedScene()
    //    {
    //        //var device = GGraphicDeviceFactory.Device;

    //        ////save render states
    //        //var ZWriteEnable = device.RenderState.ZBufferWriteEnable;
    //        //var ZBufferFunction = device.RenderState.ZBufferFunction;
    //        //var StencilEnable = device.RenderState.StencilEnable;
    //        //var ReferenceStencil = device.RenderState.ReferenceStencil;
    //        //var StencilFunction = device.RenderState.StencilFunction;
    //        //var StencilPass = device.RenderState.StencilPass;
    //        //var AlphaBlendEnable = device.RenderState.AlphaBlendEnable;
    //        //var BlendOperation = device.RenderState.BlendOperation;
    //        //var SourceBlend = device.RenderState.SourceBlend;
    //        //var DestinationBlend = device.RenderState.DestinationBlend;

    //        ////apply render states
    //        //device.RenderState.StencilEnable = true;
    //        //device.RenderState.ReferenceStencil = 1;
    //        //device.RenderState.StencilFunction = Compare.Greater;
    //        //device.RenderState.StencilPass = StencilOperation.Keep;

    //        //device.RenderState.ZBufferWriteEnable = true;
    //        //device.RenderState.ZBufferFunction = Compare.LessEqual;
    //        ////device.RenderState.ReferenceStencil = 1;
    //        ////device.RenderState.StencilFunction = Compare.Greater;
    //        ////device.RenderState.StencilPass = StencilOperation.Keep;
    //        ////device.RenderState.AlphaBlendEnable = true;
    //        ////device.RenderState.BlendOperation = BlendOperation.Add;
    //        ////device.RenderState.SourceBlend = Blend.One;
    //        ////device.RenderState.DestinationBlend = Blend.One;

    //        //var ambient = GLight.Ambient;
    //        //GLight.Ambient = new Vector3(0, 0, 0);
    //        //base.Apply();
    //        //GLight.Ambient = ambient;

    //        ////restore render states
    //        //device.RenderState.ZBufferWriteEnable = ZWriteEnable;
    //        //device.RenderState.ZBufferFunction = ZBufferFunction;
    //        //device.RenderState.StencilEnable = StencilEnable;
    //        //device.RenderState.ReferenceStencil = ReferenceStencil;
    //        //device.RenderState.StencilFunction = StencilFunction;
    //        //device.RenderState.StencilPass = StencilPass;
    //        //device.RenderState.AlphaBlendEnable = AlphaBlendEnable;
    //        //device.RenderState.BlendOperation = BlendOperation;
    //        //device.RenderState.SourceBlend = SourceBlend;
    //        //device.RenderState.DestinationBlend = DestinationBlend;
    //    }

    //    Vector4[] quadVertex;
    //    EffectHandle simpleColoredTech;

    //    void DrawFullScreenQuad()
    //    {
    //        //var device = GGraphicDeviceFactory.Device;
    //        //if (quadVertex == null)
    //        //{
    //        //    quadVertex = new Vector4[4];
    //        //}

    //        //float xpos = (float)device.PresentationParameters.BackBufferWidth;
    //        //float ypos = (float)device.PresentationParameters.BackBufferHeight;
    //        //quadVertex[0] = new Vector4(0, ypos, 0.0f, 1.0f);
    //        //quadVertex[1] = new Vector4(0, 0, 0.0f, 1.0f);
    //        //quadVertex[2] = new Vector4(xpos, ypos, 0.0f, 1.0f);
    //        //quadVertex[3] = new Vector4(xpos, 0, 0.0f, 1.0f);

    //        //var ZWriteEnable = device.RenderState.ZBufferWriteEnable;
    //        //var ZBufferFunction = device.RenderState.ZBufferFunction;
    //        //var StencilEnable = device.RenderState.StencilEnable;
    //        //var ReferenceStencil = device.RenderState.ReferenceStencil;
    //        //var StencilFunction = device.RenderState.StencilFunction;
    //        //var StencilPass = device.RenderState.StencilPass;


    //        //device.RenderState.ZBufferWriteEnable = false;
    //        //device.RenderState.StencilEnable = true;
    //        //device.RenderState.FogEnable = false;

    //        //device.RenderState.ReferenceStencil = 0x1;
    //        //device.RenderState.StencilFunction = Compare.LessEqual;
    //        //device.RenderState.StencilPass = StencilOperation.Keep;

    //        //ShaderEffect effect = EffectManager.SimpleColored;
    //        //if (simpleColoredTech == null)
    //        //    simpleColoredTech = effect.GetTechique("Technique3");

    //        //device.VertexDeclaration = null;
    //        //device.VertexFormat = VertexFormat.Transformed;

    //        //effect.Technique = simpleColoredTech;
    //        //effect.SetValue("Color", shadowColor);
    //        //effect.Apply(() => device.DrawUserPrimitives(PrimitiveType.TriangleStrip, 2, quadVertex));

    //        ////restore
    //        //device.RenderState.ZBufferWriteEnable = ZWriteEnable;
    //        //device.RenderState.ZBufferFunction = ZBufferFunction;
    //        //device.RenderState.StencilEnable = StencilEnable;
    //        //device.RenderState.ReferenceStencil = ReferenceStencil;
    //        //device.RenderState.StencilFunction = StencilFunction;
    //        //device.RenderState.StencilPass = StencilPass;
    //    }              
    //}
}
