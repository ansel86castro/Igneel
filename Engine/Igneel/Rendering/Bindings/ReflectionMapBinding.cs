using Igneel.Graphics;
using Igneel.States;
using Igneel.Techniques;

namespace Igneel.Rendering.Bindings
{
    public class PlaneReflectionBinding : RenderBinding<ReflectiveNodeTechnique>
    {
       
        IPlanarReflecionMap _mapping;

        protected override void OnEffectChanged(Effect effect)
        {
            base.OnEffectChanged(effect);

            _mapping = effect.Map<IPlanarReflecionMap>();
        }

        public override void OnBind(ReflectiveNodeTechnique value)
        {
            if (EngineState.Lighting.Reflection.Enable)
            {
                if (_mapping != null)
                {
                    _mapping.USE_REFLECTION_MAP = value.UseReflection;
                    _mapping.USE_REFRACTION_MAP = value.UseRefraction;
                }

                if (value.UseReflection)
                {
                    _mapping.ReflectionMap = value.ReflectionTexture.ToSampler();                    
                }
                if (value.UseRefraction)
                {
                    _mapping.RefractionMap = value.RefractionTexture.ToSampler();
                }
            }
        }

        public override void OnUnBind(ReflectiveNodeTechnique value)
        {
            if (_mapping != null && EngineState.Lighting.Reflection.Enable)
            {
                _mapping.ReflectionMap = Sampler<Texture2D>.Null;
                _mapping.RefractionMap = Sampler<Texture2D>.Null;
            }
        }
    }
}
