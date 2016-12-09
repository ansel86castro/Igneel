using Igneel.Graphics;

namespace Igneel.Rendering.Bindings
{
    public interface IPlanarReflecionMap
    {
        bool USE_REFLECTION_MAP { get; set; }

        Sampler<Texture2D> ReflectionMap { get; set; }

        bool USE_REFRACTION_MAP { get; set; }

        Sampler<Texture2D> RefractionMap { get; set; }
    }
}