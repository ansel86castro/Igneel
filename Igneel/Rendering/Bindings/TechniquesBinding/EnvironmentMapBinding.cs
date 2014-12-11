using Igneel.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Rendering.Bindings
{
    public class EnvironmentMapBinding : RenderBinding<EnvironmentMapTechnique>
    {
        public interface IEnvMap
        {
            bool USE_ENVIROMENT_MAP { get; set; }
        }

        IEnvMap mapping;
        const int ReflectionSampler  = 4;
        private ShaderStage stage;

        protected override void OnEffectChanged(Effect effect)
        {
            base.OnEffectChanged(effect);

            mapping = effect.Map<IEnvMap>();
            stage = Engine.Graphics.PSStage;
        }

        public override void OnBind(EnvironmentMapTechnique value)
        {
            if (mapping != null)
            {
                if (Engine.Lighting.Reflection.Enable)
                {
                    stage.SetResource(ReflectionSampler, value.Texture);
                    stage.SetSampler(ReflectionSampler, SamplerState.Linear);
                    mapping.USE_ENVIROMENT_MAP = true;
                }
                else
                    mapping.USE_ENVIROMENT_MAP = false;
            }
        }

        public override void OnUnBind(EnvironmentMapTechnique value)
        {
            if (mapping != null && mapping.USE_ENVIROMENT_MAP)
            {
                stage.SetResource(ReflectionSampler, null);
                //stage.SetSampler(ReflectionSampler, SamplerState.Linear);
                mapping.USE_ENVIROMENT_MAP = false;
            }
        }
    }
}
