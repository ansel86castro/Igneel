using Igneel.Components;
using Igneel.Graphics;
using Igneel.States;
using Igneel.Techniques;

namespace Igneel.Rendering.Bindings
{
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
                    Filter = Filter.ComparisonMinMagLinearMipPoint,
                    ComparisonFunc = Comparison.Less,                 
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

    //public class StaticShadowMapBinding : ShadowMapBinding<StaticShadowMapTechnique>
    //{            
    //    public StaticShadowMapBinding()
    //    {
    //        register = 6;
    //    }
      
        
    //    public override void OnBind(StaticShadowMapTechnique value)
    //    {
    //        //if (EngineState.Shadow.Enable)
    //        //{
    //        //    var effect = Effect;
    //        //    effect.SetValue(htexSize, 1.0f / (float)value.size);
    //        //    effect.SetValue(hEpsilon, value.bias);
    //        //    effect.SetValue(hViewProj, value.camera.ViewProj);                
    //        //    effect.SetValue(hSMFlag, true);

    //        //    Engine.RenderManager.Samplers[register].SetState(value.DepthTexture, TextureFilter.Point, TextureFilter.Point, TextureFilter.Point, unchecked((int)0xFFFFFFFF));
    //        //}
    //    }

    //    public override void OnUnBind(StaticShadowMapTechnique value)
    //    {
    //        //base.UnBind(value);
    //        //Effect.SetValue(hSMFlag, true);
    //    }
    //}

    public class BuildSMapMatBinding : Igneel.Rendering.RenderBinding<BasicMaterial, IBasicMaterialMap>
    {        
     
        public override void OnBind(BasicMaterial value)
        {
            Mapping.Surface = value.Surface;

            if (value.ContainsTrasparency && value.DiffuseMap != null)
            {
                Mapping.DiffuseMap = value.DiffuseMap.ToSampler();
                Mapping.USE_DIFFUSE_MAP = true;
            }
            else
            {
                Mapping.USE_DIFFUSE_MAP = false;
                Mapping.DiffuseMap = new Sampler<Texture2D>();
            }
        }

        public override void OnUnBind(BasicMaterial value)
        {
            Mapping.USE_DIFFUSE_MAP = false;
        }
    }

}
