using Igneel.Components;
using Igneel.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Rendering.Bindings
{
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
                    Filter = Filter.ComparisonMinMagLinearMipPoint,
                    ComparisonFunc = Comparison.Less,                 
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

    //public class StaticShadowMapBinding : ShadowMapBinding<StaticShadowMapTechnique>
    //{            
    //    public StaticShadowMapBinding()
    //    {
    //        register = 6;
    //    }
      
        
    //    public override void OnBind(StaticShadowMapTechnique value)
    //    {
    //        //if (Engine.Shadow.Enable)
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

    public class BuildSMapMatBinding : RenderBinding<MeshMaterial, IMeshMaterialMap>
    {        
     
        public override void OnBind(MeshMaterial value)
        {
            mapping.Surface = value.Surface;

            if (value.ContainsTrasparency && value.DiffuseMap != null)
            {
                mapping.DiffuseMap = value.DiffuseMap.ToSampler();
                mapping.USE_DIFFUSE_MAP = true;
            }
            else
            {
                mapping.USE_DIFFUSE_MAP = false;
                mapping.DiffuseMap = new Sampler<Texture2D>();
            }
        }

        public override void OnUnBind(MeshMaterial value)
        {
            mapping.USE_DIFFUSE_MAP = false;
        }
    }

}
