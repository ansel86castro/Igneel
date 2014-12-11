using Igneel.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Rendering
{
    public class EdgeShadowFilteringTechnique : SceneTechnique
    {
        RenderTexture2D edgeTexture;

        public EdgeShadowFilteringTechnique()
        {
            Initialize();
            Engine.Graphics.OMBackBuffer.Resized += OMBackBuffer_Resized;
        }

        void OMBackBuffer_Resized(Graphics.RenderTarget obj)
        {
            Initialize();
        }

        public Texture2D EdgeTexture { get { return edgeTexture.Texture; } }

        private void Initialize()
        {
            if (edgeTexture != null)
                edgeTexture.Dispose();

            int kernel = 3;
            var bb =Engine.Graphics.OMBackBuffer;
            edgeTexture = new RenderTexture2D(bb.Width / kernel, bb.Height / kernel, Graphics.Format.R8G8B8A8_UNORM);

        }

        public override void Apply()
        {
            //render to edgeTexture
            var graphic = Engine.Graphics;

            graphic.OMSaveRenderTarget();

            edgeTexture.SetTarget(graphic);
            graphic.Clear(Graphics.ClearFlags.Target | Graphics.ClearFlags.ZBuffer, new Color4(1, 0.2f, 0.2f, 0.2f), 1, 0);

            base.Apply();

            graphic.OMRestoreRenderTarget();
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
    }
}
