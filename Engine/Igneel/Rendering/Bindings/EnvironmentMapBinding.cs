using Igneel.Graphics;
using Igneel.States;
using Igneel.Techniques;

namespace Igneel.Rendering.Bindings
{
    public class EnvironmentMapBinding : RenderBinding<EnvironmentMapTechnique>
    {        
        IEnvironmentMap _mapping;     
        protected override void OnEffectChanged(Effect effect)
        {
            base.OnEffectChanged(effect);

            _mapping = effect.Map<IEnvironmentMap>();          
        }

        public override void OnBind(EnvironmentMapTechnique value)
        {
            if (_mapping != null)
            {
                if (EngineState.Lighting.Reflection.Enable)
                {                    
                    _mapping.EnvironmentMap = value.Texture.ToSampler();
                    _mapping.USE_ENVIROMENT_MAP = true;
                }
                else
                    _mapping.USE_ENVIROMENT_MAP = false;
            }
        }

        public override void OnUnBind(EnvironmentMapTechnique value)
        {
            if (_mapping != null && _mapping.USE_ENVIROMENT_MAP)
            {
                _mapping.EnvironmentMap = Sampler<Texture2D>.Null;
                _mapping.USE_ENVIROMENT_MAP = false;
            }
        }
    }
}
