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
        RenderTexture2D[] downSampledRts = new RenderTexture2D[2];
        Effect effect;
        Sprite sprite;
        Igneel.Rendering.Sprite.IShaderInput input;
        SillueteRender silluteRender;
       
        public EdgeShadowFilteringTechnique()
        {
            Initialize();
            Engine.Graphics.OMBackBuffer.Resized += OMBackBuffer_Resized;
            effect = Effect.GetEffect<ShadowEdgeEffect>();
            sprite = Service.Get<Sprite>();
            input = effect.Map<Igneel.Rendering.Sprite.IShaderInput>();

            silluteRender = new SillueteRender();
        }

        void OMBackBuffer_Resized(Graphics.RenderTarget obj)
        {
            Initialize();
        }

        public Texture2D EdgeTexture { get { return downSampledRts[1].Texture; } }

        private void Initialize()
        {
            foreach (var downSampled in downSampledRts)
            {
                if (downSampled != null)
                    downSampled.Dispose();   
            }         

            var bb =Engine.Graphics.OMBackBuffer;
            var kernel = Engine.Shadow.ShadowMapping.PCFBlurSize;

            downSampledRts[0] = new RenderTexture2D(bb.Width / kernel, bb.Height / kernel, Graphics.Format.R8G8B8A8_UNORM);
            downSampledRts[1] = new RenderTexture2D(downSampledRts[0].Width / kernel, downSampledRts[0].Height / kernel, Graphics.Format.R8G8B8A8_UNORM);            
        }

        public override void Apply()
        {
            
            var graphic = Engine.Graphics;
            graphic.PSStage.SetResource(8, null);

            //render to silluete texture
            Engine.ApplyTechnique(silluteRender);

            #region DownSampling

            var edgeTexture = silluteRender.EdgeTexture;
            DownSample(graphic, edgeTexture, downSampledRts[0]);
            DownSample(graphic, downSampledRts[0], downSampledRts[1]);

            graphic.OMRestoreRenderTarget();

            #endregion

            base.Apply();
        }

        private void DownSample(GraphicDevice graphic, RenderTexture2D edgeTexture, RenderTexture2D downSampled)
        {           

            downSampled.SetTarget(graphic);
            edgeTexture.SetTexture(0, graphic);
            graphic.PSStage.SetSampler(0, SamplerState.Point);

            effect.Technique = 1;

            sprite.Begin();
            sprite.SetFullScreenTransform(input);
            sprite.DrawQuad(effect);
            sprite.End();

            effect.Technique = 0;
         
            graphic.PSStage.SetResource(0, null);
            graphic.OMSetRenderTarget(null);
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

                    Engine.Graphics.PSStage.SetResource(register, value.DepthTexture);
                    Engine.Graphics.PSStage.SetSampler(register, pointSampler);
                }
            }

            public override void OnUnBind(ShadowMapTechnique value)
            {
                Engine.Graphics.PSStage.SetResource(register, null);
            }
        }

        public class SillueteRender : SceneTechnique
        {
            RenderTexture2D edgeTexture;
            ViewPort vp;
            public RenderTexture2D EdgeTexture
            {
                get { return edgeTexture; }               
            }

            public SillueteRender()
            {
                Initialize();
                Engine.Graphics.OMBackBuffer.Resized += OMBackBuffer_Resized;
               
            }

            void OMBackBuffer_Resized(RenderTarget obj)
            {
                Initialize();
            }

            private void Initialize()
            {
                if (edgeTexture != null)
                    edgeTexture.Dispose();             
                var bb = Engine.Graphics.OMBackBuffer;
                var kernel = Engine.Shadow.ShadowMapping.PCFBlurSize;

                edgeTexture = new RenderTexture2D(bb.Width, bb.Height, Graphics.Format.R8G8B8A8_UNORM);
                vp = new ViewPort(0, 0, edgeTexture.Width, edgeTexture.Height);
            }

            public override void Apply()
            {
                var graphic = Engine.Graphics;

                graphic.OMSaveRenderTarget();
                var oldvp = graphic.RSViewPort;
                graphic.RSViewPort = vp;

                edgeTexture.SetTarget(graphic);
                graphic.Clear(Graphics.ClearFlags.Target | Graphics.ClearFlags.ZBuffer, new Color4(1, 0, 0, 0), 1, 0);
                base.Apply();

                graphic.OMSetRenderTarget(null);
                graphic.RSViewPort = oldvp;
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
                device.PSStage.SetResource(8, value.EdgeTexture);
                device.PSStage.SetSampler(8, SamplerState.Linear);
            }

            public override void OnUnBind(EdgeShadowFilteringTechnique value)
            {
                var device = Engine.Graphics;
                device.PSStage.SetResource(8, null);
                device.PSStage.SetSampler(8, null);
            }
        }
    }
}
