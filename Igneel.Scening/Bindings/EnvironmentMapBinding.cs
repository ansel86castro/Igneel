using Igneel.Graphics;
using Igneel.Rendering;
using Igneel.Rendering.Bindings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Scenering.Bindings
{
    public class EnvironmentMapBinding : RenderBinding<EnvironmentMapTechnique>
    {        
        IEnvironmentMap mapping;     
        protected override void OnEffectChanged(Effect effect)
        {
            base.OnEffectChanged(effect);

            mapping = effect.Map<IEnvironmentMap>();          
        }

        public override void OnBind(EnvironmentMapTechnique value)
        {
            if (mapping != null)
            {
                if (EngineState.Lighting.Reflection.Enable)
                {                    
                    mapping.EnvironmentMap = value.Texture.ToSampler();
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
                mapping.EnvironmentMap = Sampler<Texture2D>.Null;                
                mapping.USE_ENVIROMENT_MAP = false;
            }
        }
    }
}
