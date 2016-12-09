using Igneel.Components;
using Igneel.Graphics;

namespace Igneel.Rendering.Bindings
{
    public interface IBasicMaterialMap
    {
        bool USE_DIFFUSE_MAP { get; set; }

        bool USE_SPECULAR_MAP { get; set; }

        LayerSurface Surface { get; set; }

        Sampler<Texture2D> DiffuseMap { get; set; }

        Sampler<Texture2D> SpecularMap { get; set; }

        Sampler<Texture2D> NormalMap { get; set; }

    }
}