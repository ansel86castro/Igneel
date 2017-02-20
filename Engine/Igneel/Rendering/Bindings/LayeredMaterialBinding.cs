using Igneel.Components;
using Igneel.Components.Terrain;
using Igneel.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Rendering.Bindings
{
    public interface ILayeredMaterialMap
    {
        LayerSurface Surface { get; set; }

        SamplerArray<Texture2D> Layers { get; set; }

        Sampler<Texture2D> BlendLayer { get; set; }

        bool USE_DIFFUSE_MAP { get; set; }

        bool USE_SPECULAR_MAP { get; set; }
    }

    public class LayeredMaterialBinding : RenderBinding<LayeredMaterial, ILayeredMaterialMap>
    {
        public override void OnBind(LayeredMaterial value)
        {
            Mapping.Surface = value.Surface;

            if (value.DiffuseMaps != null)
            {
                Engine.Graphics.PS.SetSampler(0, SamplerState.Linear);
                Engine.Graphics.PS.SetResources(0, value.DiffuseMaps);

                Mapping.USE_DIFFUSE_MAP = true;
            }
            else
            {
                Mapping.USE_DIFFUSE_MAP = false;
            }

            if (value.BlendFactors != null)
            {
                Mapping.USE_SPECULAR_MAP = true;
                Mapping.BlendLayer = value.BlendFactors;
            }
            else
            {
                Mapping.USE_SPECULAR_MAP = false;
            }

           
        }

        public override void OnUnBind(LayeredMaterial value)
        {
          
        }
    }
}
