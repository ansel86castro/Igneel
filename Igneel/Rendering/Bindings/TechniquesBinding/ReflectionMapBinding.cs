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
        public interface IMapping
        {
            bool USE_REFLECTION_MAP { get; set; }
            bool USE_REFRACTION_MAP { get; set; }
        }

        IMapping mapping;

        protected override void OnEffectChanged(Effect effect)
        {
            base.OnEffectChanged(effect);

            mapping = effect.Map<IMapping>();
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
                    Engine.Graphics.PSStage.SetResource(5, value.ReflectionTexture);
                    Engine.Graphics.PSStage.SetSampler(5, SamplerState.Linear);
                }
                if (value.UseRefraction)
                {
                    Engine.Graphics.PSStage.SetResource(6, value.RefractionTexture);
                    Engine.Graphics.PSStage.SetSampler(6, SamplerState.Linear);
                }
            }
        }

        public override void OnUnBind(ReflectiveNodeTechnique value)
        {
            if (mapping != null && Engine.Lighting.Reflection.Enable)
            {
                Engine.Graphics.PSStage.SetResource(5, null);
                Engine.Graphics.PSStage.SetResource(6, null);
            }
        }
    }
}
