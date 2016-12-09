using Igneel.Graphics;

namespace Igneel.Rendering.Bindings
{
    public interface IEnvironmentMap
    {
        Sampler<Texture2D> EnvironmentMap { get; set; }

        bool USE_ENVIROMENT_MAP { get; set; }
    }
}