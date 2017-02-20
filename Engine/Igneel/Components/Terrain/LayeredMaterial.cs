using Igneel.Assets;
using Igneel.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Components.Terrain
{
    public class LayeredMaterial : SurfaceMaterial
    {
        Texture2D[] _layers;
        Texture2D _blendLayer;
        private bool _containsAlpha;

        public LayeredMaterial() { }

        public LayeredMaterial(string name) : base(name, null) { }

        public LayeredMaterial(string name, ResourceManager manager) : base(name, manager) { }

        public Texture2D[] DiffuseMaps { get { return _layers; } set { _layers = value; } }

        public Texture2D BlendFactors { get { return _blendLayer; } set { _blendLayer = value; } }

        public Texture2D[] NormalMaps { get; set; }

        public Texture2D[] SpecularMaps { get; set; }

        public bool ContainsTrasparency { get { return _containsAlpha; } }

        protected override void OnDispose(bool disposing)
        {
            if (disposing)
            {
                if (_layers != null)
                {
                    foreach (var item in _layers)
                    {
                        if (item != null)
                            item.Dispose();
                    }
                }
                if (_blendLayer != null)
                    _blendLayer.Dispose();

                if (NormalMaps != null)
                {
                    foreach (var item in NormalMaps)
                    {
                        if (item != null)
                            item.Dispose();
                    }                   
                }
                if (SpecularMaps != null)
                {
                    foreach (var item in SpecularMaps)
                    {
                        if (item != null)
                            item.Dispose();
                    }
                }

                DiffuseMaps = null;
                NormalMaps = null;
                SpecularMaps = null;
                BlendFactors = null;
            }
        }
    }
}
