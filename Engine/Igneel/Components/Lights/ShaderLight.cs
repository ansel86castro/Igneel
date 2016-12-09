using System;
using System.Runtime.InteropServices;

namespace Igneel.Components
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
}
