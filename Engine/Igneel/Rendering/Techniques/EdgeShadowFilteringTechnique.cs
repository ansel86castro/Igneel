using Igneel.Effects;
using Igneel.Graphics;
using Igneel.Rendering;
using Igneel.States;

namespace Igneel.Techniques
{
    public class EdgeShadowFilteringTechnique : DefaultTechnique
    {        
        RenderTexture2D[] _downSampledRts = new RenderTexture2D[3];
        Effect _effect;
        Sprite _sprite;
        Igneel.Rendering.Sprite.IShaderInput _input;
        SillueteRender _silluteRender;
       
        public EdgeShadowFilteringTechnique()
        {
            Initialize();
            GraphicDeviceFactory.Device.BackBuffer.Resized += OMBackBuffer_Resized;
            _effect = Effect.GetEffect<ShadowEdgeEffect>(GraphicDeviceFactory.Device);
            _sprite = Service.Get<Sprite>();
            _input = _effect.Map<Igneel.Rendering.Sprite.IShaderInput>();

            _silluteRender = new SillueteRender();
        }

        void OMBackBuffer_Resized(Graphics.RenderTarget obj)
        {
            Initialize();
        }

        public Texture2D EdgeTexture { get { return _downSampledRts[2].Texture; } }

        public Texture2D EdgeSrcTexture { get { return _downSampledRts[1].Texture; } }

        public Texture2D ShadowFactorTex { get { return _silluteRender.ShadowFatorTex.Texture; } }

        private void Initialize()
        {
            foreach (var downSampled in _downSampledRts)
            {
                if (downSampled != null)
                    downSampled.Dispose();   
            }           

            var bb =GraphicDeviceFactory.Device.BackBuffer;
            var kernel = EngineState.Shadow.ShadowMapping.PcfBlurSize;
            
            int w = bb.Width;
            int h = bb.Height;
            for (int i = 0; i < _downSampledRts.Length; i++)
            {                
                _downSampledRts[i] = new RenderTexture2D(w, h, Graphics.Format.R8G8B8A8_UNORM);
                h /= kernel;
                w /= kernel;
            }            
        }

        public override void Apply()
        {
            
            var graphic = GraphicDeviceFactory.Device;
            var ps = graphic.PS;
            ps.SetResource(8, null);

            //render to shadow factor texture
            RenderManager.ApplyTechnique(_silluteRender);

            #region render to edgeTexture

            var edgeTexture = _silluteRender.ShadowFatorTex;

            _downSampledRts[0].SetTarget(graphic);
            edgeTexture.SetTexture(0, graphic);
            _effect.Technique = 1;
            ps.SetSampler(0, SamplerState.Point);

            _sprite.Begin();
            _sprite.SetFullScreenTransform(_input);
            _sprite.DrawQuad(_effect);
            _sprite.End();

            #endregion

            #region DownSampling
            ps.SetSampler(0, SamplerState.Point);

            _effect.Technique = 2;
            _sprite.Begin();
            for (int i = 0; i < _downSampledRts.Length - 1; i++)
            {
                DownSample(graphic, _downSampledRts[i], _downSampledRts[i + 1]);
                ps.SetResource(0, null);  
            }
            _sprite.End();  
            _effect.Technique = 0;                  
            graphic.RestoreRenderTarget();

            #endregion

            base.Apply();
        }

        private void DownSample(GraphicDevice graphic, RenderTexture2D src, RenderTexture2D dest)
        {           
            dest.SetTarget(graphic);
            src.SetTexture(0, graphic);                                 
            _sprite.DrawQuad(_effect);                                        
        }

        public class ShadowMapBinding : RenderBinding<ShadowMapTechnique>
        {
            static SamplerState _pointSampler;

            public interface ISmMap
            {
                Matrix LightVp { get; set; }
                float ShadowEpsilon { get; set; }
                float SmSize { get; set; }
            }

            protected int Register = 7;
            ISmMap _mapping;

            public ShadowMapBinding()
            {
                if (_pointSampler == null)
                    _pointSampler = GraphicDeviceFactory.Device.CreateSamplerState(new SamplerDesc(true)
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

                _mapping = effect.Map<ISmMap>();
            }

            public ShadowMapBinding(int register)
            {
                this.Register = register;
            }

            public override void OnBind(ShadowMapTechnique value)
            {
                if (EngineState.Shadow.Enable)
                {
                    _mapping.LightVp = value.camera.ViewProj;
                    _mapping.ShadowEpsilon = value.bias;
                    _mapping.SmSize = value.size;

                    GraphicDeviceFactory.Device.PS.SetResource(Register, value.DepthTexture);
                    GraphicDeviceFactory.Device.PS.SetSampler(Register, _pointSampler);
                }
            }

            public override void OnUnBind(ShadowMapTechnique value)
            {
                GraphicDeviceFactory.Device.PS.SetResource(Register, null);
            }
        }

        public class SillueteRender : DefaultTechnique
        {
            RenderTexture2D _shadowRt;
            ViewPort _vp;

            public RenderTexture2D ShadowFatorTex
            {
                get { return _shadowRt; }               
            }
           
            public SillueteRender()
            {
                Initialize();
                GraphicDeviceFactory.Device.BackBuffer.Resized += OMBackBuffer_Resized;                

            }

            void OMBackBuffer_Resized(RenderTarget obj)
            {
                Initialize();
            }

            private void Initialize()
            {
                if (_shadowRt != null)
                    _shadowRt.Dispose();             
                var bb = GraphicDeviceFactory.Device.BackBuffer;
                var kernel = EngineState.Shadow.ShadowMapping.PcfBlurSize;

                _shadowRt = new RenderTexture2D(bb.Width, bb.Height, Format.R8G8B8A8_UNORM);
                _vp = new ViewPort(0, 0, _shadowRt.Width, _shadowRt.Height);
            }

            public override void Apply()
            {
                var graphic = GraphicDeviceFactory.Device;

                graphic.SaveRenderTarget();
                var oldvp = graphic.ViewPort;
                graphic.ViewPort = _vp;

                _shadowRt.SetTarget(graphic);
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
                var device = GraphicDeviceFactory.Device;
                device.PS.SetResource(8, value.EdgeTexture);
                device.PS.SetSampler(8, SamplerState.Point);
            }

            public override void OnUnBind(EdgeShadowFilteringTechnique value)
            {
                var device = GraphicDeviceFactory.Device;
                device.PS.SetResource(8, null);
                device.PS.SetSampler(8, null);
            }
        }
    }
}
