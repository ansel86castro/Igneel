using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Igneel.Components.Particles
{
    [StructLayout(LayoutKind.Sequential)]
    [Serializable]
    public struct Particle
    {
        public Vector3 GlobalPosition;
        public Vector3 GlobalVelocity;
        public float Mass;
        public float InvMass;
        public float RotationAngle;
        public float AngularVelocity;

        public float Size;
        public uint Color;
        public float Life;
        public float Alpha;
    }

}
