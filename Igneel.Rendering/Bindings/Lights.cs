using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Rendering.Bindings
{
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct ShaderLight
    {
        public Vector3 Pos;
        private float pad0;

        public Vector3 Dir;
        private float pad1;

        public Vector3 Diffuse;
        private float pad2;

        public Vector3 Specular;
        private float pad3;

        public Vector3 Att; // a0 a1,a2 attenuation factors
        public float SpotPower;
        public float Range;
        public int Type;
        public float Intensity;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct ShaderDirectionalLight
    {
        public Vector3 Dir;
        public Vector3 Diffuse;
        public Vector3 Specular;
    }
    [StructLayout(LayoutKind.Sequential)]
    public struct ShaderPointLight
    {
        public Vector3 Pos;
        public Vector3 Diffuse;
        public Vector3 Specular;
        public Vector3 Attenuation; // a0 a1,a2 attenuation factors      
        public float Range;
    }
}
