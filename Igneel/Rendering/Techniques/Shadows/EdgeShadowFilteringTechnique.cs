using Igneel.Graphics;
using Igneel.Rendering.Effects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Rendering
{
    public class EdgeShadowFilteringTechnique : SceneTechnique
    {        
        RenderTexture2D[] downSampledRts = new RenderTexture2D[3];
        Effect effect;
        Sprite sprite;
        Igneel.Rendering.Sprite.IShaderInput input;
        SillueteRender silluteRender;
       
        public EdgeShadowFilteringTechnique()
        {
            Initialize();
            Engine.Graphics.BackBuffer.Resized += OMBackBuffer_Resized;
            effect = Effect.GetEffect<ShadowEdgeEffect>();
            sprite = Service.Get<Sprite>();
            input = effect.Map<Igneel.Rendering.Sprite.IShaderInput>();

            silluteRender = new SillueteRender();
        }

        void OMBackBuffer_Resized(Graphics.RenderTarget obj)
        {
            Initialize();
        }

        public Texture2D EdgeTexture { get { return downSampledRts[2].Texture; } }

        public Texture2D EdgeSrcTexture { get { return downSampledRts[1].Texture; } }

        public Texture2D ShadowFactorTex { get { return silluteRender.ShadowFatorTex.Texture; } }

        private void Initialize()
        {
            foreach (var downSampled in downSampledRts)
            {
                if (downSampled != null)
                    downSampled.Dispose();   
            }           

            var bb =Engine.Graphics.BackBuffer;
            var kernel = Engine.Shadow.ShadowMapping.PCFBlurSize;
            
            int w = bb.Width;
            int h = bb.Height;
            for (int i = 0; i < downSampledRts.Length; i++)
            {                
                downSampledRts[i] = new RenderTexture2D(w, h, Graphics.Format.R8G8B8A8_UNORM);
                h /= kernel;
                w /= kernel;
            }            
        }

        public override void Apply()
        {
            
            var graphic = Engine.Graphics;
            var ps = graphic.PS;
            ps.SetResource(8, null);

            //render to shadow factor texture
            Engine.ApplyTechnique(silluteRender);

            #region render to edgeTexture

            var edgeTexture = silluteRender.ShadowFatorTex;

            downSampledRts[0].SetTarget(graphic);
            edgeTexture.SetTexture(0, graphic);
            effect.Technique = 1;
            ps.SetSampler(0, SamplerState.Point);

            sprite.Begin();
            sprite.SetFullScreenTransform(input);
            sprite.DrawQuad(effect);
            sprite.End();

            #endregion

            #region DownSampling
            ps.SetSampler(0, SamplerState.Point);

            effect.Technique = 2;
            sprite.Begin();
            for (int i = 0; i < downSampledRts.Length - 1; i++)
            {
                DownSample(graphic, downSampledRts[i], downSampledRts[i + 1]);
                ps.SetResource(0, null);  
            }
            sprite.End();  
            effect.Technique = 0;                  
            graphic.RestoreRenderTarget();

            #endregion

            base.Apply();
        }

        private void DownSample(GraphicDevice graphic, RenderTexture2D src, RenderTexture2D dest)
        {           
            dest.SetTarget(graphic);
            src.SetTexture(0, graphic);                                 
            sprite.DrawQuad(effect);                                        
        }

        public class ShadowMapBinding : RenderBinding<ShadowMapTechnique>
        {
            static SamplerState pointSampler;

            public interface ISMMap
            {
                Matrix LightVP { get; set; }
                float SHADOW_EPSILON { get; set; }
                float SM_SIZE { get; set; }
            }

            protected int register = 7;
            ISMMap mapping;

            public ShadowMapBinding()
            {
                if (pointSampler == null)
                    pointSampler = Engine.Graphics.CreateSamplerState(new SamplerDesc(true)
                    {
                        AddressV = TextureAddressMode.Border,
                        AddressU = TextureAddressMode.Border,
                        BorderColor = new Color4(1, 1, 1, 1),
                        Filter = Filter.MinMagMipPoint,                        
                    });
            }

            protected override void OnEffectChanged(Effect effect)
            {
                base.OnEffectChanged(effect);

                mapping = effect.Map<ISMMap>();
            }

            public ShadowMapBinding(int register)
            {
                this.register = register;
            }

            public override void OnBind(ShadowMapTechnique value)
            {
                if (Engine.Shadow.Enable)
                {
                    mapping.LightVP = value.camera.ViewProj;
                    mapping.SHADOW_EPSILON = value.bias;
                    mapping.SM_SIZE = value.size;

                    Engine.Graphics.PS.SetResource(register, value.DepthTexture);
                    Engine.Graphics.PS.SetSampler(register, pointSampler);
                }
            }

            public override void OnUnBind(ShadowMapTechnique value)
            {
                Engine.Graphics.PS.SetResource(register, null);
            }
        }

        public class SillueteRender : SceneTechnique
        {
            RenderTexture2D shadowRt;
            ViewPort vp;

            public RenderTexture2D ShadowFatorTex
            {
                get { return shadowRt; }               
            }
           
            public SillueteRender()
            {
                Initialize();
                Engine.Graphics.BackBuffer.Resized += OMBackBuffer_Resized;                

            }

            void OMBackBuffer_Resized(RenderTarget obj)
            {
                Initialize();
            }

            private void Initialize()
            {
                if (shadowRt != null)
                    shadowRt.Dispose();             
                var bb = Engine.Graphics.BackBuffer;
                var kernel = Engine.Shadow.ShadowMapping.PCFBlurSize;

                shadowRt = new RenderTexture2D(bb.Width, bb.Height, Format.R8G8B8A8_UNORM);
                vp = new ViewPort(0, 0, shadowRt.Width, shadowRt.Height);
            }

            public override void Apply()
            {
                var graphic = Engine.Graphics;

                graphic.SaveRenderTarget();
                var oldvp = graphic.ViewPort;
                graphic.ViewPort = vp;

                shadowRt.SetTarget(graphic);
                graphic.Clear(Graphics.ClearFlags.Target | Graphics.ClearFlags.ZBuffer, new Color4(0, 0, 0, 0), 1, 0);
                base.Apply();             

                graphic.SetRenderTarget(null);
                graphic.ViewPort = oldvp;
            }
        }

        public override void Bind(Render render)
        {
            render.Bind(this);
        }

        public override void UnBind(Render render)
        {
            render.UnBind(this);
        }

        public class Binding : RenderBinding<EdgeShadowFilteringTechnique>
        {

            public override void OnBind(EdgeShadowFilteringTechnique value)
            {
                var device = Engine.Graphics;
                device.PS.SetResource(8, value.EdgeTexture);
                device.PS.SetSampler(8, SamplerState.Point);
            }

            public override void OnUnBind(EdgeShadowFilteringTechnique value)
            {
                var device = Engine.Graphics;
                device.PS.SetResource(8, null);
                device.PS.SetSampler(8, null);
            }
        }
    }
}
