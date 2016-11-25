using System.Runtime.InteropServices;

namespace Igneel.Components
{
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