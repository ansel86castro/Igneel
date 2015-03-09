using Igneel.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Rendering.Bindings
{
    public class PlaneReflectionBinding : RenderBinding<ReflectiveNodeTechnique>
    {
       
        IPlanarReflecionMap mapping;

        protected override void OnEffectChanged(Effect effect)
        {
            base.OnEffectChanged(effect);

            mapping = effect.Map<IPlanarReflecionMap>();
        }

        public override void OnBind(ReflectiveNodeTechnique value)
        {
            if (Engine.Lighting.Reflection.Enable)
            {
                if (mapping != null)
                {
                    mapping.USE_REFLECTION_MAP = value.UseReflection;
                    mapping.USE_REFRACTION_MAP = value.UseRefraction;
                }

                if (value.UseReflection)
                {
                    mapping.ReflectionMap = value.ReflectionTexture.ToSampler();                    
                }
                if (value.UseRefraction)
                {
                    mapping.RefractionMap = value.RefractionTexture.ToSampler();
                }
            }
        }

        public override void OnUnBind(ReflectiveNodeTechnique value)
        {
            if (mapping != null && Engine.Lighting.Reflection.Enable)
            {
                mapping.ReflectionMap = Sampler<Texture2D>.Null;
                mapping.RefractionMap = Sampler<Texture2D>.Null;
            }
        }
    }
}
