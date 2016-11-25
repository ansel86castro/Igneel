using System.Runtime.InteropServices;

namespace Igneel.Components
{
    [StructLayout(LayoutKind.Sequential)]
    public struct ShaderDirectionalLight
    {
        public Vector3 Dir;
        public Vector3 Diffuse;
        public Vector3 Specular;
    }
}