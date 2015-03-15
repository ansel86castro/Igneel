
using Igneel.Graphics;
using Igneel.Scenering;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Rendering
{
    public class DefferedLigthing <T>: SceneTechnique
        where T:Effect
    {
        public interface ShaderInput : Sprite.IShaderInput
        {
            Matrix invViewProj { get; set; }
        }

        RenderTexture2D positionRT;
        RenderTexture2D normalRT;
        RenderTexture2D diffuseRT;
        RenderTexture2D specularRT;
      
        RenderTarget[] renderTargets;
        Texture2D[] textures;
        Render render;

        public Texture2D[] Textures { get { return textures; } }

        public DefferedLigthing()
        {
            var device = GraphicDeviceFactory.Device;                   
  
            device.BackBuffer.Resized += OMBackBuffer_Resized;

            OMBackBuffer_Resized(device.BackBuffer);

            render = new Render();            
        }

        void OMBackBuffer_Resized(Graphics.RenderTarget renderTarget)
        {            
            var device = GraphicDeviceFactory.Device;
            int width = renderTarget.Width;
            int height = renderTarget.Height;

            //positionRT = new RenderTexture2D(width, height, Format.R32G32B32A32_FLOAT);
            //positionRT = new RenderTexture2D(width, height, Format.R16G16B16A16_FLOAT);
            positionRT = new RenderTexture2D(width, height, Format.R16G16B16A16_UNORM);
            normalRT = new RenderTexture2D(width, height, Format.R16G16B16A16_UNORM);
            diffuseRT = new RenderTexture2D(width, height, Format.R8G8B8A8_UNORM);
            specularRT = new RenderTexture2D(width, height, Format.R8G8B8A8_UNORM);            

            renderTargets = new RenderTarget[] 
            { 
                positionRT.Target, 
                normalRT.Target, 
                diffuseRT.Target, 
                specularRT.Target             
            };
            textures = new Texture2D[]
            {
                positionRT.Texture, 
                normalRT.Texture, 
                diffuseRT.Texture, 
                specularRT.Texture                
            };
        }

        public override void Apply()
        {
            var scene = SceneManager.Scene;
            if (scene.ActiveCamera == null)
                return;

            var device = GraphicDeviceFactory.Device;
            device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Engine.BackColor, 1, 0);

            scene.UpdateRenderLists();            

            device.SaveRenderTarget();           
            device.SetRenderTargets(renderTargets.Length, renderTargets);
            device.Clear(ClearFlags.Target, new Color4(), 1, 0);

            device.DepthTest = SceneTechnique.DephtState;
            //render opaque objects and clip transparent objects
            device.Blend = SceneTechnique.NoBlend;
            device.Rasterizer = SceneTechnique.BackFaceCulling;
            RenderEntries(scene.RenderList, PixelClipping.Transparent);

            device.SetRenderTargets(renderTargets.Length, null);
            device.RestoreRenderTarget();                     

            render.Draw(this);            

            #region RenderTransparents

            var transparents = scene.TransparentRenderList;
            if (transparents.Count > 0)
            {
                RenderManager.PopTechnique();

                var enableAmbient = EngineState.Lighting.EnableAmbient;
                var activeLights = scene.ActiveLights;
                int nbLight = activeLights.Count;

                if (nbLight > 0)
                    Light.Current = activeLights[0];

                GraphicDeviceFactory.Device.Blend = SceneTechnique.Transparent;
                GraphicDeviceFactory.Device.Rasterizer = SceneTechnique.NoCulling;
                for (int i = 0; i < transparents.Count; i++)
                {
                    transparents[i] = transparents[i].UpdateRender();
                }

                RenderEntries(transparents, PixelClipping.Opaque);

                if (nbLight > 1)
                {
                    EngineState.Lighting.EnableAmbient = false;
                    GraphicDeviceFactory.Device.Blend = SceneTechnique.Additive;

                    for (int k = 1; k < nbLight; k++)
                    {
                        Light.Current = activeLights[k];

                        RenderEntries(transparents, PixelClipping.Opaque);

                    }
                }

                EngineState.Lighting.EnableAmbient = enableAmbient;
                RenderManager.PushTechnique(this);
            }
            #endregion                     
        }

        public class Render : Render<DefferedLigthing<T>, T>            
        {
            ShaderInput sinput;
            Sprite sprite;          

            public Render()
            {
                sinput = Effect.Map<ShaderInput>();
                sprite = Service.Require<Sprite>();                
            }

            public override void Draw(DefferedLigthing<T> comp)
            {
                var effect = Effect;
                var scene = SceneManager.Scene;
                var device = GraphicDeviceFactory.Device;
                var enableAmbient = EngineState.Lighting.EnableAmbient;
                var activeLights = scene.ActiveLights;
                int nbLight = activeLights.Count;

                device.PS.SetResources(0, comp.textures);               

                #region Lighting

                sprite.Begin();
                sprite.SetFullScreenTransform(sinput);                

                if (nbLight > 0)
                    Light.Current = activeLights[0];

                //bind camera
                if (scene.ActiveCamera != null)
                    Bind(scene.ActiveCamera);

                //bind ambient lighting
                if (EngineState.Lighting.EnableAmbient)
                    Bind(scene.AmbientLight);

                Bind(activeLights[0]);    

                device.PS.SetSampler(0, SamplerState.Point);
                sinput.invViewProj = scene.ActiveCamera.InvViewProjection;
                sprite.DrawQuad(effect);
                
                if (nbLight > 1)
                {
                    EngineState.Lighting.EnableAmbient = false;
                    GraphicDeviceFactory.Device.Blend = SceneTechnique.Additive;

                    for (int k = 1; k < nbLight; k++)
                    {                        
                        Bind(activeLights[k]);                       
                        sprite.DrawQuad(effect);
                    }
                    
                }

                sprite.End();
                #endregion

                device.PS.SetResources(0, comp.textures.Length, null);
                Scene.UnBindScene(this);

                EngineState.Lighting.EnableAmbient = enableAmbient;
            }
        }
    }   
        

    //public class SceneDeferredTechnique : SceneTechnique
    //{
    //    TextureRT[] renderTargets = new TextureRT[4];
    //    DeferredCompositeRender render;

    //    public SceneDeferredTechnique()
    //        : base(true)
    //    {
    //        render = new DeferredCompositeRender();
    //        Initialize();
    //    }

    //    public override void Apply()
    //    {
    //        var rm = Engine.RenderManager;
    //        var oldRt = rm.GetRenderTarget2D(0);
    //        var device = GraphicDeviceFactory.Device;
    //        var scene = SceneManager.Scene;

    //        //update render render layers
    //        scene.UpdateRenderLists();

    //        //set render targets
    //        for (int i = 0; i < renderTargets.Length; i++)
    //            rm.SetRenderTarget2D(i, renderTargets[i]);

    //        //render opaque objects data
    //        device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, 0, 1, 0);
    //        RenderEntries(scene.RenderList, PixelClipping.Transparent);

    //        //render deferred data
    //        rm.SetRenderTarget2D(0, oldRt);
    //        render.Draw(renderTargets);

    //        //render transparent objects
    //        SetupTranspacencyBlending();
    //        RenderSceneRenderList(scene.TransparentRenderList, true, PixelClipping.Opaque);
                
    //    }

    //    protected override void OnLostDevice(SlimDX.Direct3D9.Device device)
    //    {
    //        foreach (var item in renderTargets)
    //        {
    //            if (item != null)
    //                item.Dispose();
    //        }

    //        base.OnLostDevice(device);
    //    }

    //    protected override void OnResetDevice(SlimDX.Direct3D9.Device device)
    //    {
    //        base.OnResetDevice(device);

    //        //Position G-Buffer
    //        renderTargets[0] = new TextureRT(Engine.BackBuffer.Width, Engine.BackBuffer.Height, Format.A16B16G16R16, Engine.BackBuffer.DepthStencilFormat, false);

    //        //Normal G-Buffer
    //        renderTargets[1] = new TextureRT(Engine.BackBuffer.Width, Engine.BackBuffer.Height, Format.A16B16G16R16);

    //        //Diffuse M-Buffer
    //        renderTargets[2] = new TextureRT(Engine.BackBuffer.Width, Engine.BackBuffer.Height, Format.A8R8G8B8);

    //        //Specular M-Buffer
    //        renderTargets[3] = new TextureRT(Engine.BackBuffer.Width, Engine.BackBuffer.Height, Format.A8R8G8B8);
    //    }

    //    protected override void OnDispose()
    //    {
    //        for (int i = 0; i < renderTargets.Length; i++)
    //        {
    //            renderTargets[i].Dispose();
    //        }
    //        base.OnDispose();
    //    }

    //    public static bool EnableDeferredLighting()
    //    {
    //        if (Engine.DeviceCapabititys.SimultaneousRTCount < 4) return false;

    //        Engine.RenderManager.PushTechnique<SceneDeferredTechnique>();
    //        return true;
    //    }
    //}

    //public class DeferredCompositeRender : Render<DeferredFinal>
    //{
    //    private EffectHandle hGlobalAmbient;
    //    private EffectHandle hDefault;
    //    private EffectHandle hHemisphere;

    //    public DeferredCompositeRender()
    //    {

    //    }

    //    protected override void InitializeBindings()
    //    {
    //        base.InitializeBindings();

    //        SetBinding(new ShadowMapBinding<ShadowMapTechnique>());
    //        SetBinding(new StaticShadowMapBinding());

    //        hGlobalAmbient = effect.GetTechique("GlobalAmbient");
    //        hDefault = effect.GetTechique("Default");
    //        hHemisphere = effect.GetGlobalParameter("hemisphere");
    //    }

    //    public void Draw(TextureRT[] renderTargets)
    //    {
    //        var scene = SceneManager.Scene;
    //        var rm = Engine.RenderManager;
    //        var rt = rm.GetRenderTarget2D(0);
    //        var quadRender = rm.QuadRender;
    //        quadRender.Effect = effect;
    //        var samples = rm.Samplers;


    //        //bind camera
    //        Bind(scene.ActiveCamera);
    //        //bind ambient
    //        Bind(scene.AmbientLight);

    //        int width = rt.Width;
    //        int height = rt.Height;
    //        bool hemisphere = EngineState.Lighting.HemisphericalAmbient;
    //        effect.SetValue(hHemisphere, hemisphere);

    //        //setup rendering data
    //        for (int i = 0; i < renderTargets.Length; i++)
    //            samples[i].SetState(renderTargets[i].Texture, TextureFilter.Point, TextureAddress.Clamp);

    //        //render Scene Ambient 
    //        effect.Technique = hGlobalAmbient;
    //        quadRender.Draw(width, height);

    //        //render lights 
    //        effect.Technique = hDefault;
    //        foreach (var light in scene.ActiveLights)
    //        {
    //            Bind(light);
    //            quadRender.Draw(width, height);
    //        }
    //    }

    //    public override void Draw(object component)
    //    {
    //        Draw((TextureRT[])component);
    //    }
    //}

}
