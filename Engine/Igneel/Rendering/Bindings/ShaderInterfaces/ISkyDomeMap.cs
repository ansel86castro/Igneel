using Igneel.Graphics;

namespace Igneel.Rendering.Bindings
{
    public interface ISkyDomeMap : IViewProjectionMap
    {
        Sampler<Texture2D> DiffuseMap { get; set; }

        float LightIntensity { get; set; }
    }
}