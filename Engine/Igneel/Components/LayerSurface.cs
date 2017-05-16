using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Components
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct LayerSurface
    {
        public Color4 Diffuse;

        public float Specular;

        public float Emisive;

        public float Reflectivity;

        public float Refractitity;

        public float SpecularPower;

        public static LayerSurface Default
        {
            get
            {
                return new LayerSurface()
                {
                    Diffuse = new Color4(1, 1, 1, 1),
                    Specular = 1,
                    Emisive = 0,
                    Reflectivity = 0,
                    Refractitity = 0,
                    SpecularPower = 4,
                };
            }
        }
    }
}
